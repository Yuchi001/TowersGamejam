using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class CircularEnumerable<T>
    {
        private readonly List<T> _items; 
        private int _index = -1;

        public int Index => _index % _items.Count;
        public int NextIndex => (_index + 1) % _items.Count;
        public int LastIndex => (_items.Count + _index-1) % _items.Count;
        public int Count => _items.Count;

        public CircularEnumerable(IEnumerable<T> items)
        {
            _items = new List<T>(items);
        }

        public CircularEnumerable()
        {
            _items = new List<T>();
        }

        public void Reset()
        {
            _index = -1;
        }

        public bool CanCycle()
        {
            return _items.Count > 0;
        }
        
        public T Next() {
            _index = (_index + 1) % _items.Count;

            return _items[_index];
        }

        public T Current()
        {
            _index %= _items.Count;
            _index = Mathf.Max(_index, 0);
            return _items[_index];
        }
        
        public T Previous() {
            _index = (_items.Count + _index-1) % _items.Count;

            return _items[_index];
        }

        public void ForEach(Action<T> action) => _items.ForEach(action);
    }
}