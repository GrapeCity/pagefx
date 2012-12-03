//CHANGED

//
// System.IntPtr.cs
//
// Author:
//   Miguel de Icaza (miguel@ximian.com)
//
// Maintainer:
//	 Michael Lambert, michaellambert@email.com
//
// (C) Ximian, Inc.  http://www.ximian.com
//
// Remarks:
//   Requires '/unsafe' compiler option.  This class uses void*,
//   in overloaded constructors, conversion, and cast members in 
//   the public interface.  Using pointers is not valid CLS and 
//   the methods in question have been marked with  the 
//   CLSCompliant attribute that avoid compiler warnings.
//
// FIXME: How do you specify a native int in C#?  I am going to have to do some figuring out
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

namespace System
{
    [Serializable]
    public struct IntPtr
    {
#if PTR32
        private int m_value;
#else
        private long m_value;
#endif

        public static readonly IntPtr Zero;

        public IntPtr(int i32)
        {
            m_value = i32;
        }

        public IntPtr(long i64)
        {
            if (((i64 > Int32.MaxValue) || (i64 < Int32.MinValue)) && (IntPtr.Size < 8))
            {
                throw new OverflowException(Locale.GetText("This isn't a 64bits machine."));
            }

#if PTR32
            m_value = (int)i64;
#else
            m_value = i64;
#endif
        }

        public static int Size
        {
            get { return 4; }
        }

        public override bool Equals(object o)
        {
            if (o is IntPtr)
                return ((IntPtr)o).m_value == m_value;
            return false;
        }

        public override int GetHashCode()
        {
            return (int)m_value;
        }

        public int ToInt32()
        {
            return (int)m_value;
        }

        public long ToInt64()
        {
            return m_value;
        }

        override public string ToString()
        {
            return m_value.ToString();
        }

        public static bool operator ==(IntPtr a, IntPtr b)
        {
            return (a.m_value == b.m_value);
        }

        public static bool operator !=(IntPtr a, IntPtr b)
        {
            return (a.m_value != b.m_value);
        }

        public static explicit operator IntPtr(int value)
        {
            return new IntPtr(value);
        }

        public static explicit operator IntPtr(long value)
        {
            return new IntPtr(value);
        }

        public static explicit operator int(IntPtr value)
        {
            return (int)value.m_value;
        }

        public static explicit operator long(IntPtr value)
        {
            return (long)value.m_value;
        }

        //unsafe public static explicit operator void*(IntPtr value)
        //{
        //    throw new NotImplementedException();
        //}

        //unsafe public static explicit operator IntPtr(void* ptr)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
