# Pinknose.GraphvizLib

Build [Graphviz](https://graphviz.org) diagrams from .NET: a typed object model that renders DOT,
including HTML-like labels, and an optional package that runs Graphviz to produce SVG and PNG.

| Package | What it does | Dependencies |
|---|---|---|
| `Pinknose.GraphvizLib` | Graphs, nodes, edges, subgraphs and HTML-like labels, rendered to DOT source | none |
| `Pinknose.GraphvizLib.Rendering` | Runs an installed Graphviz to render SVG, PNG or any other format; LaTeX math in labels | SkiaSharp, Svg, CSharpMath |

Both target `netstandard2.0`, `net8.0` and `net10.0`.

Use the core package alone when something else runs Graphviz — a browser build such as
`@hpcc-js/wasm-graphviz`, a server, or a build step. Add Rendering when this process should run it.

## Building DOT

```csharp
using Pinknose.GraphvizLib;
using Pinknose.GraphvizLib.Html;
using Pinknose.GraphvizLib.Html.Attributes;

var graph = new Digraph { Name = "schema", RankDirection = RankDirection.BottomToTop };
graph.NodeDefaults["fontname"] = "Arial";

var parts = new Node { Name = "parts", Label = "Parts", Shape = Shape.Box };

var tasks = new Node
{
    Name = "tasks",
    Shape = Shape.PlainText,
    SvgId = "table-tasks",       // the node's id in SVG output
    Tooltip = "Work to be done",
    Label = new HtmlBuilder()
        .StartTable(BorderSize.Set(0), CellBorder.Set(1), CellSpacing.Set(0))
            .StartRow().StartCell(PortName.Set("header")).AppendText("Tasks", HtmlTextFormat.Bold).EndCell().EndRow()
            .StartRow().StartCell(PortName.Set("part")).AppendText("Part → Parts").EndCell().EndRow()
        .EndTable(),
};

graph.Children.Add(tasks);
graph.Children.Add(parts);
graph.Edges.Add(new Edge(tasks, parts) { StartPortName = "part", ArrowHead = ArrowType.Diamond });

string dot = graph.RenderDot().DotSource;
```

- **Text is literal.** A string assigned to a label, tooltip or name appears exactly as written;
  quotes, backslashes and line breaks are escaped. Use `Label.FromEscString(...)` for Graphviz
  escapes such as `\l`.
- **Output is deterministic.** Children and edges render in the order added, and attributes in
  name order, so the same graph always produces the same DOT.
- **Anything without a property** goes in `Attributes` (per element) or `NodeDefaults` /
  `EdgeDefaults` (per graph), by its Graphviz name.

## Rendering

Requires Graphviz installed, with `dot` on the `PATH` (or set `DotExe.ExecutablePath`).

```csharp
using Pinknose.GraphvizLib;

string svg = await graph.RenderSvgStringAsync();
await graph.RenderToPngFileAsync("schema.png");
byte[] pdf = await DotExe.RenderAsync(graph, "pdf", GraphvizEngine.Dot);
```

A Graphviz error throws `GraphvizException`, carrying Graphviz's own message and exit code.

## Changes in 2.0

2.0 is a breaking release.

- **Split into two packages.** `DotExe`, the `Render…Async` methods, `GetBytes` and LaTeX moved to
  `Pinknose.GraphvizLib.Rendering`. The render methods are now extension methods in the same
  namespace, so call sites compile once the package is referenced.
- **Render methods** take the file name before the engine, the engine is optional (default
  `Dot`), and all take a `CancellationToken`. The engine argument was previously ignored.
- **`GraphvizElement.Id` is now `Name`**, and it is settable. The Graphviz `id` attribute is
  `SvgId`.
- **Plain strings are literal text.** Previously they were inserted between quotes unescaped.
- **Elements compare by reference.** Previously two nodes with the same attributes were equal,
  and one was dropped from a graph.
- **`Children` and `Edges` are `ElementCollection<T>`**, ordered and without duplicates.
  `ExtendedList<T>` and its `AddingItem` event are gone.
- **Style enums are flags**: `NodeStyle.Rounded | NodeStyle.Filled`.
- **`Node.LineWidth`** now renders as `penwidth`; it previously rendered as an attribute named `c`.
- **`HtmlImageCache` is optional**, and loads its icon lazily. Graphs no longer need one.
- **`TableCellBuilder.AppendCell`** is removed (it lost the current cell's content), and
  `AppendLatexImage` is an extension method in the Rendering package.
- **Target frameworks** are `netstandard2.0`, `net8.0` and `net10.0`. .NET Framework 4.6.2 and
  later use the netstandard build.

## Building and testing

```
dotnet build Pinknose.GraphvizLib.sln
dotnet test Pinknose.GraphvizLib.sln
```

Tests that run Graphviz are reported inconclusive when it is not installed.
