using System;
using System.Collections;
using System.Collections.Generic;

namespace DataDynamics.Collections
{
    public class ConvertedEnumerator<T1, T2> : IEnumerator<T2>
    {
        private readonly Converter<T1, T2> _converter;
        private readonly IEnumerator<T1> _original;

        public ConvertedEnumerator(IEnumerator<T1> e, Converter<T1, T2> c)
        {
            _original = e;
            _converter = c;
        }

        #region IEnumerator<T2> Members
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

        public T2 Current
        {
            get { return _converter(_original.Current); }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
        #endregion
    }

    public class ConvertedEnumerable<T1, T2> : IEnumerable<T2>
    {
        private readonly Converter<T1, T2> _converter;
        private readonly IEnumerable<T1> _original;

        public ConvertedEnumerable(IEnumerable<T1> original, Converter<T1, T2> c)
        {
            _original = original;
            _converter = c;
        }

        #region IEnumerable<T2> Members
        public IEnumerator<T2> GetEnumerator()
        {
            return new ConvertedEnumerator<T1, T2>(_original.GetEnumerator(), _converter);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }

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