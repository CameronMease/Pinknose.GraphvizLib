using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Threading;

namespace Pinknose.GraphvizLib.Tests
{
    [TestClass]
    public sealed class DotStringTests
    {
        [TestMethod]
        public void Quote_escapes_quotes_and_backslashes()
        {
            Assert.AreEqual("\"say \\\\\\\"hi\\\"\"", DotString.Quote("say \\\"hi\""));
        }

        [TestMethod]
        public void Quote_keeps_a_trailing_backslash_from_escaping_the_closing_quote()
        {
            Assert.AreEqual("\"C:\\\\\"", DotString.Quote("C:\\"));
        }

        [TestMethod]
        public void Quote_turns_line_breaks_into_newline_escapes()
        {
            Assert.AreEqual("\"a\\nb\\nc\"", DotString.Quote("a\r\nb\nc"));
        }

        [TestMethod]
        public void QuoteEscString_keeps_escapes_and_escapes_bare_quotes()
        {
            Assert.AreEqual("\"left\\l\\\"q\\\" \\\"x\"", DotString.QuoteEscString("left\\l\\\"q\\\" \"x"));
        }

        [TestMethod]
        public void QuoteEscString_doubles_a_lone_trailing_backslash()
        {
            Assert.AreEqual("\"a\\\\\"", DotString.QuoteEscString("a\\"));
        }

        [TestMethod]
        public void FormatNumber_ignores_the_current_culture()
        {
            var previous = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
                Assert.AreEqual("1.5", DotString.FormatNumber(1.5));
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = previous;
            }
        }
    }
}
