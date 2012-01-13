using System.Collections;
using System.Collections.Generic;

namespace DataDynamics.Collections
{
    public interface ISimpleList<T> : IEnumerable<T>
    {
        int Count { get; }
        T this[int index] { get; }
    }

    public class SimpleList
    {
        private class ArrayAdapter<T> : ISimpleList<T>
        {
            public ArrayAdapter(T[] array)
            {
                _array = array;
            }
            private readonly T[] _array;

            #region ISimpleList<T> Members
            public int Count
            {
                get { return _array.Length; }
            }

            public T this[int index]
            {
                get { return _array[index]; }
            }
            #endregion

            #region IEnumerable<T> Members
            public IEnumerator<T> GetEnumerator()
            {
                return new ArrayEnumerator<T>(_array);
            }
            #endregion

            #region IEnumerable Members
            IEnumerator IEnumerable.GetEnumerator()
            {
                return _array.GetEnumerator();
            }
            #endregion
        }

        private class IListAdapter<T> : ISimpleList<T>
        {
            public IListAdapter(IList<T> list)
            {
                _list = list;
            }
            private readonly IList<T> _list;

            #region ISimpleList<T> Members
            public int Count
            {
                get { return _list.Count; }
            }

            public T this[int index]
            {
                get { return _list[index]; }
            }
            #endregion

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

        public static ISimpleList<T> FromArray<T>(T[] array)
        {
            return new ArrayAdapter<T>(array);
        }

        public static ISimpleList<T> FromList<T>(IList<T> list)
        {
            return new IListAdapter<T>(list);
        }
    }

    public class SimpleList<T> : List<T>, ISimpleList<T>
    {
    }

    public class ArrayEnumerator<T> : IEnumerator<T>
    {
        private readonly T[] _array;
        private int _index;

        public ArrayEnumerator(T[] array)
        {
            _array = array;
            _index = -1;
        }

        #region IEnumerator<T> Members
        public T Current
        {
            get { return _array[_index]; }
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
        }
        #endregion

        #region IEnumerator Members
        object IEnumerator.Current
        {
            get { return _array[_index]; }
        }

        public bool MoveNext()
        {
            ++_index;
            return _index < _array.Length;
        }

        public void Reset()
        {
            _index = -1;
        }
        #endregion
    }
}