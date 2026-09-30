/////////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright(c) 2022-2026 Cameron Mease
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

using Pinknose.GraphvizLib.Html.Attributes;

namespace Pinknose.GraphvizLib.Html
{
    public class TableRowBuilder<TParent> : HtmlBuilderBase where TParent : HtmlBuilderBase
    {
        #region Fields

        private readonly TParent _parent;

        #endregion Fields

        #region Constructors

        internal TableRowBuilder(TParent parent)
        {
            _parent = parent;
            StringBuilder.Append("<TR>");
        }

        #endregion Constructors

        #region Methods

        /// <summary>A vertical rule between two cells (<c>&lt;VR/&gt;</c>).</summary>
        public TableRowBuilder<TParent> AppendVerticalRule()
        {
            StringBuilder.Append("<VR/>");
            return this;
        }

        public TParent EndRow()
        {
            StringBuilder.Append("</TR>");
            _parent.AppendRawString(StringBuilder.ToString());
            return _parent;
        }

        public TableCellBuilder<TableRowBuilder<TParent>> StartCell(params ICellAttribute[] attributes) => new(this, attributes);

        #endregion Methods
    }
}
