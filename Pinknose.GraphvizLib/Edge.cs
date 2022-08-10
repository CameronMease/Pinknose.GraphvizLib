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

using Pinknose.GraphvizLib.Attributes;
using System.Text;

namespace Pinknose.GraphvizLib
{
    public class Edge : DotRenderer
    {
        #region Constructors

        public Edge(GraphvizElement source, GraphvizElement destination)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
        }

        #endregion Constructors

        #region Properties

        [AttributeName("arrowhead")]
        public ArrowType? ArrowHead { get; set; } = null;

        [AttributeName("arrowtail")]
        public ArrowType? ArrowTail { get; set; } = null;

        [AttributeName("color")]
        public Color? Color { get; set; } = null;

        public GraphvizElement Destination { get; }

        [AttributeName("fontcolor")]
        public Color? FontColor { get; set; } = null;

        [AttributeName("fontsize")]
        public double? FontSize { get; set; } = null;

        [AttributeName("label")]
        public string? Label { get; set; } = null;

        [AttributeName("penwidth")]
        public double? LineWidth { get; set; } = null;

        public GraphvizElement Source { get; }

        [AttributeName("style")]
        public EdgeStyle? Style { get; set; } = null;

        #endregion Properties

        #region Methods

        internal override string RenderDot(Graph graph, int indent)
        {
            string connectorSyntax = graph.GetType() == typeof(Digraph) ? "->" : "--";

            string indentText = new string(' ', indent);

            var sb = new StringBuilder();

            sb.AppendLine($"{indentText}{Source.Id}{connectorSyntax}{Destination.Id} {this.RenderSingleLineAttributes()};");

            return sb.ToString();
        }

        #endregion Methods
    }
}