using System;
using System.Collections;
using System.Collections.Generic;

namespace DataDynamics.Collections
{
    public class FilteredEnumerator<T> : IEnumerator<T>, IEnumerable<T>
    {
        private readonly IEnumerator<T> _original;
        private readonly Predicate<T> _predicate;

        public FilteredEnumerator(IEnumerator<T> e, Predicate<T> p)
        {
            _original = e;
            _predicate = p;
        }

        #region IEnumerable<T> Members
        public IEnumerator GetEnumerator()
        {
            return this;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this;
        }
        #endregion

        #region IEnumerator<T> Members
        public void Dispose()
        {
            _original.Dispose();
        }

        public bool MoveNext()
        {
            while (_original.MoveNext())
            {
                if (_predicate(_original.Current))
                {
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            _original.Reset();
        }

        public T Current
        {
            get { return _original.Current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
        #endregion
    }

    public class FilteredEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _original;
        private readonly Predicate<T> _predicate;

        public FilteredEnumerable(IEnumerable<T> e, Predicate<T> p)
        {
            _original = e;
            _predicate = p;
        }

        #region IEnumerable<T> Members
        public IEnumerator<T> GetEnumerator()
        {
            return new FilteredEnumerator<T>(_original.GetEnumerator(), _predicate);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}