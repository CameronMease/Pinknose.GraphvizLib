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

using Pinknose.GraphvizLib.Html;
using System;

namespace Pinknose.GraphvizLib
{
    /// <summary>
    /// A label: literal text, a Graphviz escString, or an HTML-like label.
    /// </summary>
    /// <remarks>
    /// A plain string converts to <em>literal</em> text, so whatever it contains — quotes,
    /// backslashes, line breaks — appears as written. Use <see cref="FromEscString"/> for text that
    /// relies on Graphviz escapes such as <c>\l</c> (left-justified line) or <c>\N</c> (node name).
    /// </remarks>
    public sealed class Label
    {
        #region Fields

        private readonly LabelKind _kind;
        private readonly string _text;

        #endregion Fields

        #region Constructors

        private Label(string text, LabelKind kind)
        {
            _text = text ?? throw new ArgumentNullException(nameof(text));
            _kind = kind;
        }

        #endregion Constructors

        #region Enums

        private enum LabelKind
        {
            Literal,
            EscString,
            Html
        }

        #endregion Enums

        #region Properties

        public bool IsHtml => _kind == LabelKind.Html;

        #endregion Properties

        #region Methods

        public static Label FromEscString(string escString) => new(escString, LabelKind.EscString);

        public static Label FromHtml(HtmlBuilder builder) =>
            new((builder ?? throw new ArgumentNullException(nameof(builder))).ToString(), LabelKind.Html);

        public static Label FromText(string text) => new(text, LabelKind.Literal);

        public static implicit operator Label(string text) => FromText(text);

        public static implicit operator Label(HtmlBuilder builder) => FromHtml(builder);

        public override string ToString() => _text;

        /// <summary>The label as a DOT attribute value.</summary>
        internal string ToDot() => _kind switch
        {
            // Already wrapped in <...> by HtmlBuilder; it must not be quoted.
            LabelKind.Html => _text,
            LabelKind.EscString => DotString.QuoteEscString(_text),
            _ => DotString.Quote(_text),
        };

        #endregion Methods
    }
}
