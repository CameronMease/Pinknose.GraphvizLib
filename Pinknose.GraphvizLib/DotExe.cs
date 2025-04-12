/////////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright(c) 2022-2023 Cameron Mease
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

using SkiaSharp;
using Svg;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Pinknose.GraphvizLib
{
    public enum GraphvizEngine
    { Dot, Fdp }

    public static class DotExe
    {
        private static bool ExistsOnPath(string fileName)
        {
            return GetFullPath(fileName) != null;
        }

        private static string? GetFullPath(string fileName)
        {
            if (File.Exists(fileName))
                return Path.GetFullPath(fileName);

            var values = Environment.GetEnvironmentVariable("PATH") ?? throw new InvalidOperationException("PATH environment variable is not set.");

            foreach (var path in values.Split(Path.PathSeparator))
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                    return fullPath;
            }
            return null;
        }

        #region Fields

        private static readonly Lazy<string> DotPath = new(() =>
        {
            string? path = null;

            if (path == null)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    path = GetFullPath("dot.exe");
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    path = GetFullPath("dot");
                }
            }

            if (path == null)
            {
                throw new DirectoryNotFoundException($"Path to Graphviz bin directory ({path}) does not exist. Did you install Graphviz and add it to the system path?");
            }

            return path;
        });

        #endregion Fields

        #region Methods

        public static async Task<SKBitmap> RenderPngAsync(Graph graph, GraphvizEngine engine)
        {
            using var stream = await RenderAsync(graph, "png", engine).ConfigureAwait(false);

            stream.Seek(0, SeekOrigin.Begin);

            var bitmap = SKBitmap.Decode(stream);

            return bitmap;
        }

        public static async Task<SvgDocument> RenderSvgAsync(Graph graph, GraphvizEngine engine)
        {
            using var stream = await RenderAsync(graph, "svg", engine).ConfigureAwait(false);
            stream.Seek(0, SeekOrigin.Begin);

            var svg = SvgDocument.Open<SvgDocument>(stream);

            return svg;
        }

        public static async Task RenderToPngFileAsync(Graph graph, GraphvizEngine engine, string filename)
        {
            using var stream = await RenderAsync(graph, "png", engine).ConfigureAwait(false);

            stream.Seek(0, SeekOrigin.Begin);

            using var filestream = File.Create(filename);

            await stream.CopyToAsync(filestream);

            filestream.Close();
        }

        public static async Task RenderToSvgFileAsync(Graph graph, GraphvizEngine engine, string filename)
        {
            using var stream = await RenderAsync(graph, "svg", engine).ConfigureAwait(false);

            stream.Seek(0, SeekOrigin.Begin);

            using var filestream = File.Create(filename);

            await stream.CopyToAsync(filestream);

            filestream.Close();
        }

        private static async Task<Stream> RenderAsync(Graph graph, string type, GraphvizEngine engine)
        {
            Dictionary<string, string> imageFilePathByGuid = [];

            bool errorRunningProcess = false;

            var dot = graph.RenderDot();

            // Save embedded image files
            var tempDir = Path.GetTempPath();

            var newDotString = dot.DotSource;

            foreach (var guid in dot.ImageCache.Keys)
            {
                var imageDescription = dot.ImageCache[guid];
                var guidString = guid.ToString();
                var tempFilePath = Path.Combine(tempDir, Path.GetTempFileName());
                tempFilePath = Path.ChangeExtension(tempFilePath, imageDescription.Extension);

                imageFilePathByGuid.Add(guidString, tempFilePath);
#if NET6_0_OR_GREATER
                await File.WriteAllBytesAsync(tempFilePath, imageDescription.Bytes);
#else
                File.WriteAllBytes(tempFilePath, imageDescription.Bytes);
#endif

                var filePlaceholder = $"<GUID>{guidString}</GUID>";

                newDotString = newDotString.Replace(filePlaceholder, tempFilePath);
            }

            var proc = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = false,
                    WorkingDirectory = Path.GetDirectoryName(DotPath.Value),
                    FileName = Path.GetFileName(DotPath.Value),
                    Arguments = $@"-T{type}",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                }
            };

            proc.ErrorDataReceived += (sender, e) =>
            {
                errorRunningProcess = true;
                Debug.Write(e.Data);
            };

            proc.OutputDataReceived += (sender, e) =>
            {
            };

            try
            {
                proc.Start();

                proc.StandardInput.WriteLine(newDotString);
                proc.StandardInput.Flush();
                proc.StandardInput.Close();

                MemoryStream memoryStream = new();

                byte[] buffer = new byte[8192];
                int lastBytesRead = 0;
                int totalBytesRead = 0;

                do
                {
                    lastBytesRead = await proc.StandardOutput.BaseStream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                    totalBytesRead += lastBytesRead;
                    memoryStream.Write(buffer, 0, lastBytesRead);
                } while (lastBytesRead > 0);

#if NET5_0_OR_GREATER
                await proc.WaitForExitAsync();
#else
                proc.WaitForExit();
#endif

                if (errorRunningProcess)
                {
                    Debug.WriteLine("Error occurred while running DOT process.");
                }

                //memoryStream.Seek(0, SeekOrigin.Begin);
                return memoryStream;
            }
            catch (Exception)
            {
            }
            finally
            {
                foreach (var path in imageFilePathByGuid.Values)
                {
                    File.Delete(path);
                }
            }

            throw new NotImplementedException();
        }

        #endregion Methods
    }
}