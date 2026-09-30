using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pinknose.GraphvizLib.Html;
using Pinknose.GraphvizLib.Html.Attributes;
using System;
using System.Linq;

namespace Pinknose.GraphvizLib.Tests
{
    [TestClass]
    public sealed class HtmlBuilderTests
    {
        [TestMethod]
        public void Text_is_escaped()
        {
            var html = new HtmlBuilder().AppendText("<Qty & \"Unit\">").ToString();

            Assert.AreEqual("<&lt;Qty &amp; &quot;Unit&quot;&gt;>", html);
        }

        [TestMethod]
        public void Attribute_values_are_escaped()
        {
            var html = new HtmlBuilder()
                .StartTable()
                .StartRow().StartCell(Tooltip.Set("a \"b\" & <c>")).EndCell().EndRow()
                .EndTable()
                .ToString();

            StringAssert.Contains(html, "<TD TOOLTIP=\"a &quot;b&quot; &amp; &lt;c&gt;\">");
        }

        [TestMethod]
        public void Formats_nest_properly()
        {
            var html = new HtmlBuilder().AppendText("x", HtmlTextFormat.Bold | HtmlTextFormat.Italics).ToString();

            Assert.AreEqual("<<I><B>x</B></I>>", html);
        }

        [TestMethod]
        public void A_font_wraps_its_text()
        {
            var html = new HtmlBuilder()
                .StartFont(FontFace.Set("Arial"), PointSize.Set(10), FontColor.Set(Color.Gray))
                .AppendText("x")
                .EndFont()
                .ToString();

            Assert.AreEqual("<<FONT FACE=\"Arial\" POINT-SIZE=\"10\" COLOR=\"Gray\">x</FONT>>", html);
        }

        [TestMethod]
        public void Tables_nest_and_take_rules()
        {
            var html = new HtmlBuilder()
                .StartTable(CellBorder.Set(0), CellSpacing.Set(0))
                .StartRow()
                    .StartCell(Id.Set("header"), Href.Set("#t"))
                        .StartTable().StartRow().StartCell().AppendText("in").EndCell().EndRow().EndTable()
                    .EndCell()
                    .AppendVerticalRule()
                    .StartCell().AppendText("b").EndCell()
                .EndRow()
                .AppendHorizontalRule()
                .StartRow().StartCell(BackgroundColor.Set(Color.LightGray)).AppendText("c").EndCell().EndRow()
                .EndTable()
                .ToString();

            Assert.AreEqual(
                "<<TABLE CELLBORDER=\"0\" CELLSPACING=\"0\"><TR><TD ID=\"header\" HREF=\"#t\"><TABLE><TR><TD>in</TD></TR></TABLE></TD><VR/><TD>b</TD></TR>" +
                "<HR/><TR><TD BGCOLOR=\"lightgray\">c</TD></TR></TABLE>>",
                html);
        }

        [TestMethod]
        public void The_warning_icon_is_a_png_added_on_first_use()
        {
            var cache = new HtmlImageCache();
            Assert.AreEqual(0, cache.Count);

            var guid = cache.GetIconGuid(Icons.Warning);

            Assert.AreEqual(guid, cache.GetIconGuid(Icons.Warning));
            Assert.AreEqual(1, cache.Count);
            Assert.AreEqual("png", cache[guid].Extension);
            CollectionAssert.AreEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47 }, cache[guid].Bytes.Take(4).ToArray());
        }

        [TestMethod]
        public void Out_of_range_sizes_are_refused()
        {
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => CellPadding.Set(256));
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => PointSize.Set(0));
        }
    }
}
