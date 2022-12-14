using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinknose.GraphvizLib
{
    public class AddingItemEventArgs<T> : EventArgs
    {
        public AddingItemEventArgs(T item) : base()
        {
            Item = item;
        }

        public T Item { get; }
    }


    public class ExtendedList<T> : IList<T>
    {
        public event EventHandler<AddingItemEventArgs<T>> AddingItem;

        private List<T> _list = new List<T>();

        public T this[int index] { get => _list[index]; set => _list[index] = value; }

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public void Add(T item) 
        {
            AddingItem?.Invoke(this, new AddingItemEventArgs<T>(item));

            _list.Add(item);
        }

        public void Clear() => _list.Clear();

        public bool Contains(T item) => _list.Contains (item);
        
        public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);
        
        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
        
        public int IndexOf(T item) => _list.IndexOf(item);
        
        public void Insert(int index, T item) => _list.Insert(index, item);
        
        public bool Remove(T item) => _list.Remove(item);
        
        public void RemoveAt(int index) => _list.RemoveAt(index);
        
        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
        
    }
}
