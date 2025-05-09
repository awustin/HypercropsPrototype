using System.Collections.Generic;

namespace Assets.Hypercrops.Events
{
    public class IterativeValidator
    {
        private readonly List<object> _items;
        private int _index;
        private bool _valid;

        public IterativeValidator(List<object> items)
        {
            _items = items;
            _index = 0;
            _valid = true;
        }

        public IterativeValidator N(int count)
        {
            _valid = _items.Count == count;

            return this;
        }

        public IterativeValidator T<T>(out T item)
        {
            if (_index >= _items.Count)
            {
                item = default;
                return this;
            }

            _valid = _items[_index] is T;
            item = _valid ? (T) _items[_index] : default;

            _index ++;

            return this;
        }

        public bool IsValid()
        {
            return _valid;
        }
    }
}
