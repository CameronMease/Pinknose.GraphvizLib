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
using System.Globalization;
using System.Text;

namespace Pinknose.GraphvizLib
{
    /// <summary>
    /// Formats values as DOT quoted strings.
    /// </summary>
    /// <remarks>
    /// Inside a DOT quoted string the parser treats <c>\"</c> as a quote and keeps every other
    /// backslash; attributes of type escString (label, tooltip, URL, id, ...) then give sequences
    /// such as <c>\n</c> and <c>\l</c> their meaning. Text passed through <see cref="Quote"/> is
    /// therefore made literal: backslashes are doubled, quotes escaped, and real line breaks turned
    /// into <c>\n</c>. Without this a label containing a quote ends the string early and the
    /// whole graph fails to parse.
    /// </remarks>
    public static class DotString
    {
        #region Methods

        /// <summary>Quotes literal text, so that every character appears as written.</summary>
        public static string Quote(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var sb = new StringBuilder(text.Length + 2);
            sb.Append('"');

            foreach (var c in text)
            {
                switch (c)
                {
                    case '\\':
                        sb.Append("\\\\");
                        break;

                    case '"':
                        sb.Append("\\\"");
                        break;

                    case '\n':
                        sb.Append("\\n");
                        break;

                    case '\r':
                        // Dropped: \r\n becomes one line break, not two.
                        break;

                    default:
                        sb.Append(c);
                        break;
                }
            }

            sb.Append('"');
            return sb.ToString();
        }

        /// <summary>
        /// Quotes a Graphviz escString as written, keeping its escape sequences (<c>\n</c>,
        /// <c>\l</c>, <c>\N</c>, ...). Only quotes are escaped.
        /// </summary>
        public static string QuoteEscString(string escString)
        {
            if (escString is null)
            {
                throw new ArgumentNullException(nameof(escString));
            }

            var sb = new StringBuilder(escString.Length + 2);
            sb.Append('"');

            for (var i = 0; i < escString.Length; i++)
            {
                var c = escString[i];

                if (c == '\\' && i + 1 < escString.Length)
                {
                    // Keep an escape pair intact, so an already-escaped quote is not escaped twice.
                    sb.Append(c).Append(escString[i + 1]);
                    i++;
                }
                else if (c == '\\')
                {
                    // A lone trailing backslash would escape the closing quote.
                    sb.Append("\\\\");
                }
                else if (c == '"')
                {
                    sb.Append("\\\"");
                }
                else
                {
                    sb.Append(c);
                }
            }

            sb.Append('"');
            return sb.ToString();
        }

        /// <summary>Formats a number the way DOT expects, whatever the current culture.</summary>
        public static string FormatNumber(double value) => value.ToString("R", CultureInfo.InvariantCulture);

        #endregion Methods
    }
}
