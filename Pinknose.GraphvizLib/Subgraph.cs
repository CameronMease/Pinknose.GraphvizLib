using Pinknose.GraphvizLib.Attributes;

namespace Pinknose.GraphvizLib
{
    [HeaderText("subgraph")]
    public sealed class Subgraph : GraphvizElement, IParent
    {
        #region Constructors

        public Subgraph(Graph graph)
        {
            Graph = graph;
        }

        #endregion Constructors

        #region Properties

        public List<GraphvizElement> Children { get; } = new List<GraphvizElement>();

        [AttributeName("color")]
        public Color? Color { get; set; } = null;

        [AttributeName("fontcolor")]
        public Color? FontColor { get; set; } = null;

        [AttributeName("fontsize")]
        public double? FontSize { get; set; } = null;

        public Graph Graph { get; }
        public override string Id => (IsCluster ? "cluster_" : "") + base.Id;
        public bool IsCluster { get; set; } = true;

        [AttributeName("labeljust")]
        public HorizontalJustification? LabelJustification { get; set; } = null;

        [AttributeName("penwidth")]
        public double? LineWidth { get; set; } = null;

        [AttributeName("style")]
        public ClusterStyle? Style { get; set; } = null;

        #endregion Properties
    }
}