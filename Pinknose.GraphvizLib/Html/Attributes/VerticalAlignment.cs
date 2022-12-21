namespace Pinknose.GraphvizLib.Html.Attributes
{
    public class VerticalAlignment : HtmlAttribute, ITableAttribute, ICellAttribute
    {
        #region Properties

        public static VerticalAlignment Bottom => new VerticalAlignment() { Value = "BOTTOM" };
        public static VerticalAlignment Middle => new VerticalAlignment() { Value = "MIDDLE" };
        public static VerticalAlignment Top => new VerticalAlignment() { Value = "TOP" };
        public override string Name => "VALIGN";

        #endregion Properties
    }
}