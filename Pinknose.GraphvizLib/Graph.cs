/////////////////////////////////////////////////////////////////////////////////
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
using SkiaSharp;
using Svg;
using System.Text;

namespace Pinknose.GraphvizLib
{
    public abstract class Graph : GraphvizElement, IParent
    {

        #region Properties

        [AttributeName("bgcolor")]
        public Color? BackgroundColor { get; set; } = null;

        /// <summary>
        /// Determines if the graph will be centered in the canvas.
        /// </summary>
        [AttributeName("center")]
        public bool? Center { get; set; } = null;

        public List<GraphvizElement> Children { get; } = new List<GraphvizElement>();
        public List<Edge> Edges { get; } = new List<Edge>();

        [AttributeName("fontcolor")]
        public Color? FontColor { get; set; } = null;

        [AttributeName("labeljust")]
        public HorizontalJustification? LabelJustification { get; set; } = null;

        [AttributeName("rankdir")]
        public RankDirection? RankDirection { get; set; } = null;

        #endregion Properties

        #region Methods

        public string RenderDot() => this.RenderDot(this, 0);

        public async Task<(SKBitmap Png, SvgDocument Svg)> RenderGraphAsync()
        {
            SKBitmap png = null;
            SvgDocument svg = null;

            await Task.Run(() =>
                Parallel.Invoke(
                    () => png = Dot.RenderPngAsync(this).GetAwaiter().GetResult(),
                    () => svg = Dot.RenderSvgAsync(this).GetAwaiter().GetResult()
               )
            );

            return (png, svg);
        }

        #endregion Methods

    }
}