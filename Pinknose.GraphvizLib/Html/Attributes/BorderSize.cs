namespace Pinknose.GraphvizLib.Html.Attributes
{
    public sealed class BorderSize : HtmlAttribute, ITableAttribute, ICellAttribute
    {
        #region Constructors

        private BorderSize(string value) : base(value)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "BORDER";

        #endregion Properties

        #region Methods

        public static BorderSize Set(int size)
        {
            if (size < 0 || size > 127)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            return new BorderSize(size.ToString());
        }

        #endregion Methods
    }
}