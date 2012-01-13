namespace System.Collections.Generic
{
    public class ReadOnlyList<T> : IList<T>
    {
        protected readonly List<T> list = new List<T>();

        #region IList<T> Members
        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public T this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }
        #endregion

        #region ICollection<T> Members
        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }
        #endregion

        #region IEnumerable<T> Members
        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }
        #endregion

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
        #endregion
    }
}