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

using Pinknose.GraphvizLib.Atttributes;
using SkiaSharp;
using Svg;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Pinknose.GraphvizLib
{
    public static class GraphvizExtensions
    {
        #region Methods

        public static byte[] GetBytes(this SKBitmap png)
        {
            return png.Encode(SKEncodedImageFormat.Png, 100000).ToArray();
        }

        public static byte[] GetBytes(this SvgDocument svg)
        {
            using MemoryStream stream = new();
            using XmlWriter writer = XmlWriter.Create(stream, new XmlWriterSettings() { OmitXmlDeclaration = true });

            svg.Write(writer);
            writer.Flush();

            stream.Seek(0, SeekOrigin.Begin);

            byte[] buffer = stream.ToArray();

            string duhh = Encoding.UTF8.GetString(buffer);

            return buffer;
        }

        /// <summary>
        /// Gets the display value for a given enumeration value.
        /// </summary>
        /// <param name="theEnum"></param>
        /// <returns></returns>
        public static string GetDisplayValue(this Enum theEnum)
        {
            var enumType = theEnum.GetType();
            var memberInfos = enumType.GetMember(theEnum.ToString());
            var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);

            DisplayValueAttribute? valueAttribute = null;

            if (enumValueMemberInfo is not null)
            {
                valueAttribute = (DisplayValueAttribute?)enumValueMemberInfo.GetCustomAttributes(typeof(DisplayValueAttribute), false).FirstOrDefault();
            }

            return valueAttribute?.DisplayValue ?? theEnum.ToString();
        }

        internal static string ConvertToString(this Stream stream)
        {
            using MemoryStream memoryStream = new();
            stream.CopyTo(memoryStream);

            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        #endregion Methods
    }
}