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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Pinknose.GraphvizLib
{
    /// <summary>Anything that renders to DOT with a list of attributes.</summary>
    /// <remarks>
    /// Equality is reference equality, deliberately. An earlier version compared elements by the
    /// sum of their property hashes, so two distinct nodes with the same attributes were "equal"
    /// and the second silently vanished from a graph's children.
    /// </remarks>
    public abstract class DotRenderer
    {
        #region Constructors

        protected DotRenderer(HtmlImageCache? htmlImageCache)
        {
            HtmlImageCache = htmlImageCache;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Attributes this library has no property for, by their Graphviz name. Values are literal
        /// text and are quoted and escaped when rendered. An entry here replaces a property that
        /// renders the same attribute.
        /// </summary>
        public IDictionary<string, string> Attributes { get; } = new SortedDictionary<string, string>(StringComparer.Ordinal);

        protected HtmlImageCache? HtmlImageCache { get; }

        #endregion Properties

        #region Methods

        internal abstract Dot RenderDot(Graph graph, int indent);

        internal string RenderMultiLineAttributes(int indent)
        {
            var indentText = new string(' ', indent);
            var sb = new StringBuilder();

            foreach (var attr in GetAttributes())
            {
                sb.Append(indentText).Append(attr.Key).Append('=').Append(attr.Value).AppendLine(";");
            }

            return sb.ToString();
        }

        /// <summary>The attributes as <c> [a="1", b="2"]</c>, or an empty string if there are none.</summary>
        internal string RenderSingleLineAttributes()
        {
            var attributes = GetAttributes();

            if (attributes.Count == 0)
            {
                return string.Empty;
            }

            return " [" + string.Join(", ", attributes.Select(a => $"{a.Key}={a.Value}")) + "]";
        }

        /// <summary>
        /// Every set attribute, rendered and sorted by name, so the same element always produces
        /// the same text.
        /// </summary>
        internal IReadOnlyList<KeyValuePair<string, string>> GetAttributes()
        {
            var attributes = new SortedDictionary<string, string>(StringComparer.Ordinal);

            foreach (var property in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var nameAttribute = property.GetCustomAttribute<AttributeNameAttribute>(true);
                if (nameAttribute is null)
                {
                    continue;
                }

                var value = property.GetValue(this);
                if (value is not null)
                {
                    attributes[nameAttribute.Name] = FormatValue(value);
                }
            }

            foreach (var extra in Attributes)
            {
                attributes[extra.Key] = DotString.Quote(extra.Value);
            }

            return attributes.ToList();
        }

        internal static string FormatValue(object value) => value switch
        {
            Label label => label.ToDot(),
            Color color => DotString.Quote(color.ToString().ToLowerInvariant()),
            Enum e => DotString.Quote(e.GetDisplayValue()),
            bool b => b ? "true" : "false",
            double d => DotString.FormatNumber(d),
            float f => DotString.FormatNumber(f),
            int i => i.ToString(System.Globalization.CultureInfo.InvariantCulture),
            string s => DotString.Quote(s),
            _ => DotString.Quote(Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty),
        };

        #endregion Methods
    }
}
