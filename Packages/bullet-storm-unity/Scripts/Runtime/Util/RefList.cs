using System;
using System.Collections.Generic;

namespace CANStudio.BulletStorm.Util
{
    public class RefList<T>
    {
        private T[] _items;

        public ref T this[int i] => ref _items[i];

        public int Count { get; private set; }

        public void Add(T item)
        {
            if (Count == _items.Length)
                EnsureCapacity(Count + 1);
            _items[Count++] = item;
        }

        public void AddRange(IEnumerable<T> collection)
        {
            InsertRange(Count, collection);
        }

        public void Clear()
        {
            Count = 0;
        }

        public void Insert(int index, T item)
        {
            if ((uint) index > (uint) Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (Count == _items.Length)
                EnsureCapacity(Count + 1);
            if (index < Count)
                Array.Copy(_items, index, _items, index + 1, Count - index);
            _items[index] = item;
            ++Count;
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if ((uint) index > (uint) Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (collection is ICollection<T> objs)
            {
                var count = objs.Count;
                if (count <= 0) return;
                EnsureCapacity(Count + count);
                if (index < Count)
                    Array.Copy(_items, index, _items, index + count, Count - index);
                var array = new T[count];
                objs.CopyTo(array, 0);
                array.CopyTo(_items, index);
                Count += count;
            }
            else
            {
                foreach (var obj in collection)
                    Insert(index++, obj);
            }
        }

        private void EnsureCapacity(int min)
        {
            if (_items.Length >= min)
                return;
            var num = _items.Length == 0 ? 4 : _items.Length * 2;
            if ((uint) num > 2146435071U)
                num = 2146435071;
            if (num < min)
                num = min;
            var array = new T[num];
            _items.CopyTo(array, 0);
            _items = array;
        }
    }
}