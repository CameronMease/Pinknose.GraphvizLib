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

using Pinknose.GraphvizLib.Attributes;
using System.Collections.Generic;

namespace Pinknose.GraphvizLib
{
    [HeaderText("subgraph")]
    public sealed class Subgraph : GraphvizElement, IParent
    {
        #region Constructors

        public Subgraph(Graph graph)
        {
            Graph = graph;
        }

        #endregion Constructors

        #region Properties

        public HashSet<GraphvizElement> Children { get; } = new();

        [AttributeName("color")]
        public Color? Color { get; set; } = null;

        [AttributeName("fontcolor")]
        public Color? FontColor { get; set; } = null;

        //[AttributeName("fontsize")]
        //public double? FontSize { get; set; } = null;

        public Graph Graph { get; }
        public override string Id => (IsCluster ? "cluster_" : "") + base.Id;
        public bool IsCluster { get; set; } = true;

        [AttributeName("labeljust")]
        public HorizontalJustification? LabelJustification { get; set; } = null;

        [AttributeName("penwidth")]
        public double? LineWidth { get; set; } = null;

        [AttributeName("style")]
        public ClusterStyle? Style { get; set; } = null;

        #endregion Properties
    }
}