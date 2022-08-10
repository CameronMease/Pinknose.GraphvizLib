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
    /// See representation of shapes here: https://graphviz.org/doc/info/shapes.html
    /// </summary>
    public enum Shape
    {
        [DisplayValue("box")]
        Box,

        [DisplayValue("polygon")]
        Polygon,

        [DisplayValue("ellipse")]
        Ellipse,

        [DisplayValue("oval")]
        Oval,

        [DisplayValue("circle")]
        Circle,

        [DisplayValue("point")]
        Point,

        [DisplayValue("egg")]
        Egg,

        [DisplayValue("triangle")]
        Triangle,

        [DisplayValue("plaintext")]
        PlainText,

        [DisplayValue("plain")]
        Plain,

        [DisplayValue("diamond")]
        Diamond,

        [DisplayValue("trapezium")]
        Trapezium,

        [DisplayValue("parallelogram")]
        Parallelogram,

        [DisplayValue("house")]
        House,

        [DisplayValue("pentagon")]
        Pentagon,

        [DisplayValue("hexagon")]
        Hexagon,

        [DisplayValue("septagon")]
        Septagon,

        [DisplayValue("octagon")]
        Octagon,

        [DisplayValue("doublecircle")]
        DoubleCircle,

        [DisplayValue("doubleoctagon")]
        DoubleOctogon,

        [DisplayValue("tripleoctagon")]
        TripleOctogon,

        [DisplayValue("invtriangle")]
        InvertedTriangle,

        [DisplayValue("invtrapezium")]
        InvertedTrapezium,

        [DisplayValue("invhouse")]
        InvertedHouse,

        [DisplayValue("Mdiamond")]
        MDiamond,

        [DisplayValue("Msquare")]
        MSquare,

        [DisplayValue("Mcircle")]
        MCircle,

        [DisplayValue("rect")]
        Rectangle,

        [DisplayValue("square")]
        Square,

        [DisplayValue("star")]
        Star,

        [DisplayValue("none")]
        None,

        [DisplayValue("underline")]
        Underline,

        [DisplayValue("cylinder")]
        Cylinder,

        [DisplayValue("note")]
        Note,

        [DisplayValue("tab")]
        Tab,

        [DisplayValue("folder")]
        Folder,

        [DisplayValue("box3d")]
        Box3D,

        [DisplayValue("component")]
        Component,

        [DisplayValue("promoter")]
        Promoter,

        [DisplayValue("cds")]
        Cds,

        [DisplayValue("terminator")]
        Terminator,

        [DisplayValue("utr")]
        Utr,

        [DisplayValue("primersite")]
        PrimerSite,

        [DisplayValue("restrictionsite")]
        RestrictionSite,

        [DisplayValue("fivepoverhang")]
        FivePOverhang,

        [DisplayValue("threepoverhang")]
        ThreePOverhand,

        [DisplayValue("noverhang")]
        NoOverhand,

        [DisplayValue("assembly")]
        Assembly,

        [DisplayValue("signature")]
        Signature,

        [DisplayValue("insulator")]
        Insulator,

        [DisplayValue("ribosite")]
        RiboSite,

        [DisplayValue("rnastab")]
        RnaStab,

        [DisplayValue("proteasesite")]
        ProteaseSite,

        [DisplayValue("proteinstab")]
        ProteinStab,

        [DisplayValue("rpromoter")]
        RightPromoter,

        [DisplayValue("rarrow")]
        RightArrow,

        [DisplayValue("larrow")]
        LeftArrow,

        [DisplayValue("lpromoter")]
        LeftPromoter
    }
}