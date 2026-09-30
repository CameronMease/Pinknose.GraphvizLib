/////////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright(c) 2022-2026 Cameron Mease
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pinknose.GraphvizLib
{
    public enum GraphvizEngine
    {
        /// <summary>Hierarchical layout, for directed graphs.</summary>
        Dot,

        /// <summary>Force-directed layout.</summary>
        Fdp,

        /// <summary>Spring-model layout.</summary>
        Neato,

        /// <summary>Circular layout.</summary>
        Circo,

        /// <summary>Radial layout.</summary>
        Twopi
    }

    /// <summary>Runs an installed Graphviz to render a graph.</summary>
    public static class DotExe
    {
        #region Fields

        private static readonly object PathLock = new();

        private static string? _executablePath;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Full path to the Graphviz <c>dot</c> executable. Found on first use by searching the PATH
        /// and then the default Windows install folder; set it to use a particular Graphviz.
        /// </summary>
        /// <exception cref="FileNotFoundException">Graphviz could not be found.</exception>
        public static string ExecutablePath
        {
            get
            {
                lock (PathLock)
                {
                    return _executablePath ??= FindExecutable()
                        ?? throw new FileNotFoundException(
                            "Graphviz was not found. Install it from https://graphviz.org/download/ and add its bin folder to the PATH, or set DotExe.ExecutablePath.");
                }
            }

            set
            {
                if (!File.Exists(value))
                {
                    throw new FileNotFoundException("No Graphviz executable at that path.", value);
                }

                lock (PathLock)
                {
                    _executablePath = value;
                }
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Renders a graph in any Graphviz output format (<c>svg</c>, <c>png</c>, <c>pdf</c>, ...).
        /// </summary>
        /// <exception cref="GraphvizException">Graphviz exited with an error.</exception>
        public static async Task<byte[]> RenderAsync(
            Graph graph,
            string format,
            GraphvizEngine engine = GraphvizEngine.Dot,
            CancellationToken cancellationToken = default)
        {
            if (graph is null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            if (string.IsNullOrWhiteSpace(format))
            {
                throw new ArgumentException("A format is required.", nameof(format));
            }

            var dot = graph.RenderDot();
            var imagePaths = new List<string>();

            try
            {
                var source = WriteImages(dot, imagePaths);
                return await RunAsync(source, format, engine, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                foreach (var path in imagePaths)
                {
                    TryDelete(path);
                }
            }
        }

        private static string? FindExecutable()
        {
            var fileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "dot.exe" : "dot";

            var pathVariable = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            foreach (var directory in pathVariable.Split(Path.PathSeparator))
            {
                if (directory.Length == 0)
                {
                    continue;
                }

                var candidate = Path.Combine(directory.Trim('"'), fileName);
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                var candidate = Path.Combine(programFiles, "Graphviz", "bin", fileName);
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }

            return null;
        }

        private static async Task<byte[]> RunAsync(string source, string format, GraphvizEngine engine, CancellationToken cancellationToken)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = ExecutablePath,
                Arguments = $"-K{engine.ToString().ToLowerInvariant()} -T{format}",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardErrorEncoding = Encoding.UTF8,
            };

            using var process = new Process { StartInfo = startInfo };
            process.Start();

            using var registration = cancellationToken.Register(() => TryKill(process));

            // Read both streams while writing input: a large graph fills the stdout pipe before
            // stdin is finished, and reading them one after the other deadlocks.
            var output = new MemoryStream();
            var readOutput = process.StandardOutput.BaseStream.CopyToAsync(output);
            var readError = process.StandardError.ReadToEndAsync();

            var input = new StreamWriter(process.StandardInput.BaseStream, new UTF8Encoding(false));
            await input.WriteAsync(source).ConfigureAwait(false);
            await input.FlushAsync().ConfigureAwait(false);
            input.Close();

            await readOutput.ConfigureAwait(false);
            var error = await readError.ConfigureAwait(false);
            await Task.Run(() => process.WaitForExit()).ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            if (process.ExitCode != 0)
            {
                throw new GraphvizException(process.ExitCode, error.Trim());
            }

            return output.ToArray();
        }

        private static void TryDelete(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (IOException)
            {
                // A temp file left behind is not worth failing a render over.
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        private static void TryKill(Process process)
        {
            try
            {
                if (!process.HasExited)
                {
                    process.Kill();
                }
            }
            catch (InvalidOperationException)
            {
                // Exited between the check and the kill.
            }
        }

        /// <summary>
        /// Writes each image an HTML label uses to a temp file and puts its path in the source.
        /// Graphviz can only read images from disk.
        /// </summary>
        private static string WriteImages(Dot dot, List<string> imagePaths)
        {
            var source = dot.DotSource;

            if (dot.ImageCache is null)
            {
                return source;
            }

            foreach (var image in dot.ImageCache)
            {
                var placeholder = Html.HtmlImageCache.PlaceholderFor(image.Key);
                if (source.IndexOf(placeholder, StringComparison.Ordinal) < 0)
                {
                    continue;
                }

                var path = Path.Combine(Path.GetTempPath(), $"graphviz-{Guid.NewGuid():N}.{image.Value.Extension}");
                File.WriteAllBytes(path, image.Value.Bytes);
                imagePaths.Add(path);

                // The path sits inside an HTML attribute, so it is escaped like one.
                source = source.Replace(placeholder, path.Replace("&", "&amp;").Replace("\"", "&quot;"));
            }

            return source;
        }

        #endregion Methods
    }
}
