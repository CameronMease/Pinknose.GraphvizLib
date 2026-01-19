namespace Pinknose.GraphvizLib.Html.Attributes
{
    public sealed class Alignment : HtmlAttribute, ITableAttribute, ICellAttribute
    {
        #region Constructors

        private Alignment(string value) : base(value)
        {
        }

        #endregion Constructors

        #region Properties

        public static Alignment Center => new("CENTER");
        public static Alignment Left => new("LEFT");
        public static Alignment Right => new("RIGHT");
        public override string Name => "ALIGN";

        #endregion Properties
    }
}