using System.Collections;
using System.Collections.Generic;

namespace DataDynamics.Collections
{
    public class MergedEnumerator<T> : IEnumerator<T>, IEnumerable<T>
    {
        private readonly IEnumerator<T>[] _enums;
        private IEnumerator<T> _curenum;
        private int _index;

        public MergedEnumerator(params IEnumerable<T>[] args)
        {
            int n = args.Length;
            _enums = new IEnumerator<T>[n];
            for (int i = 0; i < n; ++i)
                _enums[i] = args[i].GetEnumerator();
        }

        #region IEnumerator<T> Members
        public T Current
        {
            get { return _curenum.Current; }
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
            get { return Current; }
        }

        public bool MoveNext()
        {
            if (_curenum == null)
            {
                _curenum = _enums[0];
            }

            while (true)
            {
                if (_curenum.MoveNext())
                    return true;

                ++_index;
                if (_index >= _enums.Length)
                    return false;

                _curenum = _enums[_index];
            }
        }

        public void Reset()
        {
            _curenum = null;
            _index = 0;
        }
        #endregion

        #region IEnumerable<T> Members
        public IEnumerator<T> GetEnumerator()
        {
            Reset();
            return this;
        }
        #endregion

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            Reset();
            return this;
        }
        #endregion
    }
}