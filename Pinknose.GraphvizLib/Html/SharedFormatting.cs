using System.Text;

namespace Pinknose.GraphvizLib.Html
{
    internal static class SharedFormatting
    {
        #region Methods

        public static string FormatImage(string filename) => $"<IMG SRC=\"{filename}\"/>";

        public static string FormatLatex(string latex) => FormatLatexAsync(latex).GetAwaiter().GetResult();

        public static async Task<string> FormatLatexAsync(string latex)
        {
            var filename = Path.ChangeExtension(Path.GetTempFileName(), "png");

            await LatexMathRenderer.RenderToDiskAsync(latex, filename);

            return FormatImage(filename);
        }

        public static string FormatLineBreak() => "<BR/>";

        public static string FormatText(string text, HtmlTextFormat format = HtmlTextFormat.None)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(text))
            {
                foreach (var option in Enum.GetValues(typeof(HtmlTextFormat)))
                {
                    if (((int)format & (int)option) != 0)
                    {
                        sb.AppendFormat("<{0}>", ((Enum)option).GetDisplayValue());
                    }
                }

                sb.Append(text);

                foreach (var option in Enum.GetValues(typeof(HtmlTextFormat)))
                {
                    if (((int)format & (int)option) != 0)
                    {
                        sb.AppendFormat("</{0}>", ((Enum)option).GetDisplayValue());
                    }
                }
            }

            return sb.ToString();
        }

        #endregion Methods
    }
}