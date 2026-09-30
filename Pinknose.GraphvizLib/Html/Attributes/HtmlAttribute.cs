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

namespace Pinknose.GraphvizLib.Html.Attributes
{
    public interface ICellAttribute : IHtmlAttribute
    {
    }

    public interface IFontAttribute : IHtmlAttribute
    {
    }

    public interface IHtmlAttribute
    {
        #region Properties

        string Name { get; }

        string Value { get; }

        #endregion Properties
    }

    public interface ITableAttribute : IHtmlAttribute
    {
    }

    public abstract class HtmlAttribute : IHtmlAttribute
    {
        #region Constructors

        protected HtmlAttribute(string value)
        {
            Value = value;
        }

        #endregion Constructors

        #region Properties

        public abstract string Name { get; }

        /// <summary>The value as given, before escaping.</summary>
        public string Value { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// The attribute as it appears in an HTML-like label. The value is escaped, because a
        /// tooltip or link containing a quote or an ampersand would otherwise end the attribute
        /// early and break the label.
        /// </summary>
        public override string ToString() => $"{Name}=\"{SharedFormatting.EscapeHtml(Value)}\"";

        #endregion Methods
    }
}
