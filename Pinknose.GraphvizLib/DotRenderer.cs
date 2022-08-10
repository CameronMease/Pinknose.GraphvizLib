﻿/////////////////////////////////////////////////////////////////////////////////
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
    public abstract class DotRenderer
    {
        #region Methods

        internal abstract string RenderDot(Graph graph, int indent);

        internal string RenderMultiLineAttributes(int indent)
        {
            string indentText = new string(' ', indent);

            var sb = new StringBuilder();

            var attributes = GetAttributes();

            foreach (var attr in attributes)
            {
                sb.AppendLine($"{indentText}{attr.Key}=\"{attr.Value}\";");
            }

            return sb.ToString();
        }

        internal string RenderSingleLineAttributes()
        {
            var sb = new StringBuilder();
            sb.Append("[");

            var attributes = GetAttributes();

            foreach (var attr in attributes)
            {
                sb.Append($"{attr.Key}=\"{attr.Value}\",");
            }

            sb.Append("]");

            return sb.ToString();
        }

        private IDictionary<string, string> GetAttributes()
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();

            var propertyInfos = this.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(AttributeNameAttribute), true).Any());

            foreach (var propertyInfo in propertyInfos)
            {
                var attributeName = ((AttributeNameAttribute)propertyInfo.GetCustomAttributes(typeof(AttributeNameAttribute), true).Single()).Name;
                string attrbuteValue;

                var value = propertyInfo.GetValue(this);

                if (value != null)
                {
                    var underlyingType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);

                    if (underlyingType != null && underlyingType.IsEnum)
                    {
                        if (underlyingType == typeof(Color))
                        {
                            attrbuteValue = value.ToString().ToLower();
                        }
                        else
                        {
                            attrbuteValue = ((Enum)value).GetDisplayValue();
                        }
                    }
                    else if (underlyingType == typeof(bool))
                    {
                        attrbuteValue = value.ToString().ToLower();
                    }
                    else
                    {
                        attrbuteValue = value.ToString();
                    }

                    if (!string.IsNullOrEmpty(attrbuteValue))
                    {
                        attributes.Add(attributeName, attrbuteValue);
                    }
                }
            }

            return attributes;
        }

        #endregion Methods
    }
}