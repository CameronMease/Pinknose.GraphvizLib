﻿using CSharpMath.SkiaSharp;
using SkiaSharp;

namespace Pinknose.GraphvizLib.Html
{
    internal static class LatexMathRenderer
    {
        #region Methods

        internal static async Task<byte[]> RenderAsync(string latex)
        {
            var painter = new MathPainter() { LaTeX = latex };

            using var image = painter.DrawAsStream(format: SKEncodedImageFormat.Png);

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

        internal static async void RenderToDisk(string latex, string filename)
        {
            var bytes = RenderAsync(latex).GetAwaiter().GetResult();
            File.WriteAllBytes(filename, bytes);
        }

        #endregion Methods
    }
}