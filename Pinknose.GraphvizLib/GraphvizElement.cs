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
using Pinknose.GraphvizLib.Html;
using Pinknose.Utilities;
using System;
using System.Linq;
using System.Text;

namespace Pinknose.GraphvizLib
{
    public abstract class GraphvizElement : DotRenderer
    {
        public GraphvizElement(HtmlImageCache? imageCache = null) : base(imageCache)
        {
        }

        #region Fields

        private static readonly int DefaultIndentAmount = 3;

        #endregion Fields

        #region Properties

        [AttributeName("fontsize")]
        public double? FontSize { get; set; } = null;

        /// <summary>
        /// Unique identifier for the element.  Can be set by the user, or an ID will be automatically generated.
        /// </summary>
        public Guid Guid { get; set; } = Guid.NewGuid();

        public virtual string Id => "A" + Guid.ToString().Replace("-", "");

        [AttributeName("label")]
        public Label? Label { get; set; } = null;

        [AttributeName("texlbl")]
        public Label? TexLabel { get; set; } = null;

        #endregion Properties

        #region Methods

        internal override Dot RenderDot(Graph graph, int indent)
        {
            string indentText = new(' ', indent);
            string nextIndentText = new(' ', indent + DefaultIndentAmount);

            var headerAttribute = (HeaderTextAttribute?)this.GetType().GetCustomAttributes(typeof(HeaderTextAttribute), true).FirstOrDefault();
            var isParent = headerAttribute != null;

            var sb = new StringBuilder();

            if (isParent)
            {
                sb.AppendLine($"{indentText}{headerAttribute?.Text} {this.Id} " + "{");

                sb.AppendLine(this.RenderMultiLineAttributes(indent + DefaultIndentAmount));

                var parentInterface = this.GetType().GetInterface(nameof(IParent));

                if (parentInterface != null)
                {
                    foreach (var child in ((IParent)this).Children)
                    {
                        sb.AppendLine(child.RenderDot(graph, indent + DefaultIndentAmount).DotSource);
                    }
                }
            }

            bool isAssignableToGraph = this.GetType().IsAssignableTo(typeof(Graph));

            if (isAssignableToGraph)
            {
                foreach (var edge in ((Graph)this).Edges)
                {
                    sb.Append(edge.RenderDot(graph, indent + DefaultIndentAmount));
                }
            }

            if (isParent)
            {
                sb.AppendLine(indentText + "}");
            }

            return new(sb.ToString(), this.HtmlImageCache);
        }

        #endregion Methods
    }
}