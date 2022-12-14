using System.Text;

namespace Pinknose.GraphvizLib.Html
{
    public class TableRowBuilder<TParent> : HtmlBuilderBase where TParent : HtmlBuilderBase
    {
        #region Fields

        private readonly TParent _parent;

        #endregion Fields

        #region Constructors

        internal TableRowBuilder(TParent parent) : base()
        {
            _parent = parent;

            StringBuilder.Append("<TR>");
        }

        #endregion Constructors

        #region Methods

        public TParent EndRow()
        {
            StringBuilder.Append("</TR>");
            _parent.AppendRawString(StringBuilder.ToString());
            return _parent;
        }

        public TableCellBuilder<TableRowBuilder<TParent>> StartCell(Alignment alignment = Alignment.Center, string? portName = null, int? rowSpan = null, int? columnSpan = null) =>
            new TableCellBuilder<TableRowBuilder<TParent>>(this, alignment: alignment, portName: portName, rowSpan: rowSpan, columnSpan : columnSpan);

        #endregion Methods
    }
}