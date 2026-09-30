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

using SkiaSharp;
using Svg;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Pinknose.GraphvizLib
{
    /// <summary>Rendering a graph by running an installed Graphviz. See <see cref="DotExe"/>.</summary>
    public static class GraphRenderingExtensions
    {
        #region Methods

        public static byte[] GetBytes(this SKBitmap png)
        {
            if (png is null)
            {
                throw new ArgumentNullException(nameof(png));
            }

            using var data = png.Encode(SKEncodedImageFormat.Png, 100);
            return data.ToArray();
        }

        public static byte[] GetBytes(this SvgDocument svg)
        {
            if (svg is null)
            {
                throw new ArgumentNullException(nameof(svg));
            }

            using var stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { OmitXmlDeclaration = true, Encoding = new UTF8Encoding(false) }))
            {
                svg.Write(writer);
            }

            return stream.ToArray();
        }

        /// <summary>Renders PNG and SVG at once, running Graphviz for each in parallel.</summary>
        public static async Task<(SKBitmap Png, SvgDocument Svg)> RenderGraphAsync(
            this Graph graph,
            GraphvizEngine engine = GraphvizEngine.Dot,
            CancellationToken cancellationToken = default)
        {
            var png = graph.RenderPngAsync(engine, cancellationToken);
            var svg = graph.RenderSvgAsync(engine, cancellationToken);

            await Task.WhenAll(png, svg).ConfigureAwait(false);

            return (await png.ConfigureAwait(false), await svg.ConfigureAwait(false));
        }

        public static async Task<SKBitmap> RenderPngAsync(
            this Graph graph,
            GraphvizEngine engine = GraphvizEngine.Dot,
            CancellationToken cancellationToken = default)
        {
            var bytes = await DotExe.RenderAsync(graph, "png", engine, cancellationToken).ConfigureAwait(false);

            return SKBitmap.Decode(bytes)
                ?? throw new InvalidOperationException("Graphviz produced PNG output that could not be decoded.");
        }

        public static async Task<SvgDocument> RenderSvgAsync(
            this Graph graph,
            GraphvizEngine engine = GraphvizEngine.Dot,
            CancellationToken cancellationToken = default)
        {
            var bytes = await DotExe.RenderAsync(graph, "svg", engine, cancellationToken).ConfigureAwait(false);

            using var stream = new MemoryStream(bytes);
            return SvgDocument.Open<SvgDocument>(stream);
        }

        /// <summary>Renders SVG as text, untouched by any SVG library.</summary>
        public static async Task<string> RenderSvgStringAsync(
            this Graph graph,
            GraphvizEngine engine = GraphvizEngine.Dot,
            CancellationToken cancellationToken = default)
        {
            var bytes = await DotExe.RenderAsync(graph, "svg", engine, cancellationToken).ConfigureAwait(false);
            return Encoding.UTF8.GetString(bytes);
        }

        public static Task RenderToPngFileAsync(
            this Graph graph,
            string filename,
            GraphvizEngine engine = GraphvizEngine.Dot,
            CancellationToken cancellationToken = default) =>
            RenderToFileAsync(graph, "png", filename, engine, cancellationToken);

        public static Task RenderToSvgFileAsync(
            this Graph graph,
            string filename,
            GraphvizEngine engine = GraphvizEngine.Dot,
            CancellationToken cancellationToken = default) =>
            RenderToFileAsync(graph, "svg", filename, engine, cancellationToken);

        private static async Task RenderToFileAsync(
            Graph graph,
            string format,
            string filename,
            GraphvizEngine engine,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("A file name is required.", nameof(filename));
            }

            var bytes = await DotExe.RenderAsync(graph, format, engine, cancellationToken).ConfigureAwait(false);

            using var file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
            await file.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);
        }

        #endregion Methods
    }
}
