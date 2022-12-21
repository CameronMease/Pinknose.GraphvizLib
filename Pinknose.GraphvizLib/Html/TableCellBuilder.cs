using Pinknose.GraphvizLib.Html.Attributes;
using System.Text;

namespace Pinknose.GraphvizLib.Html
{
    public class TableCellBuilder<TParent> : HtmlBuilderBase where TParent : HtmlBuilderBase
    {
        #region Fields

        private static string? IconsPath = null;

        private readonly TParent _parent;

        #endregion Fields

        #region Constructors

        internal TableCellBuilder(TParent parent, params ICellAttribute[] attributes) : base()
        {
            _parent = parent;

            StringBuilder.Append($"<TD ");

            foreach (var attribute in attributes)
            {
                StringBuilder.Append($" {attribute.ToString()}");
            }

            StringBuilder.Append(">");
        }

        #endregion Constructors

        #region Methods

        public TableCellBuilder<TParent> AppendCell(params ICellAttribute[] attributes) =>
            new TableCellBuilder<TParent>(_parent, attributes);

        public TableCellBuilder<TParent> AppendImage(string filename)
        {
            StringBuilder.Append(SharedFormatting.FormatImage(filename));
            return this;
        }

        public TableCellBuilder<TParent> AppendLatexImage(string latex)
        {
            StringBuilder.Append(SharedFormatting.FormatLatex(latex));
            return this;
        }

        public TableCellBuilder<TParent> AppendLineBreak()
        {
            StringBuilder.Append(SharedFormatting.FormatLineBreak());
            return this;
        }

        public TableCellBuilder<TParent> AppendText(string text, HtmlTextFormat format = HtmlTextFormat.None)
        {
            StringBuilder.Append(SharedFormatting.FormatText(text, format));
            return this;
        }

        public TableCellBuilder<TParent> AppendWarningIcon() => AppendIcon("StatusWarning.png");

        public TParent EndCell()
        {
            StringBuilder.Append("</TD>");
            _parent.AppendRawString(StringBuilder.ToString());
            return _parent;
        }

        private TableCellBuilder<TParent> AppendIcon(string filename)
        {
            if (IconsPath == null)
            {
                IconsPath = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), "Html", "Icons");
            }

            var fullPath = Path.Combine(IconsPath, filename);

            AppendImage(fullPath);

            return this;
        }

        #endregion Methods
    }
}