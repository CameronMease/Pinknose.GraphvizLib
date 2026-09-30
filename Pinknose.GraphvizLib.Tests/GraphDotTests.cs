using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pinknose.GraphvizLib.Html;
using Pinknose.GraphvizLib.Html.Attributes;

namespace Pinknose.GraphvizLib.Tests
{
    [TestClass]
    public sealed class GraphDotTests
    {
        [TestMethod]
        public void A_simple_digraph_renders_exactly()
        {
            var graph = new Digraph { Name = "G", RankDirection = GraphvizLib.RankDirection.LeftToRight };
            var a = new Node { Name = "a", Label = "A", Shape = GraphvizLib.Shape.Box };
            var b = new Node { Name = "b" };
            graph.Children.Add(a);
            graph.Children.Add(b);
            graph.Edges.Add(new Edge(a, b) { ArrowHead = ArrowType.Diamond });

            var expected =
                "digraph \"G\" {\n" +
                "   rankdir=\"LR\";\n" +
                "   \"a\" [label=\"A\", shape=\"box\"];\n" +
                "   \"b\";\n" +
                "   \"a\" -> \"b\" [arrowhead=\"diamond\"];\n" +
                "}\n";

            Assert.AreEqual(expected, graph.RenderDot().DotSource.Replace("\r\n", "\n"));
        }

        [TestMethod]
        public void An_undirected_graph_uses_the_undirected_keyword_and_operator()
        {
            var graph = new UndirectedGraph { Name = "U" };
            var a = new Node { Name = "a" };
            var b = new Node { Name = "b" };
            graph.Children.Add(a);
            graph.Children.Add(b);
            graph.Edges.Add(new Edge(a, b));

            var dot = graph.RenderDot().DotSource;

            StringAssert.StartsWith(dot, "graph \"U\" {");
            StringAssert.Contains(dot, "\"a\" -- \"b\";");
        }

        [TestMethod]
        public void Two_nodes_with_identical_attributes_are_both_kept()
        {
            var graph = new Digraph();
            graph.Children.Add(new Node { Label = "same" });
            graph.Children.Add(new Node { Label = "same" });

            Assert.AreEqual(2, graph.Children.Count);
        }

        [TestMethod]
        public void Adding_the_same_node_twice_keeps_one()
        {
            var graph = new Digraph();
            var node = new Node();
            graph.Children.Add(node);
            graph.Children.Add(node);

            Assert.AreEqual(1, graph.Children.Count);
        }

        [TestMethod]
        public void Children_render_in_the_order_they_were_added()
        {
            var graph = new Digraph { Name = "G" };
            foreach (var name in new[] { "z", "a", "m", "b" })
            {
                graph.Children.Add(new Node { Name = name });
            }

            var dot = graph.RenderDot().DotSource;

            Assert.IsLessThan(dot.IndexOf("\"a\""), dot.IndexOf("\"z\""));
            Assert.IsLessThan(dot.IndexOf("\"m\""), dot.IndexOf("\"a\""));
            Assert.IsLessThan(dot.IndexOf("\"b\""), dot.IndexOf("\"m\""));
        }

        [TestMethod]
        public void Node_line_width_renders_as_penwidth()
        {
            var node = new Node { Name = "n", LineWidth = 2.5 };

            StringAssert.Contains(Render(node), "penwidth=2.5");
        }

