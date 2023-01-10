namespace Pinknose.GraphvizLib.Html.Attributes
{
    public sealed class FontColor : HtmlAttribute, IFontAttribute
    {
        #region Constructors

        private FontColor(string value) : base(value)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "COLOR";

        #endregion Properties

        #region Methods

        public static FontColor Set(Color color)
        {
            return new FontColor(color.GetDisplayValue());
        }

        #endregion Methods
    }
}