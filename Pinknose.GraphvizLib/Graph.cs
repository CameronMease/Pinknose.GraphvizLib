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

using Pinknose.GraphvizLib.Attributes;
using Pinknose.GraphvizLib.Html;
using SkiaSharp;
using Svg;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pinknose.GraphvizLib
{
    public abstract class Graph : GraphvizElement, IParent
    {
        #region Constructors

        public Graph(HtmlImageCache imageCache) : base(imageCache)
        {
            Edges.AddingItem += Edges_AddingItem;
        }

        #endregion Constructors

        #region Properties

        [AttributeName("bgcolor")]
        public Color? BackgroundColor { get; set; } = null;

        /// <summary>
        /// Determines if the graph will be centered in the canvas.
        /// </summary>
        [AttributeName("center")]
        public bool? Center { get; set; } = null;

        public HashSet<GraphvizElement> Children { get; } = new();

        public ExtendedList<Edge> Edges { get; } = new();

        [AttributeName("fontcolor")]
        public Color? FontColor { get; set; } = null;

        Graph IParent.Graph => this;

        [AttributeName("labeljust")]
        public HorizontalJustification? LabelHorizontalJustification { get; set; } = null;

        [AttributeName("labelloc")]
        public VerticalJustification? LabelVerticalJustification { get; set; } = null;

        [AttributeName("rankdir")]
        public RankDirection? RankDirection { get; set; } = null;

        [AttributeName("splines")]
        public SplineType SplineType { get; set; } = SplineType.Spline;

        #endregion Properties

        #region Methods

        public Dot RenderDot() => this.RenderDot(this, 0);

        public async Task<(SKBitmap Png, SvgDocument Svg)> RenderGraphAsync(GraphvizEngine engine)
        {
            SKBitmap? png = null;
            SvgDocument? svg = null;

            await Task.Run(() =>
                Parallel.Invoke(
                    () => png = DotExe.RenderPngAsync(this, engine).GetAwaiter().GetResult(),
                    () => svg = DotExe.RenderSvgAsync(this, engine).GetAwaiter().GetResult()
               )
            );

            return (png ?? throw new ArgumentNullException(), svg ?? throw new ArgumentNullException());
        }

        public async Task<SKBitmap> RenderPngAsync(GraphvizEngine engine) => await DotExe.RenderPngAsync(this, engine);

        public async Task<SvgDocument> RenderSvgAsync(GraphvizEngine engine) => await DotExe.RenderSvgAsync(this, engine);

        public async Task RenderToPngFileAsync(GraphvizEngine engine, string filename) => await DotExe.RenderToPngFileAsync(this, engine, filename);

        public async Task RenderToSvgFileAsync(GraphvizEngine engine, string filename) => await DotExe.RenderToSvgFileAsync(this, engine, filename);

        private void Edges_AddingItem(object? sender, AddingItemEventArgs<Edge> e)
        {
            if (!Children.Contains(e.Item.Source))
            {
                //TODO: Revisit
                //throw new NotImplementedException("Edge is being added but source node is not a child of this graph.");
            }
            else if (!Children.Contains(e.Item.Destination))
            {
                //TODO: Revisit
                //throw new NotImplementedException("Edge is being added but destination node is not a child of this graph.");
            }
        }

        #endregion Methods
    }
}