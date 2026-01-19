namespace Pinknose.GraphvizLib.Html.Attributes
{
    public sealed class PortName : HtmlAttribute, ICellAttribute, ITableAttribute
    {
        #region Constructors

        private PortName(string value) : base(value)
        {
        }

        #endregion Constructors

        #region Properties

        public override string Name => "PORT";

        #endregion Properties

        #region Methods

        public static PortName Set(string name) => new PortName(name);

        #endregion Methods

    }
}