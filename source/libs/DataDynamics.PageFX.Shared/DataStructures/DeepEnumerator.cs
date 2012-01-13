using System;
using System.Collections;
using System.Collections.Generic;

namespace DataDynamics.Collections
{
    public class DeepEnumerator<T> : IEnumerator<T>, IEnumerable<T>
    {
        #region ctor
        private readonly IEnumerator<T> _enum;

        private static void flatten(T root, T cur, Converter<T, IEnumerable<T>> kids, List<T> flat, bool includeRoot)
        {
            foreach (var kid in kids(cur))
            {
                flatten(root, kid, kids, flat, includeRoot);
            }
            if (includeRoot)
            {
                flat.Add(cur);
            }
            else
            {
                if (!Equals(cur, root))
                    flat.Add(cur);
            }
        }

        public DeepEnumerator(T root, Converter<T, IEnumerable<T>> kids, bool includeRoot, bool firstParent)
        {
            var flat = new List<T>();
            if (firstParent)
            {
                int i = 0;
                var parent = root;
                if (includeRoot)
                {
                    flat.Add(root);
                    ++i;
                }
                foreach (var kid in kids(parent))
                {
                    flat.Add(kid);
                }
                while (i < flat.Count)
                {
                    parent = flat[i];
                    foreach (var kid in kids(parent))
                    {
                        flat.Add(kid);
                    }
                    ++i;
                }
            }
            else
            {
                flatten(root, root, kids, flat, includeRoot);
            }
            _enum = flat.GetEnumerator();
        }
        #endregion

        #region IEnumerator<T> Members
        public T Current
        {
            get { return _enum.Current; }
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
            return _enum.MoveNext();
        }

        public void Reset()
        {
            _enum.Reset();
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