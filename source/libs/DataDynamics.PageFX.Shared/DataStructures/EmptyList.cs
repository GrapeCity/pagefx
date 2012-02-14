using System.Linq;

namespace System.Collections.Generic
{
    public class EmptyList<T> : IList<T>
    {
        #region IList<T> Members
        public int IndexOf(T item)
        {
            return -1;
        }

        public void Insert(int index, T item)
        {
        }

        public void RemoveAt(int index)
        {
        }

        public T this[int index]
        {
            get { return default(T); }
            set { }
        }
        #endregion

        #region ICollection<T> Members
        public void Add(T item)
        {
        }

        public void Clear()
        {
        }

        public bool Contains(T item)
        {
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
        }

        public int Count
        {
            get { return 0; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return false;
        }
        #endregion

        #region IEnumerable<T> Members
        public IEnumerator<T> GetEnumerator()
        {
            return Enumerable.Empty<T>().GetEnumerator();
        }
        #endregion

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}