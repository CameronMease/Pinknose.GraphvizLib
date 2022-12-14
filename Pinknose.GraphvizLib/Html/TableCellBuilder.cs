using System.Text;

namespace Pinknose.GraphvizLib.Html
{
    public class TableCellBuilder<TParent> : HtmlBuilderBase where TParent : HtmlBuilderBase
    {
        #region Fields

        private readonly TParent _parent;

        #endregion Fields

        #region Constructors

        internal TableCellBuilder(TParent parent, Alignment alignment, string? portName, int? rowSpan, int? columnSpan) : base()
        {
            _parent = parent;

            StringBuilder.Append($"<TD ALIGN=\"{((Enum)alignment).GetDisplayValue()}\"");
            
            if (portName!= null)
            {
                StringBuilder.Append($" PORT=\"{portName}\"");
            }

            if (rowSpan != null)
            {
                StringBuilder.Append($" ROWSPAN=\"{rowSpan}\"");
            }

            if (columnSpan != null)
            {
                StringBuilder.Append($" COLSPAN=\"{columnSpan}\"");
            }

            StringBuilder.Append(">");
        }

        #endregion Constructors

        #region Methods

        public TableCellBuilder<TParent> AppendCell(Alignment alignment = Alignment.Center, string? portName = null, int? rowSpan = null, int? columnSpan = null) =>
            new TableCellBuilder<TParent>(_parent, alignment: alignment, portName: portName, rowSpan: rowSpan, columnSpan : columnSpan);

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

        public TParent EndCell()
        {
            StringBuilder.Append("</TD>");
            _parent.AppendRawString(StringBuilder.ToString());
            return _parent;
        }

        #endregion Methods
    }
}