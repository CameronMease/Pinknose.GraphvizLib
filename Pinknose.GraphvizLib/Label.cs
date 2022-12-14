using Pinknose.GraphvizLib.Html;
using System.Text.Encodings.Web;

namespace Pinknose.GraphvizLib
{
    public class Label
    {
        #region Fields

        private static TextEncoderSettings TextEncoderSettings = new TextEncoderSettings();
        private string? _text = null;

        #endregion Fields

        #region Constructors

        private Label()
        {
        }

        #endregion Constructors

        #region Properties

        public bool IsHtml { get; private set; } = false;

        #endregion Properties

        #region Methods

        public static implicit operator Label(string label) => new Label() { _text = label };

        public static implicit operator Label(HtmlBuilder builder) => new Label()
        {
            _text = builder.ToString(),
            IsHtml = true
        };

        public override string ToString() => _text;

        #endregion Methods
    }
}