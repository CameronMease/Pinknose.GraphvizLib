using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinknose.GraphvizLib.Html.Attributes
{
    public class PortName : HtmlAttribute, ICellAttribute, ITableAttribute
    {
        public override string Name => "PORT";

        public static PortName Set(string name) => new PortName() { Value = name }; 
    }
}
