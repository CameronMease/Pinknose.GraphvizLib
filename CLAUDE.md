# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build Commands
- Build: `dotnet build Pinknose.GraphvizLib.sln`
- Clean: `dotnet clean Pinknose.GraphvizLib.sln`
- Restore packages: `dotnet restore Pinknose.GraphvizLib.sln`

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