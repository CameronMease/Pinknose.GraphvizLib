namespace Pinknose.GraphvizLib
{
    internal static class StringExtensions
    {
        #region Methods

        internal static string SurroundInQuotes(this string text) => $"\"{text}\"";

        internal static string SurroundInQuotes(this string text, bool surround) => surround ? SurroundInQuotes(text) : text;

        #endregion Methods
    }
}