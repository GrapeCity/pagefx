using System.Collections;
using System.Collections.Generic;

namespace DataDynamics.PageFX.FLI.ABC
{
    public class AbcConstList<T> : IEnumerable<T>
        where T : class, IAbcConst
    {
        readonly List<T> _list = new List<T>();
        readonly Hashtable _index = new Hashtable();

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

        #region IEnumerable<T> Members
        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        #endregion

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        #endregion
    }
}