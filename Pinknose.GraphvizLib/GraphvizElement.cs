using Pinknose.GraphvizLib.Attributes;
using System.Text;

namespace Pinknose.GraphvizLib
{
    public abstract class GraphvizElement : DotRenderer
    {
        #region Fields

        private static readonly int DefaultIndentAmount = 3;

        #endregion Fields

        #region Properties

        [AttributeName("fontsize")]
        public double? FontSize { get; set; } = null;

        public virtual string Id { get; } = "A" + Guid.NewGuid().ToString().Replace("-", "");

        [AttributeName("label")]
        public Label? Label { get; set; } = null;

        [AttributeName("texlbl")]
        public Label? TexLabel { get; set; } = null;

        #endregion Properties

        #region Methods

        internal override string RenderDot(Graph graph, int indent)
        {
            string indentText = new string(' ', indent);
            string nextIndentText = new string(' ', indent + DefaultIndentAmount);

            var headerAttribute = (HeaderTextAttribute)this.GetType().GetCustomAttributes(typeof(HeaderTextAttribute), true).FirstOrDefault();
            var isParent = headerAttribute != null;

            var sb = new StringBuilder();

            if (isParent)
            {
                sb.AppendLine($"{indentText}{headerAttribute?.Text} {this.Id} " + "{");

                sb.AppendLine(this.RenderMultiLineAttributes(indent + DefaultIndentAmount));

                var parentInterface = this.GetType().GetInterface(nameof(IParent));

                if (parentInterface != null)
                {
                    foreach (var child in ((IParent)this).Children)
                    {
                        sb.AppendLine(child.RenderDot(graph, indent + DefaultIndentAmount));
                    }
                }
            }

            if (this.GetType().IsAssignableTo(typeof(Graph)))
            {
                foreach (var edge in ((Graph)this).Edges)
                {
                    sb.Append(edge.RenderDot(graph, indent + DefaultIndentAmount));
                }
            }

            if (isParent)
            {
                sb.AppendLine(indentText + "}");
            }

            return sb.ToString();
        }

        #endregion Methods
    }
}