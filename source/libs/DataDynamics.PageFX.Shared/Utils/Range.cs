using System;
using System.Collections.Generic;

namespace DataDynamics
{
    public interface IRange<T>
    {
        bool Contains(T value);
    }

    public static class Range
    {
        public sealed class GT<T> : IRange<T> where T : IComparable<T>
        {
            public GT(T value)
            {
                _value = value;
            }

            public T Value
            {
                get { return _value; }
                set { _value = value; }
            }
            private T _value;

            #region IRange<T> Members
            public bool Contains(T value)
            {
                return value.CompareTo(_value) > 0;
            }
            #endregion
        }

        public sealed class GE<T> : IRange<T> where T : IComparable<T>
        {
            public GE(T value)
            {
                _value = value;
            }

            public T Value
            {
                get { return _value; }
                set { _value = value; }
            }
            private T _value;

            #region IRange<T> Members
            public bool Contains(T value)
            {
                return value.CompareTo(_value) >= 0;
            }
            #endregion
        }

        public sealed class LT<T> : IRange<T> where T : IComparable<T>
        {
            public LT(T value)
            {
                _value = value;
            }

            public T Value
            {
                get { return _value; }
                set { _value = value; }
            }
            private T _value;

            #region IRange<T> Members
            public bool Contains(T value)
            {
                return value.CompareTo(_value) < 0;
            }
            #endregion
        }

        public sealed class LE<T> : IRange<T> where T : IComparable<T>
        {
            public LE(T value)
            {
                _value = value;
            }

            public T Value
            {
                get { return _value; }
                set { _value = value; }
            }
            private T _value;

            #region IRange<T> Members
            public bool Contains(T value)
            {
                return value.CompareTo(_value) <= 0;
            }
            #endregion
        }

        public sealed class AND<T> : IRange<T>
        {
            public List<IRange<T>> Ranges
            {
                get { return _ranges; }
            }
            private readonly List<IRange<T>> _ranges = new List<IRange<T>>();

            #region IRange<T> Members
            public bool Contains(T value)
            {
                int n = _ranges.Count;
                for (int i = 0; i < n; ++i)
                {
                    if (!_ranges[i].Contains(value))
                        return false;
                }
                return true;
            }
            #endregion
        }

        public sealed class OR<T> : IRange<T>
        {
            public List<IRange<T>> Ranges
            {
                get { return _ranges; }
            }
            private readonly List<IRange<T>> _ranges = new List<IRange<T>>();

            #region IRange<T> Members
            public bool Contains(T value)
            {
                int n = _ranges.Count;
                for (int i = 0; i < n; ++i)
                {
                    if (_ranges[i].Contains(value))
                        return true;
                }
                return false;
            }
            #endregion
        }
    }

    public class Ranges<T> where T : IComparable<T>
    {
        public static readonly IRange<T> Positive = new _Positive();
        public static readonly IRange<T> Negative = new _Negative();
        public static readonly IRange<T> NonPositive = new _NonPositive();
        public static readonly IRange<T> NonNegative = new _NonNegative();

        private class _Positive : IRange<T>
        {
            #region IRange<T> Members
            public bool Contains(T value)
            {
                return value.CompareTo(default(T)) > 0;
            }
            #endregion
        }

        private class _Negative : IRange<T>
        {
            #region IRange<T> Members
            public bool Contains(T value)
            {
                return value.CompareTo(default(T)) < 0;
            }
            #endregion
        }

        private class _NonPositive : IRange<T>
        {
            #region IRange<T> Members
            public bool Contains(T value)
            {
                return value.CompareTo(default(T)) <= 0;
            }
            #endregion
        }

        private class _NonNegative : IRange<T>
        {
            #region IRange<T> Members
            public bool Contains(T value)
            {
                return value.CompareTo(default(T)) >= 0;
            }
            #endregion
        }
    }
}