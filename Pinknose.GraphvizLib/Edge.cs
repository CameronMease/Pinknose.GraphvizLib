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

using Pinknose.GraphvizLib.Attributes;
using System;
using System.Text;

namespace Pinknose.GraphvizLib
{
    public sealed class Edge : DotRenderer
    {
        #region Constructors

        public Edge(GraphvizElement source, GraphvizElement destination) : base(null)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
        }

        #endregion Constructors

        #region Properties

        [AttributeName("arrowhead")]
        public ArrowType? ArrowHead { get; set; }

        [AttributeName("arrowtail")]
        public ArrowType? ArrowTail { get; set; }

        /// <summary>SVG class of the edge (Graphviz <c>class</c>).</summary>
        [AttributeName("class")]
        public string? Class { get; set; }

        [AttributeName("color")]
        public Color? Color { get; set; }

        [AttributeName("constraint")]
        public bool? Constraint { get; set; }

        public GraphvizElement Destination { get; }

        /// <summary>
        /// Which ends get arrowheads. In an undirected graph Graphviz draws none unless this is set.
        /// </summary>
        [AttributeName("dir")]
        public EdgeDirection? Direction { get; set; }

        /// <summary>
        /// The port on <see cref="Destination"/> the edge ends at: a <c>PORT</c> in its HTML label.
        /// </summary>
        public string? EndPortName { get; set; }

        [AttributeName("fontcolor")]
        public Color? FontColor { get; set; }

        [AttributeName("fontname")]
        public string? FontName { get; set; }

        [AttributeName("fontsize")]
        public double? FontSize { get; set; }

        /// <summary>The compass point on the destination the edge ends at.</summary>
        [AttributeName("headport")]
        public PortPosition? HeadPort { get; set; }

        [AttributeName("label")]
        public Label? Label { get; set; }

        [AttributeName("penwidth")]
        public double? LineWidth { get; set; }

        public GraphvizElement Source { get; }

        /// <summary>
        /// The port on <see cref="Source"/> the edge starts from: a <c>PORT</c> in its HTML label.
        /// </summary>
        public string? StartPortName { get; set; }

        [AttributeName("style")]
        public EdgeStyle? Style { get; set; }

        /// <summary>Id of the edge's group in SVG output (Graphviz <c>id</c>).</summary>
        [AttributeName("id")]
        public string? SvgId { get; set; }

        /// <summary>The compass point on the source the edge starts from.</summary>
        [AttributeName("tailport")]
        public PortPosition? TailPort { get; set; }

        [AttributeName("target")]
        public string? Target { get; set; }

        /// <summary>Hover text in SVG output.</summary>
        [AttributeName("tooltip")]
        public string? Tooltip { get; set; }

        /// <summary>Link target in SVG output (Graphviz <c>URL</c>).</summary>
        [AttributeName("URL")]
        public string? Url { get; set; }

        #endregion Properties

        #region Methods

        internal override Dot RenderDot(Graph graph, int indent)
        {
            var sb = new StringBuilder();
            sb.Append(' ', indent)
                .Append(Endpoint(Source, StartPortName))
                .Append(' ').Append(graph.EdgeOperator).Append(' ')
                .Append(Endpoint(Destination, EndPortName))
                .Append(RenderSingleLineAttributes())
                .AppendLine(";");

            return new Dot(sb.ToString(), HtmlImageCache);
        }

        private static string Endpoint(GraphvizElement element, string? port) =>
            port is null ? element.QuotedName : $"{element.QuotedName}:{DotString.Quote(port)}";

        #endregion Methods
    }
}
