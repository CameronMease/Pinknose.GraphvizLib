namespace Pinknose.GraphvizLib.Html.Attributes
{
    public class ColumnSpan : HtmlAttribute, ICellAttribute
    {
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

            return new ColumnSpan() { Value = span.ToString() };
        }

        #endregion Methods
    }
}