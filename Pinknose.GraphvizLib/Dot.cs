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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Pinknose.GraphvizLib
{
    public enum GraphvizEngine
    { Dot, Fdp }

    public static class Dot
    {
        #region Fields

        private static readonly Lazy<string> DotBinPath = new(() =>
            {
                var assemblyLocation = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Dot))?.Location) ?? throw new ArgumentNullException();

                string path = Path.Combine(
                    assemblyLocation,
                    "Graphviz",
                    "bin");

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
            var dotExecutableName = engine switch
            {
                GraphvizEngine.Fdp => "fdp.exe",
                GraphvizEngine.Dot => "dot.exe",
                _ => throw new NotImplementedException()
            };

            bool errorRunningProcess = false;

            string dot = graph.RenderDot();

            var proc = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = false,
                    WorkingDirectory = DotBinPath.Value,
                    FileName = Path.Combine(DotBinPath.Value, dotExecutableName),
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

                proc.StandardInput.WriteLine(dot);
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

                return memoryStream;
            }
            catch (Exception)
            {
            }

            throw new NotImplementedException();
        }

        #endregion Methods
    }
}