namespace Pinknose.GraphvizLib.Html.Attributes
{
    public sealed class VerticalAlignment : HtmlAttribute, ITableAttribute, ICellAttribute
    {
        #region Constructors

        private VerticalAlignment(string value) : base(value)
        {
        }

        #endregion Constructors

        #region Properties

        public static VerticalAlignment Bottom => new VerticalAlignment("BOTTOM");
        public static VerticalAlignment Middle => new VerticalAlignment("MIDDLE");
        public static VerticalAlignment Top => new VerticalAlignment("TOP");
        public override string Name => "VALIGN";

        #endregion Properties
    }
}