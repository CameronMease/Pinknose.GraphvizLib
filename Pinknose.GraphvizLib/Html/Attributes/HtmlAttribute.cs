using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinknose.GraphvizLib.Html.Attributes
{
    public interface IHtmlAttribute
    {
        public string Name { get; }
        public string Value { get; }
        public string ToString();
    }

    public interface ITableAttribute : IHtmlAttribute
    {

    }

    public interface ICellAttribute : IHtmlAttribute
    {

    }

    public interface IFontAttribute : IHtmlAttribute { }


    public abstract class HtmlAttribute : IHtmlAttribute
    {
        public abstract string Name { get; }

        public string Value { get; protected init; }

        public override string ToString() => $"{Name}=\"{Value}\"";

    }
}
