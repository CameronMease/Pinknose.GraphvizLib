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
    public abstract class GraphvizElement : DotRenderer
    {
        #region Fields

        private static readonly int DefaultIndentAmount = 3;

        #endregion Fields

        #region Properties

        [AttributeName("fontsize")]
        public double? FontSize { get; set; } = null;

        public virtual string Id { get; } = "A" + Guid.NewGuid().ToString().Replace("-", "");

        [AttributeName("label")]
        public string? Label { get; set; } = null;

        #endregion Properties

        #region Methods

        internal override string RenderDot(Graph graph, int indent)
        {
            string indentText = new string(' ', indent);
            string nextIndentText = new string(' ', indent + DefaultIndentAmount);

            var headerAttribute = (HeaderTextAttribute)this.GetType().GetCustomAttributes(typeof(HeaderTextAttribute), true).FirstOrDefault();
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
                        sb.AppendLine(child.RenderDot(graph, indent + DefaultIndentAmount));
                    }
                }
            }

            if (this.GetType().IsAssignableTo(typeof(Graph)))
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

            return sb.ToString();
        }

        #endregion Methods
    }
}