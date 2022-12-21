using Pinknose.GraphvizLib.Html.Attributes;
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

        public TableCellBuilder<TableRowBuilder<TParent>> StartCell(params ICellAttribute[] attributes) =>
            new TableCellBuilder<TableRowBuilder<TParent>>(this, attributes);


        
        #endregion Methods
    }
}