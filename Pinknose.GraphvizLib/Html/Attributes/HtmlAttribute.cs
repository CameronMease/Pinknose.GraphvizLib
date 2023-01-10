namespace Pinknose.GraphvizLib.Html.Attributes
{
    public interface ICellAttribute : IHtmlAttribute
    {
    }

    public interface IFontAttribute : IHtmlAttribute
    { }

    public interface IHtmlAttribute
    {
        #region Properties

        public string Name { get; }
        public string Value { get; }

        #endregion Properties

        #region Methods

        public string ToString();

        #endregion Methods
    }

    public interface ITableAttribute : IHtmlAttribute
    {
    }

    public abstract class HtmlAttribute : IHtmlAttribute
    {
        #region Constructors

        public HtmlAttribute(string value)
        {
            Value = value;
        }

        #endregion Constructors

        #region Properties

        public abstract string Name { get; }

        public string Value { get; }

        #endregion Properties

        #region Methods

        public override string ToString() => $"{Name}=\"{Value}\"";

        #endregion Methods
    }
}