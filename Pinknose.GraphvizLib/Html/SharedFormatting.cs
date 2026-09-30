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

using System;
using System.Linq;
using System.Text;

namespace Pinknose.GraphvizLib.Html
{
    internal static class SharedFormatting
    {
        #region Fields

        private static readonly HtmlTextFormat[] Formats =
            Enum.GetValues(typeof(HtmlTextFormat))
                .Cast<HtmlTextFormat>()
                .Where(f => f != HtmlTextFormat.None)
                .ToArray();

        #endregion Fields

        #region Methods

        /// <summary>Escapes text for an HTML-like label, as element content or an attribute value.</summary>
        public static string EscapeHtml(string text) => text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&apos;");

        /// <summary>
        /// An image by its cache id. The placeholder is replaced with a file path when a renderer
        /// writes the image to disk; see <see cref="HtmlImageCache.PlaceholderFor"/>.
        /// </summary>
        public static string FormatImage(Guid guid) => $"<IMG SRC=\"{HtmlImageCache.PlaceholderFor(guid)}\"/>";

        public static string FormatLineBreak() => "<BR/>";

        public static string FormatText(string text, HtmlTextFormat format = HtmlTextFormat.None)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            foreach (var option in Formats.Where(f => format.HasFlag(f)))
            {
                sb.Append('<').Append(option.GetDisplayValue()).Append('>');
            }

            sb.Append(EscapeHtml(text));

            // Closed in reverse, so the tags nest: <B><I>x</I></B>, not <B><I>x</B></I>.
            foreach (var option in Formats.Where(f => format.HasFlag(f)).Reverse())
            {
                sb.Append("</").Append(option.GetDisplayValue()).Append('>');
            }

            return sb.ToString();
        }

        #endregion Methods
    }
}