        [TestMethod]
        public void A_label_with_quotes_and_backslashes_stays_valid()
        {
            var node = new Node { Name = "n", Label = @"say ""hi"" C:\" };

            StringAssert.Contains(Render(node), @"label=""say \""hi\"" C:\\""");
        }

        [TestMethod]
        public void A_node_name_with_spaces_and_quotes_is_quoted()
        {
            var node = new Node { Name = "a \"b\"" };

            StringAssert.Contains(Render(node), "\"a \\\"b\\\"\";");
        }

        [TestMethod]
        public void Style_flags_combine_with_commas()
        {
            var node = new Node { Name = "n", Style = NodeStyle.Rounded | NodeStyle.Filled };

            StringAssert.Contains(Render(node), "style=\"filled,rounded\"");
        }

        [TestMethod]
        public void Svg_attributes_render_under_their_graphviz_names()
        {
            var node = new Node { Name = "n", SvgId = "t1", Class = "table", Tooltip = "Tip", Url = "#t1", Target = "_top", FontName = "Arial" };

            var dot = Render(node);

            StringAssert.Contains(dot, "id=\"t1\"");
            StringAssert.Contains(dot, "class=\"table\"");
            StringAssert.Contains(dot, "tooltip=\"Tip\"");
            StringAssert.Contains(dot, "URL=\"#t1\"");
            StringAssert.Contains(dot, "target=\"_top\"");
            StringAssert.Contains(dot, "fontname=\"Arial\"");
        }

        [TestMethod]
        public void Extra_attributes_are_quoted_and_override_properties()
        {
            var node = new Node { Name = "n", Shape = GraphvizLib.Shape.Box };
            node.Attributes["shape"] = "ellipse";
            node.Attributes["peripheries"] = "2";

            var dot = Render(node);

            StringAssert.Contains(dot, "shape=\"ellipse\"");
            StringAssert.Contains(dot, "peripheries=\"2\"");
            Assert.DoesNotContain("\"box\"", dot);
        }

        [TestMethod]
        public void Attributes_render_in_name_order()
        {
            var node = new Node { Name = "n", Width = 1, Color = GraphvizLib.Color.Red, Label = "x" };

            StringAssert.Contains(Render(node), "[color=\"red\", label=\"x\", width=1]");
        }

        [TestMethod]
        public void Node_and_edge_defaults_render_before_the_children()
        {
            var graph = new Digraph { Name = "G" };
            graph.NodeDefaults["fontname"] = "Arial";
            graph.EdgeDefaults["color"] = "gray";
            graph.Children.Add(new Node { Name = "a" });

            var dot = graph.RenderDot().DotSource;

            StringAssert.Contains(dot, "node [fontname=\"Arial\"];");
            StringAssert.Contains(dot, "edge [color=\"gray\"];");
            Assert.IsLessThan(dot.IndexOf("\"a\";"), dot.IndexOf("node ["));
        }

        [TestMethod]
        public void A_cluster_gets_the_cluster_prefix_and_keeps_its_name()
        {
            var graph = new Digraph { Name = "G" };
            var cluster = new Subgraph(graph) { Name = "group", Label = "Group" };
            cluster.Children.Add(new Node { Name = "a" });
            graph.Children.Add(cluster);

            var dot = graph.RenderDot().DotSource;

            StringAssert.Contains(dot, "subgraph \"cluster_group\" {");
            Assert.AreEqual("group", cluster.Name);
        }

        [TestMethod]
        public void A_plain_subgraph_has_no_cluster_prefix()
        {
            var graph = new Digraph { Name = "G" };
            graph.Children.Add(new Subgraph(graph) { Name = "rank", IsCluster = false });

            StringAssert.Contains(graph.RenderDot().DotSource, "subgraph \"rank\" {");
        }

        [TestMethod]
        public void Edge_ports_are_quoted()
        {
            var graph = new Digraph { Name = "G" };
            var a = new Node { Name = "a" };
            var b = new Node { Name = "b" };
            graph.Edges.Add(new Edge(a, b) { StartPortName = "field 1", EndPortName = "header", Direction = EdgeDirection.Back });

            StringAssert.Contains(graph.RenderDot().DotSource, "\"a\":\"field 1\" -> \"b\":\"header\" [dir=\"back\"];");
        }

        [TestMethod]
        public void An_html_label_is_not_quoted()
        {
            var html = new HtmlBuilder()
                .StartTable(BorderSize.Set(0))
                .StartRow().StartCell(PortName.Set("p")).AppendText("x").EndCell().EndRow()
                .EndTable();
            var node = new Node { Name = "n", Label = html };

            StringAssert.Contains(Render(node), "label=<<TABLE BORDER=\"0\"><TR><TD PORT=\"p\">x</TD></TR></TABLE>>");
        }

        [TestMethod]
        public void A_graph_needs_no_image_cache()
        {
            var graph = new Digraph();
            graph.Children.Add(new Node());

            Assert.IsNull(graph.RenderDot().ImageCache);
        }

        private static string Render(Node node)
        {
            var graph = new Digraph { Name = "G" };
            graph.Children.Add(node);
            return graph.RenderDot().DotSource;
        }
    }
}
