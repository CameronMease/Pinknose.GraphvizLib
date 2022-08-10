/////////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright(c) 2022 Cameron Mease
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
using System.Diagnostics;
using System.Reflection;

namespace Pinknose.GraphvizLib
{
    public static class Dot
    {
        #region Fields

        private static readonly string DotExecutableName = "fdp.exe";

        private static Lazy<string> DotBinPath = new Lazy<string>(() =>
            {
                string path = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetAssembly(typeof(Dot)).Location),
                    "Graphviz",
                    "bin");

                return path;
            });

        #endregion Fields

        #region Methods

        public static async Task<SKBitmap> RenderPngAsync(Graph graph)
        {
            using var stream = await RenderAsync(graph, "png").ConfigureAwait(false);

            stream.Seek(0, SeekOrigin.Begin);

            return SKBitmap.Decode(stream);
        }

        public static async Task<SvgDocument> RenderSvgAsync(Graph graph)
        {
            using var stream = await RenderAsync(graph, "svg").ConfigureAwait(false);
            stream.Seek(0, SeekOrigin.Begin);

            var svg = SvgDocument.Open<SvgDocument>(stream);

            return svg;
        }

        private static async Task<Stream> RenderAsync(Graph graph, string type)
        {
            string dot = graph.RenderDot();

            var proc = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = false,
                    WorkingDirectory = DotBinPath?.Value,
                    FileName = Path.Combine(DotBinPath?.Value, DotExecutableName),
                    Arguments = $"-T{type}",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                }
            };

            proc.Start();

            proc.StandardInput.WriteLine(dot);
            proc.StandardInput.Flush();
            proc.StandardInput.Close();

            MemoryStream memoryStream = new MemoryStream();

            byte[] buffer = new byte[1024];
            int bytesRead = 0;

            do
            {
                bytesRead = await proc.StandardOutput.BaseStream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                memoryStream.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);

            proc.WaitForExit();

            return memoryStream;
        }

        #endregion Methods
    }
}