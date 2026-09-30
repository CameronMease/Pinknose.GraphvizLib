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
using System.Text;

namespace Pinknose.GraphvizLib
{
    public sealed class Node : GraphvizElement
    {
        #region Constructors

        public Node(HtmlImageCache? imageCache = null) : base(imageCache)
        {
        }

        #endregion Constructors

        #region Properties

        [AttributeName("color")]
        public Color? Color { get; set; }

        [AttributeName("fillcolor")]
        public Color? FillColor { get; set; }

        [AttributeName("fixedsize")]
        public bool? FixedSize { get; set; }

        /// <summary>Height in inches.</summary>
        [AttributeName("height")]
        public double? Height { get; set; }

        /// <summary>Width of the node's outline, in points.</summary>
        [AttributeName("penwidth")]
        public double? LineWidth { get; set; }

        /// <summary>Space around the label, in inches.</summary>
        [AttributeName("margin")]
        public double? Margin { get; set; }

        [AttributeName("shape")]
        public Shape? Shape { get; set; }

        [AttributeName("style")]
        public NodeStyle? Style { get; set; }

        /// <summary>Width in inches.</summary>
        [AttributeName("width")]
        public double? Width { get; set; }

        #endregion Properties

        #region Methods

        internal override Dot RenderDot(Graph graph, int indent)
        {
            var sb = new StringBuilder();
            sb.Append(' ', indent).Append(QuotedName).Append(RenderSingleLineAttributes()).AppendLine(";");

            return new Dot(sb.ToString(), HtmlImageCache);
        }

        #endregion Methods
    }
}
