namespace Pinknose.GraphvizLib.Html.Attributes
{
    public class Alignment : HtmlAttribute, ITableAttribute, ICellAttribute
    {
        #region Properties

        public static Alignment Center => new Alignment() { Value = "CENTER" };
        public static Alignment Left => new Alignment() { Value = "LEFT" };
        public static Alignment Right => new Alignment() { Value = "RIGHT" };
        public override string Name => "ALIGN";

        #endregion Properties
    }
}