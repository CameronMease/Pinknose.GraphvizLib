/////////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright(c) 2022-2026 Cameron Mease
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
/////////////////////////////////////////////////////////////////////////////////

using Pinknose.GraphvizLib.Attributes;
using System;

namespace Pinknose.GraphvizLib
{
    /// <summary>
    /// See https://www.graphviz.org/docs/attr-types/arrowType/ for demos.
    /// </summary>
    public enum ArrowType
    {
        [DisplayValue("normal")]
        Normal,

        [DisplayValue("inv")]
        Inverted,

        [DisplayValue("dot")]
        Dot,

        [DisplayValue("invdot")]
        InvertedDot,

        [DisplayValue("odot")]
        OpenDot,

        [DisplayValue("invodot")]
        InvertedOpenDot,

        [DisplayValue("none")]
        None,

        [DisplayValue("tee")]
        Tee,

        [DisplayValue("empty")]
        EmptyArrow,

        [DisplayValue("invempty")]
        InvertedEmptyArrow,

        [DisplayValue("diamond")]
        Diamond,

        [DisplayValue("odiamond")]
        OpenDiamond,

        [DisplayValue("ediamond")]
        EmptyDiamond,

        [DisplayValue("crow")]
        Crow,

        [DisplayValue("box")]
        Box,

        [DisplayValue("obox")]
        OpenBox,

        [DisplayValue("open")]
        OpenArrow,

        [DisplayValue("halfopen")]
        HalfOpenArrow,

        [DisplayValue("vee")]
        Vee
    }

    /// <summary>
    /// See https://www.graphviz.org/docs/attr-types/style/ for demos.
    /// </summary>
    [Flags]
    public enum ClusterStyle
    {
        [DisplayValue("filled")]
        Filled = 1 << 0,

        [DisplayValue("striped")]
        Striped = 1 << 1,

        [DisplayValue("rounded")]
        Rounded = 1 << 2,
    }

    /// <summary>
    /// See https://www.graphviz.org/docs/attr-types/style/ for demos.
    /// </summary>
    [Flags]
    public enum EdgeStyle
    {
        [DisplayValue("dashed")]
        Dashed = 1 << 0,

        [DisplayValue("dotted")]
        Dotted = 1 << 1,

        [DisplayValue("solid")]
        Solid = 1 << 2,

        [DisplayValue("invis")]
        Invisible = 1 << 3,

        [DisplayValue("bold")]
        Bold = 1 << 4,

        [DisplayValue("tapered")]
        Tapered = 1 << 5,
    }

    /// <summary>
    /// See https://www.graphviz.org/docs/attr-types/style/ for demos.
    /// </summary>
    [Flags]
    public enum NodeStyle
    {
        [DisplayValue("dashed")]
        Dashed = 1 << 0,

        [DisplayValue("dotted")]
        Dotted = 1 << 1,

        [DisplayValue("solid")]
        Solid = 1 << 2,

        [DisplayValue("invis")]
        Invisible = 1 << 3,

        [DisplayValue("bold")]
        Bold = 1 << 4,

        [DisplayValue("filled")]
        Filled = 1 << 5,

        [DisplayValue("striped")]
        Striped = 1 << 6,

        [DisplayValue("wedged")]
        Wedged = 1 << 7,

        [DisplayValue("diagonals")]
        Diagonals = 1 << 8,

        [DisplayValue("rounded")]
        Rounded = 1 << 9,
    }

    /// <summary>
    /// See https://www.graphviz.org/docs/attrs/splines/
    /// </summary>
    public enum SplineType
    {
        [DisplayValue("line")]
        Line,

        [DisplayValue("spline")]
        Spline,

        [DisplayValue("polyline")]
        PolyLine,

        [DisplayValue("ortho")]
        Orthogonal,

        [DisplayValue("curve")]
        Curve,
    }

    /// <summary>
    /// Which ends of an edge get arrowheads. See https://graphviz.org/docs/attrs/dir/
    /// </summary>
    public enum EdgeDirection
    {
        [DisplayValue("forward")]
        Forward,

        [DisplayValue("back")]
        Back,

        [DisplayValue("both")]
        Both,

        [DisplayValue("none")]
        None
    }
}
