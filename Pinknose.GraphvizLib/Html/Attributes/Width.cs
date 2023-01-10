namespace Pinknose.GraphvizLib.Html.Attributes
{
    public sealed class Width : HtmlAttribute, ITableAttribute, ICellAttribute
    {
        #region Constructors

        private Width(string value) : base(value)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "WIDTH";

        #endregion Properties

        #region Methods

        public static Width Set(int value)
        {
            if (value < 0 || value > 65535)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            return new Width(value.ToString());
        }

        #endregion Methods
    }
}