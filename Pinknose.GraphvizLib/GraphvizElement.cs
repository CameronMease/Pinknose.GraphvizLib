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
using System.Text;

namespace Pinknose.GraphvizLib
{
    /// <summary>A node, graph or subgraph.</summary>
    public abstract class GraphvizElement : DotRenderer
    {
        #region Fields

        internal const int DefaultIndentAmount = 3;

        private string _name;

        #endregion Fields

        #region Constructors

        protected GraphvizElement(HtmlImageCache? imageCache = null) : base(imageCache)
        {
            _name = "A" + Guid.NewGuid().ToString("N");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// SVG class of the element (Graphviz <c>class</c>), for styling or finding it in SVG output.
        /// </summary>
        [AttributeName("class")]
        public string? Class { get; set; }

        [AttributeName("fontcolor")]
        public Color? FontColor { get; set; }

        [AttributeName("fontname")]
        public string? FontName { get; set; }

        [AttributeName("fontsize")]
        public double? FontSize { get; set; }

        [AttributeName("label")]
        public Label? Label { get; set; }

        /// <summary>
        /// The element's DOT identifier. Generated uniquely by default; set it to make the output
        /// predictable or readable. Any text is allowed — it is quoted when rendered.
        /// </summary>
        public virtual string Name
        {
            get => _name;
            set => _name = string.IsNullOrEmpty(value) ? throw new ArgumentException("A name cannot be empty.", nameof(value)) : value;
        }

        /// <summary>
        /// Id of the element's group in SVG output (Graphviz <c>id</c>). Not the same as
        /// <see cref="Name"/>, which identifies the element within the DOT source.
        /// </summary>
        [AttributeName("id")]
        public string? SvgId { get; set; }

        /// <summary>Browser window or frame a link opens in (Graphviz <c>target</c>).</summary>
        [AttributeName("target")]
        public string? Target { get; set; }

        [AttributeName("texlbl")]
        public Label? TexLabel { get; set; }

        /// <summary>Hover text in SVG output.</summary>
        [AttributeName("tooltip")]
        public string? Tooltip { get; set; }

        /// <summary>Link target in SVG output (Graphviz <c>URL</c>).</summary>
        [AttributeName("URL")]
        public string? Url { get; set; }

        /// <summary>The name as it appears in DOT source.</summary>
        internal string QuotedName => DotString.Quote(Name);

        #endregion Properties

        #region Methods

        /// <summary>Renders a graph or subgraph: header, attributes, children, edges.</summary>
        internal Dot RenderBlock(
            Graph graph,
            int indent,
            string header,
            string renderedName,
            Action<StringBuilder, int>? beforeChildren = null,
            Action<StringBuilder, int>? afterChildren = null)
        {
            var indentText = new string(' ', indent);
            var innerIndent = indent + DefaultIndentAmount;

            var sb = new StringBuilder();
            sb.Append(indentText).Append(header).Append(' ').Append(renderedName).AppendLine(" {");
            sb.Append(RenderMultiLineAttributes(innerIndent));

            beforeChildren?.Invoke(sb, innerIndent);

            foreach (var child in ((IParent)this).Children)
            {
                sb.Append(child.RenderDot(graph, innerIndent).DotSource);
            }

            afterChildren?.Invoke(sb, innerIndent);

            sb.Append(indentText).AppendLine("}");

            return new Dot(sb.ToString(), HtmlImageCache);
        }

        #endregion Methods
    }
}
