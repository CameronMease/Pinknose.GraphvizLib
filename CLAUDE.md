# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build Commands
- Build: `dotnet build Pinknose.GraphvizLib.sln`
- Test: `dotnet test Pinknose.GraphvizLib.sln` (rendering tests need Graphviz's `dot` on the PATH; without it they are inconclusive)
- Pack: `dotnet pack Pinknose.GraphvizLib.sln -c Release`
- Clean: `dotnet clean Pinknose.GraphvizLib.sln`

## Structure
- `Pinknose.GraphvizLib` — graph model, DOT output, HTML-like labels. Must stay free of package dependencies: consumers such as an Excel-DNA add-in pack it into a single file, and a native dependency breaks that.
- `Pinknose.GraphvizLib.Rendering` — everything that runs Graphviz or needs SkiaSharp/Svg/CSharpMath.
- `Pinknose.GraphvizLib.Tests` — MSTest. DOT output is asserted as text; rendering tests run the real `dot`.
- Version and package metadata live in `Directory.Build.props`.

## Rules for DOT output
- Every quoted value goes through `DotString` (or `SharedFormatting.EscapeHtml` inside HTML-like labels). Never interpolate user text into DOT unescaped.
- Output must be deterministic: collections keep insertion order and attributes render sorted by name.
- Elements compare by reference.

## Code Style Guidelines
- Use nullable reference types and explicit nullability annotations
- Use regions for organizing code sections (#region Properties, #region Methods, etc.)
- Order: Constructors, Properties, Methods
- Prefer async/await for asynchronous operations
- Use PascalCase for public members and camelCase for parameters
- Place attributes on their own line above the member declaration
- Use XML documentation comments for public API
- Use 4-space indentation
- Include MIT license header at the top of each file
- Use explicit access modifiers (public, private, etc.)