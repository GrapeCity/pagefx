namespace System.Collections.Generic
{
	public class HashList<TKey, TValue> : IList<TValue>
    {
		private readonly Func<TValue, TKey> _keyProvider;
        private readonly Dictionary<TKey, int> _index;
        private readonly List<TValue> _list;

        public HashList(Func<TValue, TKey> keyProvider)
        {
            _index = new Dictionary<TKey, int>();
            _list = new List<TValue>();
            _keyProvider = keyProvider;
        }

        private TKey KeyOf(TValue value)
        {
            return _keyProvider(value);
        }

        #region IList<TValue> Members
        public int IndexOf(TValue item)
        {
            int i;
            if (_index.TryGetValue(KeyOf(item), out i))
                return i;
            return -1;
        }

        public void Insert(int index, TValue item)
        {
            _list.Insert(index, item);
            _index.Clear();
            int n = _list.Count;
            for (int i = 0; i < n; ++i)
            {
                var key = KeyOf(_list[i]);
                _index.Add(key, i);
            }
        }

        public void RemoveAt(int index)
        {
            var item = _list[index];
            _index.Remove(KeyOf(item));
            _list.RemoveAt(index);
        }

        public TValue this[int index]
        {
            get { return _list[index]; }
            set
            {
                if (!_list[index].Equals(value))
                {
                    var old = _list[index];
                    _index.Remove(KeyOf(old));
                    _list[index] = value;
                    _index.Add(KeyOf(value), index);
                }
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                int i;
                if (_index.TryGetValue(key, out i))
                    return _list[i];
                return default(TValue);
            }
        }
        #endregion

        #region ICollection<TValue> Members
        public void Add(TValue item)
        {
            var key = KeyOf(item);
            if (_index.ContainsKey(key))
                throw new ArgumentException();
            int i = _list.Count;
            _list.Add(item);
            _index.Add(key, i);
        }

        public void AddFast(TValue item)
        {
            var key = KeyOf(item);
            int i = _list.Count;
            _list.Add(item);
            _index.Add(key, i);
        }

        public void Clear()
        {
            _list.Clear();
            _index.Clear();
        }

        public bool Contains(TValue item)
        {
            var key = KeyOf(item);
            return _index.ContainsKey(key);
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            for (int i = 0; i < _list.Count && arrayIndex + i < array.Length; ++i)
            {
                array[arrayIndex + i] = _list[i];
            }
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(TValue item)
        {
            var key = KeyOf(item);
            int i;
            if (_index.TryGetValue(key, out i))
            {
                _list.RemoveAt(i);
                _index.Remove(key);
                return true;
            }
            return false;
        }
        #endregion

        #region IEnumerable<TValue> Members
        public IEnumerator<TValue> GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        #endregion

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        public void AddRange(IEnumerable<TValue> items)
        {
            foreach (var item in items)
                Add(item);
        }

        public void RemoveRange(IEnumerable<TValue> items)
        {
            foreach (var item in items)
                Remove(item);
        }

        public TValue[] ToArray()
        {
            return _list.ToArray();
        }

        public void Sort(IComparer<TValue> comparer)
        {
            int n = _list.Count;
            if (n <= 1) return;
            if (comparer == null)
                comparer = Comparer<TValue>.Default;
            _list.Sort(comparer);
            UpdateIndex();
        }

        public void Sort(Comparison<TValue> comparison)
        {
            int n = _list.Count;
            if (n <= 1) return;
            _list.Sort(comparison);
            UpdateIndex();
        }

        private void UpdateIndex()
        {
            int n = _list.Count;
            _index.Clear();
            for (int i = 0; i < n; ++i)
                _index.Add(KeyOf(_list[i]), i);
        }
    }
}