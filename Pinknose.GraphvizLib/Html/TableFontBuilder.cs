using Pinknose.GraphvizLib.Html.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinknose.GraphvizLib.Html
{
    public class TableFontBuilder<TParent> : HtmlBuilderBase where TParent : HtmlBuilderBase
    {
        private readonly TParent _parent;

        internal TableFontBuilder(TParent parent, params ICellAttribute[] attributes) : base()
        {
            _parent = parent;

            StringBuilder.Append($"<FONT ");

            foreach (var attribute in attributes)
            {
                StringBuilder.Append($" {attribute.ToString()}");
            }

            StringBuilder.Append(">");
        }
    }
}
