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

using Pinknose.GraphvizLib.Attributes;
using System.Text;

namespace Pinknose.GraphvizLib
{
    public class Node : GraphvizElement
    {
        #region Properties

        [AttributeName("color")]
        public Color? Color { get; set; } = null;

        [AttributeName("fillcolor")]
        public Color? FillColor { get; set; } = null;

        [AttributeName("fixedsize")]
        public bool? FixedSize { get; set; } = null;

        [AttributeName("fontcolor")]
        public Color? FontColor { get; set; } = null;

        [AttributeName("c")]
        public double LineWidth { get; set; } = 1.0;

        [AttributeName("shape")]
        public Shape? Shape { get; set; } = null;

        [AttributeName("style")]
        public NodeStyle? Style { get; set; } = null;

        [AttributeName("width")]
        public double? Width { get; set; } = null;

        #endregion Properties

        #region Methods

        internal override string RenderDot(Graph graph, int indent)
        {
            string indentText = new string(' ', indent);

            StringBuilder sb = new StringBuilder();

            sb.Append($"{indentText}{this.Id} ");

            sb.AppendLine($"{this.RenderSingleLineAttributes()};");

            return sb.ToString();
        }

        

        #endregion Methods
    }
}