namespace Pinknose.GraphvizLib.Atttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class DisplayValueAttribute : Attribute
    {
        #region Constructors

        internal DisplayValueAttribute(string displayValue)
        {
            DisplayValue = displayValue;
        }

        #endregion Constructors

        #region Properties

        internal string DisplayValue { get; }

        #endregion Properties
    }
}