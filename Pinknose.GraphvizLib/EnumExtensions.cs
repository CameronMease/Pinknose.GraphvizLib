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

using Pinknose.GraphvizLib.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pinknose.GraphvizLib
{
    public static class EnumExtensions
    {
        #region Methods

        /// <summary>
        /// Gets the DOT value for an enumeration value: its <c>DisplayValue</c>, or its name if it
        /// has none. For a <see cref="FlagsAttribute"/> enumeration, the values of every set flag,
        /// comma-separated, which is how DOT combines styles such as <c>rounded,filled</c>.
        /// </summary>
        public static string GetDisplayValue(this Enum theEnum)
        {
            if (theEnum is null)
            {
                throw new ArgumentNullException(nameof(theEnum));
            }

            var enumType = theEnum.GetType();

            if (enumType.GetCustomAttributes(typeof(FlagsAttribute), false).Length == 0)
            {
                return DisplayValueOf(enumType, theEnum.ToString());
            }

            var bits = Convert.ToUInt64(theEnum, System.Globalization.CultureInfo.InvariantCulture);
            var names = new List<string>();

            foreach (Enum flag in Enum.GetValues(enumType))
            {
                var flagBits = Convert.ToUInt64(flag, System.Globalization.CultureInfo.InvariantCulture);

                if (flagBits != 0 && (bits & flagBits) == flagBits)
                {
                    names.Add(DisplayValueOf(enumType, flag.ToString()));
                }
            }

            return string.Join(",", names);
        }

        private static string DisplayValueOf(Type enumType, string memberName)
        {
            var member = enumType
                .GetMember(memberName)
                .FirstOrDefault(m => m.DeclaringType == enumType);

            var attribute = member?.GetCustomAttribute<DisplayValueAttribute>(false);

            return attribute?.DisplayValue ?? memberName;
        }

        #endregion Methods
    }
}
