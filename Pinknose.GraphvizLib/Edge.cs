using Pinknose.GraphvizLib.Attributes;
using System.Text;

namespace Pinknose.GraphvizLib
{
    public class Edge : DotRenderer
    {
        #region Constructors

        public Edge(GraphvizElement source, GraphvizElement destination)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Destination = destination ?? throw new ArgumentNullException(nameof(destination));
        }

        #endregion Constructors

        #region Properties

        [AttributeName("arrowhead")]
        public ArrowType? ArrowHead { get; set; } = null;

        [AttributeName("arrowtail")]
        public ArrowType? ArrowTail { get; set; } = null;

        [AttributeName("color")]
        public Color? Color { get; set; } = null;

        public GraphvizElement Destination { get; }

        [AttributeName("fontcolor")]
        public Color? FontColor { get; set; } = null;

        [AttributeName("fontsize")]
        public double? FontSize { get; set; } = null;

        [AttributeName("label")]
        public Label? Label { get; set; } = null;

        [AttributeName("penwidth")]
        public double? LineWidth { get; set; } = null;

        public GraphvizElement Source { get; }

        [AttributeName("style")]
        public EdgeStyle? Style { get; set; } = null;

        [AttributeName("headport")]
        public PortPosition? HeadPort { get; set; } = null;

        [AttributeName("tailport")]
        public PortPosition? TailPort { get; set; } = null;

        public string? StartPortName { get; set; } = null;
        public string? EndPortName { get; set; } = null;

        #endregion Properties

        #region Methods

        internal override string RenderDot(Graph graph, int indent)
        {
            string connectorSyntax = graph.GetType() == typeof(Digraph) ? "->" : "--";

            string indentText = new string(' ', indent);

            var startSourceId = StartPortName is null ? Source.Id : $"{Source.Id}:{StartPortName}";
            var endSourceId = EndPortName is null ? Destination.Id : $"{Destination.Id}:{EndPortName}";

            var sb = new StringBuilder();

            sb.AppendLine($"{indentText}{startSourceId}{connectorSyntax}{endSourceId} {this.RenderSingleLineAttributes()};");

            return sb.ToString();
        }

        #endregion Methods
    }
}