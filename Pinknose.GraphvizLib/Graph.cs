using Pinknose.GraphvizLib.Attributes;
using SkiaSharp;
using Svg;

namespace Pinknose.GraphvizLib
{
    public abstract class Graph : GraphvizElement, IParent
    {
        public Graph()
        {
            Edges.AddingItem += Edges_AddingItem;
        }

        private void Edges_AddingItem(object? sender, AddingItemEventArgs<Edge> e)
        {
            if (!Children.Contains(e.Item.Source))
            {
                throw new NotImplementedException("Edge is being added but source node is not a child of this graph.");
            }
            else if (!Children.Contains(e.Item.Destination))
            {
                throw new NotImplementedException("Edge is being added but destination node is not a child of this graph.");
            }
        }

        #region Properties

        [AttributeName("bgcolor")]
        public Color? BackgroundColor { get; set; } = null;

        /// <summary>
        /// Determines if the graph will be centered in the canvas.
        /// </summary>
        [AttributeName("center")]
        public bool? Center { get; set; } = null;

        public List<GraphvizElement> Children { get; } = new List<GraphvizElement>();

        public ExtendedList<Edge> Edges { get; } = new();

        [AttributeName("fontcolor")]
        public Color? FontColor { get; set; } = null;

        Graph IParent.Graph => this;

        [AttributeName("labeljust")]
        public HorizontalJustification? LabelJustification { get; set; } = null;

        [AttributeName("rankdir")]
        public RankDirection? RankDirection { get; set; } = null;

        [AttributeName("splines")]
        public SplineType SplineType { get; set; } = SplineType.Spline;

        #endregion Properties

        #region Methods

        public string RenderDot() => this.RenderDot(this, 0);

        public async Task<(SKBitmap Png, SvgDocument Svg)> RenderGraphAsync(GraphvizEngine engine)
        {
            SKBitmap png = null;
            SvgDocument svg = null;

            await Task.Run(() =>
                Parallel.Invoke(
                    () => png = Dot.RenderPngAsync(this, engine).GetAwaiter().GetResult(),
                    () => svg = Dot.RenderSvgAsync(this, engine).GetAwaiter().GetResult()
               )
            );

            return (png, svg);
        }

        public async Task<SKBitmap> RenderPngAsync(GraphvizEngine engine) => await Dot.RenderPngAsync(this, engine);

        public async Task<SvgDocument> RenderSvgAsync(GraphvizEngine engine) => await Dot.RenderSvgAsync(this, engine);

        #endregion Methods
    }
}