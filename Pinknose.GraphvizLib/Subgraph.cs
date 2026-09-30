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

namespace Pinknose.GraphvizLib
{
    /// <summary>
    /// A group of nodes within a graph. A cluster (the default) is drawn as a box around its
    /// nodes; a plain subgraph only groups them.
    /// </summary>
    public sealed class Subgraph : GraphvizElement, IParent
    {
        #region Constructors

        public Subgraph(Graph graph, HtmlImageCache? imageCache = null) : base(imageCache)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
        }

        #endregion Constructors

        #region Properties

        public ElementCollection<GraphvizElement> Children { get; } = new();

        [AttributeName("color")]
        public Color? Color { get; set; }

        [AttributeName("fillcolor")]
        public Color? FillColor { get; set; }

        public Graph Graph { get; }

        /// <summary>
        /// Whether this is a cluster. Graphviz recognises a cluster by its name, so the DOT name
        /// gains a <c>cluster_</c> prefix.
        /// </summary>
        public bool IsCluster { get; set; } = true;

        [AttributeName("labeljust")]
        public HorizontalJustification? LabelJustification { get; set; }

        [AttributeName("penwidth")]
        public double? LineWidth { get; set; }

        [AttributeName("style")]
        public ClusterStyle? Style { get; set; }

        #endregion Properties

        #region Methods

        internal override Dot RenderDot(Graph graph, int indent) =>
            // The prefix lives in the rendered name only, so Name stays what the caller set.
            RenderBlock(graph, indent, "subgraph", IsCluster ? DotString.Quote("cluster_" + Name) : QuotedName);

        #endregion Methods
    }
}
