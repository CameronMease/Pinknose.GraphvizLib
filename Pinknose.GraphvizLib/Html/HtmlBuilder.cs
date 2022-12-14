using Pinknose.GraphvizLib.Atttributes;
using System.Text;

namespace Pinknose.GraphvizLib.Html
{
    [Flags]
    public enum HtmlTextFormat
    {
        None = 0,

        [DisplayValue("I")]
        Italics = 1 << 0,

        [DisplayValue("B")]
        Bold = 1 << 1,

        [DisplayValue("U")]
        Underline = 1 << 2,

        [DisplayValue("O")]
        Overline = 1 << 3,

        [DisplayValue("SUB")]
        Subscript = 1 << 4,

        [DisplayValue("SUP")]
        Superscript = 1 << 5,

        [DisplayValue("S")]
        Strikeout = 1 << 6
    }

    public class HtmlBuilder : HtmlBuilderBase
    {
        #region Methods

        public HtmlBuilder AppendLineBreak()
        {
            StringBuilder.Append(SharedFormatting.FormatLineBreak());
            return this;
        }

        public HtmlBuilder AppendText(string text, HtmlTextFormat format = HtmlTextFormat.None)
        {
            StringBuilder.Append(SharedFormatting.FormatText(text, format));
            return this;
        }

        public TableBuilder<HtmlBuilder> StartTable(int borderSize = 1) => new TableBuilder<HtmlBuilder>(this, borderSize: borderSize);

        public override string ToString()
        {
            return "<" + base.ToString() + ">";
        }

        #endregion Methods
    }

    public abstract class HtmlBuilderBase
    {
        #region Properties

        protected StringBuilder StringBuilder { get; } = new StringBuilder();

        #endregion Properties

        #region Methods

        public override string ToString() => StringBuilder.ToString();

        internal void AppendRawString(string text) => StringBuilder.Append(text);

        #endregion Methods
    }
}