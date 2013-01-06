using System.Collections;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.FlashLand.Abc
{
    public sealed class AbcConstList<T> : IReadOnlyList<T>
        where T : class, IAbcConst
    {
        private readonly List<T> _list = new List<T>();
        private readonly Hashtable _index = new Hashtable();

        public int Count
        {
            get { return _list.Count; }
        }

        public T this[int index]
        {
            get { return _list[index]; }
        }

        public T this[string key]
        {
            get
            {
                int i = IndexOf(key);
                if (i >= 0)
                    return _list[i];
                return null;
            }
        }

        public void Add(T item)
        {
            int i = _list.Count;
            item.Index = i;
            _list.Add(item);
            _index[item.Key] = i;
        }

        public int IndexOf(string key)
        {
            var i = _index[key];
            if (i != null) return (int)i;
            return -1;
        }

        public bool IsDefined(T item)
        {
            if (item == null) return false;
            int i = item.Index;
            if (i < 0 || i >= _list.Count) return false;
            return _list[i] == item;
        }

	    public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

	    IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}