//CHANGED

//
// System.UIntPtr.cs
//
// Author:
//   Michael Lambert (michaellambert@email.com)
//
// (C) 2001 Michael Lambert, All Rights Reserved
//
// Remarks:
// Requires '/unsafe' compiler option.  This class uses void*,
// ulong, and uint in overloaded constructors, conversion, and
// cast members in the public interface.  Using pointers is not
// valid CLS and the methods in question have been marked with
// the CLSCompliant attribute that avoid compiler warnings.

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
    public struct UIntPtr
    {
        public static readonly UIntPtr Zero = new UIntPtr(0u);

#if PTR32
        internal uint m_value;
#else
        internal ulong m_value;
#endif

        public UIntPtr(ulong value)
        {
            if ((value > UInt32.MaxValue) && (UIntPtr.Size < 8))
            {
                throw new OverflowException(Locale.GetText("This isn't a 64bits machine."));
            }

#if PTR32
            m_value = (uint)value;
#else
            m_value = value;
#endif
        }

        public UIntPtr(uint value)
        {
            m_value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is UIntPtr)
            {
                UIntPtr obj2 = (UIntPtr)obj;
                return m_value == obj2.m_value;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (int)m_value;
        }

        public uint ToUInt32()
        {
            return (uint)m_value;
        }

        public ulong ToUInt64()
        {
            return (ulong)m_value;
        }

        public override string ToString()
        {
            return ((uint)m_value).ToString();
        }

        public static bool operator ==(UIntPtr value1, UIntPtr value2)
        {
            return value1.m_value == value2.m_value;
        }

        public static bool operator !=(UIntPtr value1, UIntPtr value2)
        {
            return value1.m_value != value2.m_value;
        }

        public static explicit operator ulong(UIntPtr value)
        {
            return (ulong)value.m_value;
        }

        public static explicit operator uint(UIntPtr value)
        {
            return (uint)value.m_value;
        }

        public static explicit operator UIntPtr(ulong value)
        {
            return new UIntPtr(value);
        }

        public static explicit operator UIntPtr(uint value)
        {
            return new UIntPtr(value);
        }

        public static int Size
        {
            get { return 4; }
        }
    }
}
