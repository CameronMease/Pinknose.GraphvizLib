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

using CSharpMath.SkiaSharp;
using Pinknose.GraphvizLib.Html;
using SkiaSharp;
using System;
using System.IO;

namespace Pinknose.GraphvizLib
{
    /// <summary>LaTeX math, rendered to an image and placed in an HTML-like label.</summary>
    public static class LatexMathRenderer
    {
        #region Methods

        /// <summary>Renders LaTeX math and appends it to a table cell as an image.</summary>
        public static TableCellBuilder<TParent> AppendLatexImage<TParent>(
            this TableCellBuilder<TParent> cell,
            string latex,
            HtmlImageCache imageCache)
            where TParent : HtmlBuilderBase
        {
            if (cell is null)
            {
                throw new ArgumentNullException(nameof(cell));
            }

            if (imageCache is null)
            {
                throw new ArgumentNullException(nameof(imageCache));
            }

            var guid = imageCache.Add(new ImageDescription("png", Render(latex)));
            return cell.AppendImage(guid);
        }

        /// <summary>Renders LaTeX math to PNG bytes.</summary>
        /// <exception cref="ArgumentException">The LaTeX could not be rendered.</exception>
        public static byte[] Render(string latex)
        {
            if (string.IsNullOrWhiteSpace(latex))
            {
                throw new ArgumentException("LaTeX is required.", nameof(latex));
            }

            var painter = new MathPainter { LaTeX = latex };

            using var image = painter.DrawAsStream(format: SKEncodedImageFormat.Png)
                ?? throw new ArgumentException($"Could not render LaTeX: {painter.ErrorMessage}", nameof(latex));
            using var memory = new MemoryStream();
            image.CopyTo(memory);

            return memory.ToArray();
        }

        #endregion Methods
    }
}
