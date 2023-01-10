namespace Pinknose.GraphvizLib.Html.Attributes
{
    public sealed class FixedSize : HtmlAttribute, ITableAttribute, ICellAttribute
    {
        #region Constructors

        private FixedSize(string value) : base(value)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "FIXEDSIZE";

        #endregion Properties

        #region Methods

        public static FixedSize Set(bool value)
        {
            return new FixedSize(value.ToString().ToLower());
        }

        #endregion Methods
    }
}