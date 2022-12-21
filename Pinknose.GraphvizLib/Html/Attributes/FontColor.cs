using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinknose.GraphvizLib.Html.Attributes
{
    public class FontColor : HtmlAttribute, IFontAttribute
    {
        public override string Name => "COLOR";

        public static FontColor Set(Color color) 
        { 
            return new FontColor() { Value = color.GetDisplayValue() };
        }
    }
}
