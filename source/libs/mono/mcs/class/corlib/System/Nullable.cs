//
// System.Nullable
//
// Martin Baulig (martin@ximian.com)
//
// (C) 2004 Novell, Inc.
//

//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Reflection;
#if NET_2_0
using System.Collections.Generic;
#endif
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

//#if NET_2_0
namespace System
{
#if NOT_PFX
#if NET_2_0
	[ComVisible (true)]
#endif
    public static class Nullable
    {
        public static int Compare<T>(Nullable<T> left, Nullable<T> right) where T : struct
        {
            IComparable icomparable = left.value as IComparable;
            if (icomparable == null)
                throw new ArgumentException("At least one object must implement IComparable.");
            if (left.has_value == false && right.has_value == false)
                return 0;
            if (!left.has_value)
                return -1;
            if (!right.has_value)
                return 1;

            return icomparable.CompareTo(right.value);
        }

        public static bool Equals<T>(Nullable<T> value1, Nullable<T> value2) where T : struct
        {
            return value1.Equals(value2);
        }

        public static Type GetUnderlyingType(Type nullableType)
        {
            if (nullableType == null)
                throw new ArgumentNullException("nullableType");
            if (nullableType.IsGenericType && nullableType.GetGenericTypeDefinition() == typeof(Nullable<>))
                return nullableType.GetGenericArguments()[0];
            else
                return null;
        }
    }
#endif

    [Serializable]
    public struct Nullable<T> where T : struct
    {
        #region Sync with runtime code
        internal T m_value;
        internal bool has_value;
        #endregion

        public Nullable(T value)
        {
            this.has_value = true;
            this.m_value = value;
        }

        public bool HasValue
        {
            get { return has_value; }
        }

        public T Value
        {
            get
            {
                if (!has_value)
                    throw new InvalidOperationException("Nullable object must have a value.");

                return m_value;
            }
        }

        public override bool Equals(object other)
        {
            if (other == null)
                return has_value == false;
            if (!(other is Nullable<T>))
                return false;

            return Equals((Nullable<T>)other);
        }

        bool Equals(Nullable<T> other)
        {
            Nullable<T> no = (Nullable<T>)other;
            if (no.has_value != has_value)
                return false;

            if (has_value == false)
                return true;

            return no.m_value.Equals(m_value);
        }

        public override int GetHashCode()
        {
            if (!has_value)
                return 0;

            return m_value.GetHashCode();
        }

        public T GetValueOrDefault()
        {
            return GetValueOrDefault(default(T));
        }

        public T GetValueOrDefault(T def_value)
        {
            if (!has_value)
                return def_value;
            else
                return m_value;
        }

        public override string ToString()
        {
            if (has_value)
                return m_value.ToString();
            return "";
        }

        public static implicit operator Nullable<T>(T value)
        {
            return new Nullable<T>(value);
        }

        public static explicit operator T(Nullable<T> value)
        {
            return value.Value;
        }

        // These are called by the JIT
        // Ironicly, the C#  code is the same for these two,
        // however, on the inside they do somewhat different things
        static object Box(T? o)
        {
            if (o == null)
                return null;
            return (T)o;
        }

        static T? Unbox(object o)
        {
            if (o == null)
                return null;
            return (T)o;
        }
    }
}
//#endif
