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

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pinknose.GraphvizLib
{
    /// <summary>
    /// The children or edges of a graph: kept in the order they were added, each at most once.
    /// </summary>
    /// <remarks>
    /// Order matters because DOT output order is layout input — <c>dot</c> places nodes partly by
    /// the order it reads them — so the same graph must always produce the same text. Identity is
    /// by reference: two nodes that happen to have the same attributes are still two nodes.
    /// </remarks>
    public sealed class ElementCollection<T> : ICollection<T>, IReadOnlyList<T>
        where T : class
    {
        #region Fields

        private readonly List<T> _items = new();
        private readonly HashSet<T> _members = new(ReferenceEqualityComparer.Instance);

        #endregion Fields

        #region Properties

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public T this[int index] => _items[index];

        #endregion Properties

        #region Methods

        /// <summary>Adds an item. Adding one already present does nothing.</summary>
        public void Add(T item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (_members.Add(item))
            {
                _items.Add(item);
            }
        }

        public void Clear()
        {
            _items.Clear();
            _members.Clear();
        }

        public bool Contains(T item) => item is not null && _members.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

        public bool Remove(T item)
        {
            if (item is null || !_members.Remove(item))
            {
                return false;
            }

            _items.Remove(item);
            return true;
        }

        #endregion Methods

        #region Classes

        private sealed class ReferenceEqualityComparer : IEqualityComparer<T>
        {
            public static readonly ReferenceEqualityComparer Instance = new();

            public bool Equals(T? x, T? y) => ReferenceEquals(x, y);

            public int GetHashCode(T obj) => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
        }

        #endregion Classes
    }
}
