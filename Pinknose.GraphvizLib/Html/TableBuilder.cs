using Pinknose.GraphvizLib.Html.Attributes;
using System.Text;

namespace Pinknose.GraphvizLib.Html
{
    public class TableBuilder<TParent> : HtmlBuilderBase where TParent : HtmlBuilderBase
    {
        #region Fields

        private readonly TParent _parent;

        #endregion Fields

        #region Constructors

        internal TableBuilder(TParent parent, params ITableAttribute[] attributes) : base()
        {
            _parent = parent;

            StringBuilder.Append($"<TABLE");

            foreach ( var attribute in attributes )
            {
                StringBuilder.Append($" {attribute.ToString()}");
            }

            StringBuilder.Append(">");
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