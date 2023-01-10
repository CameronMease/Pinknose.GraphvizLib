namespace Pinknose.GraphvizLib.Html.Attributes
{
    public sealed class RowSpan : HtmlAttribute, ICellAttribute
    {
        #region Constructors

        private RowSpan(string value) : base(value)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "ROWSPAN";

        #endregion Properties

        #region Methods

        public static RowSpan Set(int span)
        {
            if (span < 1 || span > 65535)
            {
                throw new ArgumentOutOfRangeException(nameof(span));
            }

            return new RowSpan(span.ToString());
        }

        #endregion Methods
    }
}