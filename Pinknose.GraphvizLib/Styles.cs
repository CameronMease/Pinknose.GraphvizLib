/////////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright(c) 2022 Cameron Mease
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

using Pinknose.GraphvizLib.Atttributes;

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
    public enum ClusterStyle
    {
        [DisplayValue("filled")]
        Filled,

        [DisplayValue("striped")]
        Striped,

        [DisplayValue("rounded")]
        Rounded
    }

    /// <summary>
    /// See https://www.graphviz.org/docs/attr-types/style/ for demos.
    /// </summary>
    public enum EdgeStyle
    {
        [DisplayValue("dashed")]
        Dashed,

        [DisplayValue("dotted")]
        Dotted,

        [DisplayValue("solid")]
        Solid,

        [DisplayValue("invis")]
        Invisible,

        [DisplayValue("bold")]
        Bold,

        [DisplayValue("tapered")]
        Tapered
    }

    /// <summary>
    /// See https://www.graphviz.org/docs/attr-types/style/ for demos.
    /// </summary>
    public enum NodeStyle
    {
        [DisplayValue("dashed")]
        Dashed,

        [DisplayValue("dotted")]
        Dotted,

        [DisplayValue("solid")]
        Solid,

        [DisplayValue("invis")]
        Invisible,

        [DisplayValue("bold")]
        Bold,

        [DisplayValue("filled")]
        Filled,

        [DisplayValue("striped")]
        Striped,

        [DisplayValue("wedged")]
        Wedged,

        [DisplayValue("diagonals")]
        Diagonals,

        [DisplayValue("rounded")]
        Rounded
    }
}