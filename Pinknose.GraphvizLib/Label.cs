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

using Pinknose.GraphvizLib.Html;

namespace Pinknose.GraphvizLib
{
    public class Label
    {
        #region Fields

        private string _text = string.Empty;

        #endregion Fields

        #region Constructors

        private Label()
        {
        }

        #endregion Constructors

        #region Properties

        public bool IsHtml { get; private set; } = false;

        #endregion Properties

        #region Methods

        public static implicit operator Label(string label) => new Label() { _text = label };

        public static implicit operator Label(HtmlBuilder builder) => new Label()
        {
            _text = builder.ToString(),
            IsHtml = true
        };

        public override int GetHashCode() => _text.GetHashCode();

        public override string ToString() => _text;

        #endregion Methods
    }
}