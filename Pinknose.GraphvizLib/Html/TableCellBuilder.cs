/////////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright(c) 2022-2023 Cameron Mease
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
using System;
using System.IO;
using System.Text;

namespace Pinknose.GraphvizLib.Html
{
    public class TableCellBuilder<TParent> : HtmlBuilderBase where TParent : HtmlBuilderBase
    {
        #region Fields

        private static string? IconsPath = null;

        private readonly TParent _parent;

        #endregion Fields

        #region Constructors

        internal TableCellBuilder(TParent parent, params ICellAttribute[] attributes) : base()
        {
            _parent = parent;

            StringBuilder.Append($"<TD ");

            foreach (var attribute in attributes)
            {
                StringBuilder.Append($" {attribute.ToString()}");
            }

            StringBuilder.Append('>');
        }

        #endregion Constructors

        #region Methods

        public TableCellBuilder<TParent> AppendCell(params ICellAttribute[] attributes) =>
            new(_parent, attributes);

        public TableCellBuilder<TParent> AppendImage(string filename)
        {
            StringBuilder.Append(SharedFormatting.FormatImage(filename));
            return this;
        }

        public TableCellBuilder<TParent> AppendLatexImage(string latex)
        {
            StringBuilder.Append(SharedFormatting.FormatLatex(latex));
            return this;
        }

        public TableCellBuilder<TParent> AppendLineBreak()
        {
            StringBuilder.Append(SharedFormatting.FormatLineBreak());
            return this;
        }

        public TableCellBuilder<TParent> AppendText(string text, HtmlTextFormat format = HtmlTextFormat.None)
        {
            StringBuilder.Append(SharedFormatting.FormatText(text, format));
            return this;
        }

        public TableCellBuilder<TParent> AppendWarningIcon() => AppendIcon("StatusWarning.png");

        public TParent EndCell()
        {
            StringBuilder.Append("</TD>");
            _parent.AppendRawString(StringBuilder.ToString());
            return _parent;
        }

        private TableCellBuilder<TParent> AppendIcon(string filename)
        {
            var assemblyLocation = Path.GetDirectoryName(this.GetType().Assembly.Location) ?? throw new ArgumentNullException(nameof(Type.Assembly.Location));

            IconsPath ??= Path.Combine(assemblyLocation, "Html", "Icons");

            var fullPath = Path.Combine(IconsPath, filename);

            AppendImage(fullPath);

            return this;
        }

        #endregion Methods
    }
}