using System;
using System.Collections;
using System.Collections.Generic;

namespace Pinknose.GraphvizLib
{
    public class AddingItemEventArgs<T> : EventArgs
    {
        #region Constructors

        public AddingItemEventArgs(T item) : base()
        {
            Item = item;
        }

        #endregion Constructors

        #region Properties

        public T Item { get; }

        #endregion Properties
    }

    public class ExtendedList<T> : ICollection<T>
    {
        #region Fields

        private readonly HashSet<T> _list = new();

        #endregion Fields

        #region Events

        public event EventHandler<AddingItemEventArgs<T>>? AddingItem;

        #endregion Events

        //public T this[int index] { get => _list[index]; set => _list[index] = value; }

        #region Properties

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        #endregion Properties

        #region Methods

        public void Add(T item)
        {
            AddingItem?.Invoke(this, new AddingItemEventArgs<T>(item));

            _list.Add(item);
        }

        public void Clear() => _list.Clear();

        public bool Contains(T item) => _list.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        //public int IndexOf(T item) => _list.IndexOf(item);

        //public void Insert(int index, T item) => _list.Insert(index, item);

        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

        public bool Remove(T item) => _list.Remove(item);

        #endregion Methods

        //public void RemoveAt(int index) => _list.RemoveAt(index);
    }
}