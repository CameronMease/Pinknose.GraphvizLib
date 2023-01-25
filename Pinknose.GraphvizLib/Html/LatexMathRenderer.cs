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

using CSharpMath.SkiaSharp;
using SkiaSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Pinknose.GraphvizLib.Html
{
    internal static class LatexMathRenderer
    {
        #region Methods

        internal static async Task<byte[]> RenderAsync(string latex)
        {
            var painter = new MathPainter() { LaTeX = latex };

            using var image = painter.DrawAsStream(format: SKEncodedImageFormat.Png) ?? throw new ArgumentNullException();

            byte[] bytes = new byte[image.Length];

            image.Seek(0, SeekOrigin.Begin);

            var byteCount = 0;
            var bytesRead = 0;

            do
            {
                bytesRead = await image.ReadAsync(bytes, byteCount, bytes.Length - byteCount);
                byteCount += bytesRead;
            } while (bytesRead > 0);

            return bytes;
        }

        internal static async Task RenderToDiskAsync(string latex, string filename)
        {
            var bytes = await RenderAsync(latex);
            await File.WriteAllBytesAsync(filename, bytes);
        }

        #endregion Methods
    }
}