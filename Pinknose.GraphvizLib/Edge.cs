﻿/////////////////////////////////////////////////////////////////////////////////
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

using Pinknose.GraphvizLib.Attributes;
using Pinknose.GraphvizLib.Html;
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
        public ArrowType? ArrowHead { get; set; } = null;

        [AttributeName("arrowtail")]
        public ArrowType? ArrowTail { get; set; } = null;

        [AttributeName("color")]
        public Color? Color { get; set; } = null;

        public GraphvizElement Destination { get; }

        public string? EndPortName { get; set; } = null;

        [AttributeName("fontcolor")]
        public Color? FontColor { get; set; } = null;

        [AttributeName("fontsize")]
        public double? FontSize { get; set; } = null;

        [AttributeName("headport")]
        public PortPosition? HeadPort { get; set; } = null;

        [AttributeName("label")]
        public Label? Label { get; set; } = null;

        [AttributeName("penwidth")]
        public double? LineWidth { get; set; } = null;

        public GraphvizElement Source { get; }

        public string? StartPortName { get; set; } = null;

        [AttributeName("style")]
        public EdgeStyle? Style { get; set; } = null;

        [AttributeName("tailport")]
        public PortPosition? TailPort { get; set; } = null;

        #endregion Properties

        #region Methods

        internal override Dot RenderDot(Graph graph, int indent)
        {
            string connectorSyntax = graph.GetType() == typeof(Digraph) ? "->" : "--";

            string indentText = new string(' ', indent);

            var startSourceId = StartPortName is null ? Source.Id : $"{Source.Id}:{StartPortName}";
            var endSourceId = EndPortName is null ? Destination.Id : $"{Destination.Id}:{EndPortName}";

            var sb = new StringBuilder();

            sb.AppendLine($"{indentText}{startSourceId}{connectorSyntax}{endSourceId} {this.RenderSingleLineAttributes()};");

            return new (sb.ToString(), this.HtmlImageCache);
        }

        #endregion Methods
    }
}