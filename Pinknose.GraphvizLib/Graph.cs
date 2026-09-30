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
using Pinknose.GraphvizLib.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pinknose.GraphvizLib
{
    /// <summary>A top-level graph. See <see cref="Digraph"/> and <see cref="UndirectedGraph"/>.</summary>
    public abstract class Graph : GraphvizElement, IParent
    {
        #region Constructors

        protected Graph(HtmlImageCache? imageCache = null) : base(imageCache)
        {
        }

        #endregion Constructors

        #region Properties

        [AttributeName("bgcolor")]
        public Color? BackgroundColor { get; set; }

        /// <summary>Determines if the graph will be centered in the canvas.</summary>
        [AttributeName("center")]
        public bool? Center { get; set; }

        public ElementCollection<GraphvizElement> Children { get; } = new();

        /// <summary>
        /// Default attributes for every edge (<c>edge [...]</c>), by Graphviz name. Values are
        /// literal text.
        /// </summary>
        public IDictionary<string, string> EdgeDefaults { get; } = new SortedDictionary<string, string>(StringComparer.Ordinal);

        /// <summary>
        /// Every edge in the graph, including edges between nodes inside subgraphs. Rendered after
        /// all nodes, in the order added.
        /// </summary>
        public ElementCollection<Edge> Edges { get; } = new();

        Graph IParent.Graph => this;

        /// <summary>The DOT keyword that opens this graph: <c>digraph</c> or <c>graph</c>.</summary>
        internal abstract string Keyword { get; }

        /// <summary>The DOT edge operator for this graph: <c>-&gt;</c> or <c>--</c>.</summary>
        internal abstract string EdgeOperator { get; }

        [AttributeName("labeljust")]
        public HorizontalJustification? LabelHorizontalJustification { get; set; }

        [AttributeName("labelloc")]
        public VerticalJustification? LabelVerticalJustification { get; set; }

        /// <summary>
        /// Default attributes for every node (<c>node [...]</c>), by Graphviz name. Values are
        /// literal text.
        /// </summary>
        public IDictionary<string, string> NodeDefaults { get; } = new SortedDictionary<string, string>(StringComparer.Ordinal);

        /// <summary>Minimum space between nodes in the same rank, in inches.</summary>
        [AttributeName("nodesep")]
        public double? NodeSeparation { get; set; }

        [AttributeName("rankdir")]
        public RankDirection? RankDirection { get; set; }

        /// <summary>Minimum space between ranks, in inches.</summary>
        [AttributeName("ranksep")]
        public double? RankSeparation { get; set; }

        [AttributeName("splines")]
        public SplineType? SplineType { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>Renders the whole graph as DOT source.</summary>
        public Dot RenderDot() => RenderDot(this, 0);

        internal override Dot RenderDot(Graph graph, int indent) =>
            RenderBlock(graph, indent, Keyword, QuotedName, (sb, innerIndent) =>
            {
                AppendDefaults(sb, innerIndent, "node", NodeDefaults);
                AppendDefaults(sb, innerIndent, "edge", EdgeDefaults);
            },
            (sb, innerIndent) =>
            {
                foreach (var edge in Edges)
                {
                    sb.Append(edge.RenderDot(graph, innerIndent).DotSource);
                }
            });

        private static void AppendDefaults(StringBuilder sb, int indent, string keyword, IDictionary<string, string> defaults)
        {
            if (defaults.Count == 0)
            {
                return;
            }

            var parts = new List<string>();
            foreach (var entry in defaults)
            {
                parts.Add($"{entry.Key}={DotString.Quote(entry.Value)}");
            }

            sb.Append(' ', indent).Append(keyword).Append(" [").Append(string.Join(", ", parts)).AppendLine("];");
        }

        #endregion Methods
    }
}
