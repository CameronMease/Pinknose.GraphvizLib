using Pinknose.GraphvizLib.Atttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinknose.GraphvizLib
{
    public enum PortPosition
    {
        [DisplayValue("n")]
        North,

        [DisplayValue("ne")]
        NorthEast,

        [DisplayValue("e")]
        East,

        [DisplayValue("se")]
        SouthEast,

        [DisplayValue("s")]
        South,

        [DisplayValue("sw")]
        SouthWest,

        [DisplayValue("w")]
        West,

        [DisplayValue("nw")]
        NorthWest,

        [DisplayValue("c")]
        Center,

        [DisplayValue("_")]
        Default
    }
}
