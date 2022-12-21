using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinknose.GraphvizLib.Html.Attributes
{
    public class Alignment : HtmlAttribute, ITableAttribute, ICellAttribute
    {
        public override string Name => "ALIGN";

        public static Alignment Center => new Alignment() { Value = "CENTER" };
        public static Alignment Left => new Alignment() { Value = "LEFT" };
        public static Alignment Right => new Alignment() { Value = "RIGHT" };
    }
}
