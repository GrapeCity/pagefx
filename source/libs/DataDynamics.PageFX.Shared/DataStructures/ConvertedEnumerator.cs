using System.Collections;
using System.Collections.Generic;

namespace DataDynamics.Collections
{
	public class BaseTypeEnumerator<T, B> : IEnumerator<B> where T : B
    {
        private readonly IEnumerator<T> _original;

        public BaseTypeEnumerator(IEnumerator<T> e)
        {
            _original = e;
        }

        public BaseTypeEnumerator(IEnumerable<T> e)
        {
            _original = e.GetEnumerator();
        }

        #region IEnumerator<B> Members
        public void Dispose()
        {
            _original.Dispose();
        }

        public bool MoveNext()
        {
            return _original.MoveNext();
        }

        public void Reset()
        {
            _original.Reset();
        }

        public B Current
        {
            get { return _original.Current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
        #endregion
    }

    public class BaseTypeEnumerable<T, B> : IEnumerable<B> where T : B
    {
        private readonly IEnumerable<T> _original;

        public BaseTypeEnumerable(IEnumerable<T> original)
        {
            _original = original;
        }

        #region IEnumerable<B> Members
        public IEnumerator<B> GetEnumerator()
        {
            return new BaseTypeEnumerator<T, B>(_original.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}