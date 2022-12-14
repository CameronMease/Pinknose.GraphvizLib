using System.Text;

namespace Pinknose.GraphvizLib.Html
{
    public class TableBuilder<TParent> : HtmlBuilderBase where TParent : HtmlBuilderBase
    {
        #region Fields

        private readonly TParent _parent;

        #endregion Fields

        #region Constructors

        internal TableBuilder(TParent parent, int borderSize = 1) : base()
        {
            _parent = parent;

            StringBuilder.Append($"<TABLE BORDER=\"{borderSize}\">");
        }

        #endregion Constructors

        #region Methods

        public TParent EndTable()
        {
            StringBuilder.Append("</TABLE>");
            _parent.AppendRawString(StringBuilder.ToString());
            return _parent;
        }

        public TableRowBuilder<TableBuilder<TParent>> StartRow() => new TableRowBuilder<TableBuilder<TParent>>(this);

        #endregion Methods
    }
}