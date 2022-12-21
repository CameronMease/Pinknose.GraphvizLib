using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinknose.GraphvizLib.Html.Attributes
{
    public class RowSpan : HtmlAttribute, ICellAttribute
    {
        public override string Name => "ROWSPAN";

        public static RowSpan Set(int span)
        {
            if (span < 1 || span > 65535)
            {
                throw new ArgumentOutOfRangeException(nameof(span));
            }

            return new RowSpan() { Value = span.ToString() };
        }
    }
}
