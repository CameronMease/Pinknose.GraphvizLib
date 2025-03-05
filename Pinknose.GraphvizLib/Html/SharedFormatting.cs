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

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Pinknose.GraphvizLib.Html
{
    internal static class SharedFormatting
    {
        #region Methods

        public static string FormatImage(Guid guid) => $"<IMG SRC=\"<GUID>{guid}</GUID>\"/>";

        public static string FormatLatex(string latex, HtmlImageCache imageCache) => FormatLatexAsync(latex, imageCache).GetAwaiter().GetResult();

        public static async Task<string> FormatLatexAsync(string latex, HtmlImageCache imageCache)
        {
            //var filename = Path.ChangeExtension(Path.GetTempFileName(), "png");

            byte[] bytes = await LatexMathRenderer.RenderAsync(latex);
            Guid guid = imageCache.Add(new ImageDescription("png", bytes));

            return FormatImage(guid);
        }

        public static string FormatLineBreak() => "<BR/>";

        public static string FormatText(string text, HtmlTextFormat format = HtmlTextFormat.None)
        {
            StringBuilder sb = new();

            if (!string.IsNullOrEmpty(text))
            {
                foreach (var option in Enum.GetValues(typeof(HtmlTextFormat)))
                {
                    if (option is null)
                    {
                        continue;
                    }

                    if (((int)format & (int)option) != 0)
                    {
                        sb.AppendFormat("<{0}>", ((Enum)option).GetDisplayValue());
                    }
                }

                var tempText = text
                    .Replace("&", "&amp;")
                    .Replace("<", "&lt;")
                    .Replace(">", "&gt;")
                    .Replace("\"", "&quot;")
                    .Replace("'", "&apos;");

                sb.Append(tempText);

                foreach (var option in Enum.GetValues(typeof(HtmlTextFormat)))
                {
                    if (option is null)
                    {
                        continue;
                    }

                    if (((int)format & (int)option) != 0)
                    {
                        sb.AppendFormat("</{0}>", ((Enum)option).GetDisplayValue());
                    }
                }
            }

            return sb.ToString();
        }

        #endregion Methods
    }
}