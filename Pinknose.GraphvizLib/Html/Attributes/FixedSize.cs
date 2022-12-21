namespace Pinknose.GraphvizLib.Html.Attributes
{
    public class FixedSize : HtmlAttribute, ITableAttribute, ICellAttribute
    {
        #region Properties

        public override string Name => "FIXEDSIZE";

        #endregion Properties

        #region Methods

        public static FixedSize Set(bool value)
        {
            return new FixedSize() { Value = value.ToString().ToLower() };
        }

        #endregion Methods
    }
}