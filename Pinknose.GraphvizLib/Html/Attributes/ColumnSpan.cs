namespace Pinknose.GraphvizLib.Html.Attributes
{
    public sealed class ColumnSpan : HtmlAttribute, ICellAttribute
    {
        #region Constructors

        private ColumnSpan(string value) : base(value)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "COLSPAN";

        #endregion Properties

        #region Methods

        public static ColumnSpan Set(int span)
        {
            if (span < 1 || span > 65535)
            {
                throw new ArgumentOutOfRangeException(nameof(span));
            }

            return new ColumnSpan(span.ToString());
        }

        #endregion Methods
    }
}