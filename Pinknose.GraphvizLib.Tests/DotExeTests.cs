using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pinknose.GraphvizLib.Html;
using Pinknose.GraphvizLib.Html.Attributes;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pinknose.GraphvizLib.Tests
{
    /// <summary>
    /// Runs the real Graphviz. Inconclusive where it is not installed, so the unit tests still run
    /// on a machine without it; CI installs it.
    /// </summary>
    [TestClass]
    public sealed class DotExeTests
    {
        [TestInitialize]
        public void RequireGraphviz()
        {
            try
            {
                _ = DotExe.ExecutablePath;
            }
            catch (FileNotFoundException)
            {
                Assert.Inconclusive("Graphviz is not installed.");
            }
        }

        [TestMethod]
        public async Task Svg_output_carries_ids_classes_and_tooltips()
        {
            var graph = new Digraph { Name = "G" };
            var a = new Node { Name = "a", SvgId = "table-a", Class = "table", Tooltip = "The \"A\" table", Label = "A & B" };
            var b = new Node { Name = "b", SvgId = "table-b" };
            graph.Children.Add(a);
            graph.Children.Add(b);
            graph.Edges.Add(new Edge(a, b) { SvgId = "edge-1" });

            var svg = XDocument.Parse(await graph.RenderSvgStringAsync());
            XNamespace ns = "http://www.w3.org/2000/svg";

            var groupA = svg.Descendants(ns + "g").Single(g => (string?)g.Attribute("id") == "table-a");
            StringAssert.Contains((string?)groupA.Attribute("class"), "table");
            Assert.IsTrue(svg.Descendants(ns + "g").Any(g => (string?)g.Attribute("id") == "edge-1"));
            Assert.IsTrue(svg.Descendants(ns + "text").Any(t => t.Value == "A & B"));
            Assert.IsTrue(svg.Descendants().Any(e => ((string?)e.Attribute(XName.Get("title", "http://www.w3.org/1999/xlink")))?.Contains("The \"A\" table") == true
                || e.Value.Contains("The \"A\" table")));
        }

        [TestMethod]
        public async Task Html_label_ports_carry_edges()
        {
            var graph = new Digraph { Name = "G", SplineType = GraphvizLib.SplineType.Spline };
            var table = new Node
            {
                Name = "t",
                Shape = GraphvizLib.Shape.PlainText,
                Label = new HtmlBuilder()
                    .StartTable(BorderSize.Set(0), CellBorder.Set(1))
                    .StartRow().StartCell(PortName.Set("header")).AppendText("Tasks").EndCell().EndRow()
                    .StartRow().StartCell(PortName.Set("part"), Tooltip.Set("Part"), Href.Set("#part")).AppendText("Part → Parts").EndCell().EndRow()
                    .EndTable(),
            };
            var parts = new Node { Name = "p", Label = "Parts" };
            graph.Children.Add(table);
            graph.Children.Add(parts);
            graph.Edges.Add(new Edge(table, parts) { StartPortName = "part", TailPort = PortPosition.East });

            var svg = await graph.RenderSvgStringAsync();

            // The cell's link and tooltip survive, and the edge leaves the table's node.
            StringAssert.Contains(svg, "Part → Parts");
            StringAssert.Contains(svg, "xlink:href=\"#part\" xlink:title=\"Part\"");
            StringAssert.Contains(svg, "<title>t:e&#45;&gt;p</title>");
        }

        [TestMethod]
        public async Task An_image_in_a_label_renders_and_its_temp_file_is_removed()
        {
            var cache = new HtmlImageCache();
            var graph = new Digraph(cache) { Name = "G" };
            graph.Children.Add(new Node(cache)
            {
                Name = "n",
                Shape = GraphvizLib.Shape.PlainText,
                Label = new HtmlBuilder()
                    .StartTable().StartRow().StartCell().AppendIcon(Icons.Warning, cache).EndCell().EndRow().EndTable(),
            });

            var before = Directory.GetFiles(Path.GetTempPath(), "graphviz-*").Length;
            var svg = await graph.RenderSvgStringAsync();
            var after = Directory.GetFiles(Path.GetTempPath(), "graphviz-*").Length;

            StringAssert.Contains(svg, "<image");
            Assert.IsLessThanOrEqualTo(before, after, "a temp image file was left behind");
        }

        [TestMethod]
        public async Task Png_output_decodes()
        {
            var graph = new Digraph { Name = "G" };
            graph.Children.Add(new Node { Name = "a" });

            using var png = await graph.RenderPngAsync();

            Assert.IsTrue(png.Width > 0 && png.Height > 0);
        }

        [TestMethod]
        public async Task The_engine_argument_is_used()
        {
            var graph = new UndirectedGraph { Name = "G" };
            var a = new Node { Name = "a" };
            var b = new Node { Name = "b" };
            graph.Children.Add(a);
            graph.Children.Add(b);
            graph.Edges.Add(new Edge(a, b));

            var dotLayout = await DotExe.RenderAsync(graph, "plain", GraphvizEngine.Dot);
            var circoLayout = await DotExe.RenderAsync(graph, "plain", GraphvizEngine.Circo);

            CollectionAssert.AreNotEqual(dotLayout, circoLayout);
        }

        [TestMethod]
        public async Task A_graphviz_failure_throws_with_its_message()
        {
            var graph = new Digraph();
            graph.Children.Add(new Node());

            var ex = await Assert.ThrowsExactlyAsync<GraphvizException>(() => DotExe.RenderAsync(graph, "no-such-format"));

            Assert.AreNotEqual(0, ex.ExitCode);
            StringAssert.Contains(ex.ErrorOutput, "no-such-format");
        }

        [TestMethod]
        public async Task A_large_graph_does_not_deadlock()
        {
            var graph = new Digraph { Name = "G" };
            Node? previous = null;
            for (var i = 0; i < 2000; i++)
            {
                var node = new Node { Name = "n" + i, Label = new string('x', 40) };
                graph.Children.Add(node);
                if (previous is not null)
                {
                    graph.Edges.Add(new Edge(previous, node));
                }

                previous = node;
            }

            var bytes = await DotExe.RenderAsync(graph, "svg").WaitAsync(TimeSpan.FromMinutes(2));

            Assert.IsGreaterThan(100_000, bytes.Length);
        }
    }
}
