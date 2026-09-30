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
using Pinknose.GraphvizLib.Html.Attributes;
using System;
using System.Text;

namespace Pinknose.GraphvizLib.Html
{
    [Flags]
    public enum HtmlTextFormat
    {
        None = 0,

        [DisplayValue("I")]
        Italics = 1 << 0,

        [DisplayValue("B")]
        Bold = 1 << 1,

        [DisplayValue("U")]
        Underline = 1 << 2,

        [DisplayValue("O")]
        Overline = 1 << 3,

        [DisplayValue("SUB")]
        Subscript = 1 << 4,

        [DisplayValue("SUP")]
        Superscript = 1 << 5,

        [DisplayValue("S")]
        Strikeout = 1 << 6
    }

    /// <summary>
    /// Builds an HTML-like label. Assign it to a <see cref="Label"/>; see
    /// https://graphviz.org/doc/info/shapes.html#html for what Graphviz accepts.
    /// </summary>
    public class HtmlBuilder : HtmlBuilderBase
    {
        #region Methods

        public HtmlBuilder AppendLineBreak()
        {
            AppendRawString(SharedFormatting.FormatLineBreak());
            return this;
        }

        public HtmlBuilder AppendText(string text, HtmlTextFormat format = HtmlTextFormat.None)
        {
            AppendRawString(SharedFormatting.FormatText(text, format));
            return this;
        }

        public FontBuilder<HtmlBuilder> StartFont(params IFontAttribute[] attributes) => new(this, attributes);

        public TableBuilder<HtmlBuilder> StartTable(params ITableAttribute[] attributes) => new(this, attributes);

        /// <summary>The label, wrapped in the angle brackets that mark it as HTML to Graphviz.</summary>
        public override string ToString() => "<" + base.ToString() + ">";

        #endregion Methods
    }

    public abstract class HtmlBuilderBase
    {
        #region Properties

        protected StringBuilder StringBuilder { get; } = new StringBuilder();

        #endregion Properties

        #region Methods

        public override string ToString() => StringBuilder.ToString();

        internal void AppendRawString(string text) => StringBuilder.Append(text);

        /// <summary>Opens a tag with its attributes: <c>&lt;TD PORT="x"&gt;</c>.</summary>
        internal static string OpenTag(string tag, IHtmlAttribute[] attributes)
        {
            var sb = new StringBuilder("<").Append(tag);

            foreach (var attribute in attributes ?? Array.Empty<IHtmlAttribute>())
            {
                sb.Append(' ').Append(attribute);
            }

            return sb.Append('>').ToString();
        }

        #endregion Methods
    }
}
