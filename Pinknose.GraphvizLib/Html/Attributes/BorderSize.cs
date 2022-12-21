using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinknose.GraphvizLib.Html.Attributes
{
    public class BorderSize : HtmlAttribute, ITableAttribute, ICellAttribute
    {
        public override string Name => "BORDER";

        public static BorderSize Set(int size)
        {
            if (size <0 || size > 127)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            return new BorderSize() { Value = size.ToString() };
        }
    }
}
