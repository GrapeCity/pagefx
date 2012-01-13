using System;
using System.Collections;
using System.Collections.Generic;

namespace DataDynamics.Collections
{
    public class EmptyEnumerable<T> : IEnumerable<T>
    {
        public static readonly IEnumerable<T> Instance = new EmptyEnumerable<T>();

        public IEnumerator<T> GetEnumerator()
        {
            return new EmptyEnumerator<T>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class EmptyEnumerator<T> : IEnumerator<T>, IEnumerable<T>
    {
        public static readonly IEnumerator<T> Instance = new EmptyEnumerator<T>();

        public void Dispose()
        {
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            return false;
        }

        public void Reset()
        {
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public T Current
        {
            get { throw new InvalidOperationException(); }
        }

        object IEnumerator.Current
        {
            get { throw new InvalidOperationException(); }
        }
    }

    public class EmptyEnumerator : IEnumerator
    {
        public static readonly EmptyEnumerator Instance = new EmptyEnumerator();

        public bool MoveNext()
        {
            return false;
        }

        public void Reset()
        {
        }

        public object Current
        {
            get { throw new InvalidOperationException(); }
        }
    }
}