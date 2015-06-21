using System;
using System.Collections.Generic;
using System.Linq;

namespace MemoryGame.Common
{
    public class RoundRobin<TItem>
    {
        private readonly object _lock = new object();

        private readonly List<TItem> _items;
        private int _lastIndex = -1;

        public RoundRobin(IEnumerable<TItem> items)
        {
            _items = items.ToList();
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public void ForEach(Action<TItem> action)
        {
            _items.ForEach(action);
        }

        public TItem GetNext()
        {
            return _items[GetNextIndex()];
        }

        private int GetNextIndex()
        {
            lock (_lock)
            {
                var index = _lastIndex;
                index++;
                if (index >= _items.Count)
                {
                    index = 0;
                }
                _lastIndex = index;

                return index;
            }
        }
    }
}