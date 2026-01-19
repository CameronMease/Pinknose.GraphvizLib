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
}