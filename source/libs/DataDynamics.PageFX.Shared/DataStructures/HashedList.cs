namespace System.Collections.Generic
{
    #region class HashedList
    public class HashedList<TKey, TValue> : IList<TValue>
    {
        private readonly IKeyProvider<TKey, TValue> _key;
        private readonly Dictionary<TKey, int> _index;
        private readonly List<TValue> _list;

        #region Constructors
        public HashedList(IKeyProvider<TKey, TValue> keyProvider)
        {
            _index = new Dictionary<TKey, int>();
            _list = new List<TValue>();
            _key = keyProvider;
        }

        public HashedList(KeyFunction<TKey, TValue> f)
            : this(new FunctorKeyProvider<TKey, TValue>(f))
        {
        }
        #endregion

        private TKey KeyOf(TValue value)
        {
            return _key.KeyOf(value);
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
    #endregion

    #region class SimpleHashedList
    public class SimpleHashedList<TKey, TValue> : IList<TValue>
    {
        private readonly IKeyProvider<TKey, TValue> _key;
        private readonly Dictionary<TKey, TValue> _dic;
        private readonly List<TValue> _list;

        #region Constructors
        public SimpleHashedList(IKeyProvider<TKey, TValue> keyProvider)
        {
            _dic = new Dictionary<TKey, TValue>();
            _list = new List<TValue>();
            _key = keyProvider;
        }

        public SimpleHashedList(KeyFunction<TKey, TValue> f)
            : this(new FunctorKeyProvider<TKey, TValue>(f))
        {
        }
        #endregion

        private TKey KeyOf(TValue value)
        {
            return _key.KeyOf(value);
        }

        #region IList<TValue> Members
        public int IndexOf(TValue item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, TValue item)
        {
            var key = KeyOf(item);
            if (_dic.ContainsKey(key))
                throw new InvalidOperationException();
            _list.Insert(index, item);
            _dic.Add(key, item);
        }

        public void RemoveAt(int index)
        {
            var item = _list[index];
            _dic.Remove(KeyOf(item));
            _list.RemoveAt(index);
        }

        public TValue this[int index]
        {
            get { return _list[index]; }
            set
            {
                if (!Equals(_list[index], value))
                {
                    var old = _list[index];
                    _dic.Remove(KeyOf(old));
                    _list[index] = value;
                    _dic.Add(KeyOf(value), value);
                }
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue v;
                if (_dic.TryGetValue(key, out v))
                    return v;
                return default(TValue);
            }
        }
        #endregion

        #region ICollection<TValue> Members
        public void Add(TValue item)
        {
            var key = KeyOf(item);
            if (_dic.ContainsKey(key))
                throw new ArgumentException();
            _list.Add(item);
            _dic.Add(key, item);
        }

        public void Clear()
        {
            _list.Clear();
            _dic.Clear();
        }

        public bool Contains(TValue item)
        {
            var key = KeyOf(item);
            return _dic.ContainsKey(key);
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
            int i = IndexOf(item);
            if (i < 0) return false;
            var key = KeyOf(item);
            _list.RemoveAt(i);
            _dic.Remove(key);
            return true;
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
        }

        public void Sort(Comparison<TValue> comparison)
        {
            int n = _list.Count;
            if (n <= 1) return;
            _list.Sort(comparison);
        }
    }
    #endregion

}