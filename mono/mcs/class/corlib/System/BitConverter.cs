//CHANGED
//
// System.BitConverter.cs
//
// Author:
//   Matt Kimball (matt@kimball.net)
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

using System.Text;
using Native;

namespace System
{
    public
#if NET_2_0
	static
#else
 sealed
#endif
 class BitConverter
    {
#if !NET_2_0
        private BitConverter()
        {
        }
#endif

        static readonly bool SwappedWordsInDouble = DoubleWordsAreSwapped();
        public static readonly bool IsLittleEndian = AmILittleEndian();

        static bool AmILittleEndian()
        {
            //TODO:
            return true;
            //ByteArray arr = new ByteArray();
            //arr.writeInt(1);
            //arr.position = 0;
            //return arr.readByte() == 1;
            // binary representations of 1.0:
            // big endian: 3f f0 00 00 00 00 00 00
            // little endian: 00 00 00 00 00 00 f0 3f
            // arm fpa little endian: 00 00 f0 3f 00 00 00 00
            //double d = 1.0;
            //byte *b = (byte*)&d;
            //return (b [0] == 0);
        }

        static bool DoubleWordsAreSwapped()
        {
            //TODO:
            // binary representations of 1.0:
            // big endian: 3f f0 00 00 00 00 00 00
            // little endian: 00 00 00 00 00 00 f0 3f
            // arm fpa little endian: 00 00 f0 3f 00 00 00 00
            //double d = 1.0;
            //byte *b = (byte*)&d;
            //return b [2] == 0xf0;
            return false;
        }

        internal static long DoubleToInt64Bits(double value)
        {
            return ToInt64(GetBytes(value), 0);
        }

        internal static double Int64BitsToDouble(long value)
        {
            return ToDouble(GetBytes(value), 0);
        }

        internal static double InternalInt64BitsToDouble(long value)
        {
            return SwappableToDouble(GetBytes(value), 0);
        }

        #region GetBytes
        public static byte[] GetBytes(bool value)
        {
            return new byte[] { value ? (byte)1 : (byte)0 };
        }

        public static byte[] GetBytes(char value)
        {
            byte b0 = (byte)value;
            byte b1 = (byte)(value >> 8);
            if (IsLittleEndian) return new byte[] {b0, b1};
            return new byte[] {b1, b0};
        }

        public static byte[] GetBytes(short value)
        {
            byte b0 = (byte)value;
            byte b1 = (byte)(value >> 8);
            if (IsLittleEndian) return new byte[] {b0, b1};
            return new byte[] {b1, b0};
        }

        [CLSCompliant(false)]
        public static byte[] GetBytes(ushort value)
        {
            byte b0 = (byte)value;
            byte b1 = (byte)(value >> 8);
            if (IsLittleEndian) return new byte[] {b0, b1};
            return new byte[] {b1, b0};
        }

        public static byte[] GetBytes(int value)
        {
            uint v = (uint)value;
            byte b0 = (byte)v;
            byte b1 = (byte)(v >> 8);
            byte b2 = (byte)(v >> 16);
            byte b3 = (byte)(v >> 24);
            if (IsLittleEndian)
                return new byte[] { b0, b1, b2, b3 };
            return new byte[] { b3, b2, b1, b0 };
        }

        [CLSCompliant(false)]
        public static byte[] GetBytes(uint value)
        {
            byte b0 = (byte)value;
            byte b1 = (byte)(value >> 8);
            byte b2 = (byte)(value >> 16);
            byte b3 = (byte)(value >> 24);
            if (IsLittleEndian)
                return new byte[] {b0, b1, b2, b3};
            return new byte[] {b3, b2, b1, b0};
        }

        public static byte[] GetBytes(long value)
        {
            ulong v = (ulong)value;
            byte b0 = (byte)v;
            byte b1 = (byte)(v >> 8);
            byte b2 = (byte)(v >> 16);
            byte b3 = (byte)(v >> 24);
            byte b4 = (byte)(v >> 32);
            byte b5 = (byte)(v >> 40);
            byte b6 = (byte)(v >> 48);
            byte b7 = (byte)(v >> 56);
            if (IsLittleEndian)
                return new byte[] { b0, b1, b2, b3, b4, b5, b6, b7 };
            return new byte[] { b7, b6, b5, b4, b3, b2, b1, b0 };
        }

        [CLSCompliant(false)]
        public static byte[] GetBytes(ulong value)
        {
            byte b0 = (byte)value;
            byte b1 = (byte)(value >> 8);
            byte b2 = (byte)(value >> 16);
            byte b3 = (byte)(value >> 24);
            byte b4 = (byte)(value >> 32);
            byte b5 = (byte)(value >> 40);
            byte b6 = (byte)(value >> 48);
            byte b7 = (byte)(value >> 56);
            if (IsLittleEndian)
                return new byte[] {b0, b1, b2, b3, b4, b5, b6, b7};
            return new byte[] {b7, b6, b5, b4, b3, b2, b1, b0};
        }

        public static byte[] GetBytes(float value)
        {
            ByteArray arr = ByteArrayFactory.Create(IsLittleEndian);
			arr.WriteFloat(value);
	        return ReadBytes(arr, 4);
        }

        public static byte[] GetBytes(double value)
        {
			ByteArray arr = ByteArrayFactory.Create(IsLittleEndian);
			arr.WriteDouble(value);
	        return ReadBytes(arr, 8);
        }

		private static byte[] ReadBytes(ByteArray byteArray, int n)
		{
			byteArray.position = 0;
			byte[] res = new byte[n];
			for (int i = 0; i < n; ++i)
				res[i] = (byte)byteArray.ReadByte();
			return res;
		}
        #endregion

        private static void Check(byte[] value, int startIndex, int size)
        {
            if (value == null)
            {
#if NET_2_0
                throw new ArgumentNullException("value");
#else
                throw new ArgumentNullException("byteArray");
#endif
            }
            int n = value.Length;
            if (startIndex < 0 || startIndex >= n)
                throw new ArgumentOutOfRangeException("startIndex", "Index was"
                    + " out of range. Must be non-negative and less than the"
                    + " size of the collection.");
            if (n - size < startIndex)
                throw new ArgumentException("Destination array is not long"
                    + " enough to copy all the items in the collection."
                    + " Check array index and length.");
        }

        public static bool ToBoolean(byte[] value, int startIndex)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (startIndex < 0 || (startIndex > value.Length - 1))
                throw new ArgumentOutOfRangeException("startIndex", "Index was"
                    + " out of range. Must be non-negative and less than the"
                    + " size of the collection.");

            return value[startIndex] != 0;
        }

        public static char ToChar(byte[] value, int startIndex)
        {
            Check(value, startIndex, 2);
            
            if (IsLittleEndian)
                return (char)(value[startIndex]
                              | (value[startIndex + 1] << 8));
            return (char)(value[startIndex + 1]
                          | (value[startIndex] << 8));
        }

        public static short ToInt16(byte[] value, int startIndex)
        {
            return (short)ToUInt16(value, startIndex);
        }

        public static int ToInt32(byte[] value, int startIndex)
        {
            return (int)ToUInt32(value, startIndex);
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(byte[] value, int startIndex)
        {
            Check(value, startIndex, 2);

            if (IsLittleEndian)
                return (ushort)(value[startIndex]
                                | (value[startIndex + 1] << 8));
            return (ushort)(value[startIndex + 1]
                            | (value[startIndex] << 8));
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(byte[] value, int startIndex)
        {
            Check(value, startIndex, 4);

            uint v;
            if (IsLittleEndian)
            {
                v = value[startIndex];
                v |= (uint)value[startIndex + 1] << 8;
                v |= (uint)value[startIndex + 2] << 16;
                v |= (uint)value[startIndex + 3] << 24;
            }
            else
            {
                v = value[startIndex + 3];
                v |= (uint)value[startIndex + 2] << 8;
                v |= (uint)value[startIndex + 1] << 16;
                v |= (uint)value[startIndex] << 24;
            }
            return v;
        }

        public static long ToInt64(byte[] value, int startIndex)
        {
            return (long)ToUInt64(value, startIndex);
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(byte[] value, int startIndex)
        {
            Check(value, startIndex, 8);

            ulong v;
            if (IsLittleEndian)
            {
                v = (ulong)value[startIndex];
                v |= (ulong)value[startIndex + 1] << 8;
                v |= (ulong)value[startIndex + 2] << 16;
                v |= (ulong)value[startIndex + 3] << 24;
                v |= (ulong)value[startIndex + 4] << 32;
                v |= (ulong)value[startIndex + 5] << 40;
                v |= (ulong)value[startIndex + 6] << 48;
                v |= (ulong)value[startIndex + 7] << 56;
            }
            else
            {
                v = (ulong)value[startIndex + 7];
                v |= (ulong)value[startIndex + 6] << 8;
                v |= (ulong)value[startIndex + 5] << 16;
                v |= (ulong)value[startIndex + 4] << 24;
                v |= (ulong)value[startIndex + 3] << 32;
                v |= (ulong)value[startIndex + 2] << 40;
                v |= (ulong)value[startIndex + 1] << 48;
                v |= (ulong)value[startIndex] << 56;
            }
            return v;
        }

	    public static float ToSingle(byte[] value, int startIndex)
        {
            Check(value, startIndex, 4);
            ByteArray arr = ByteArrayFactory.Create(IsLittleEndian, value, startIndex);
            return arr.ReadFloat();
        }

        public static double ToDouble(byte[] value, int startIndex)
        {
            Check(value, startIndex, 8);
            ByteArray arr = ByteArrayFactory.Create(IsLittleEndian, value, startIndex);
            return arr.ReadDouble();
        }

	    internal static double SwappableToDouble(byte[] value, int startIndex)
        {
            if (SwappedWordsInDouble)
            {
                Check(value, startIndex, 8);
				ByteArray arr = ByteArrayFactory.Create(IsLittleEndian);
                arr.WriteByte(value[startIndex + 4]);
                arr.WriteByte(value[startIndex + 5]);
                arr.WriteByte(value[startIndex + 6]);
                arr.WriteByte(value[startIndex + 7]);
                arr.WriteByte(value[startIndex + 0]);
                arr.WriteByte(value[startIndex + 1]);
                arr.WriteByte(value[startIndex + 2]);
                arr.WriteByte(value[startIndex + 3]);
	            arr.position = 0;
                return arr.ReadDouble();
            }

            return ToDouble(value, startIndex);
        }

        #region ToString
        public static string ToString(byte[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return ToString(value, 0, value.Length);
        }

        public static string ToString(byte[] value, int startIndex)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return ToString(value, startIndex, value.Length - startIndex);
        }

        public static string ToString(byte[] value, int startIndex, int length)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            // The 4th and last clause (start_index >= value.Length)
            // was added as a small fix to a very obscure bug.
            // It makes a small difference when start_index is
            // outside the range and length==0. 
            if (startIndex < 0 || startIndex >= value.Length)
            {
#if NET_2_0
				// special (but valid) case (e.g. new byte [0])
				if ((startIndex == 0) && (value.Length == 0))
					return String.Empty;
#endif
                throw new ArgumentOutOfRangeException("startIndex", "Index was"
                    + " out of range. Must be non-negative and less than the"
                    + " size of the collection.");
            }

            if (length < 0)
                throw new ArgumentOutOfRangeException("length",
                    "Value must be positive.");

            // note: re-ordered to avoid possible integer overflow
            if (startIndex > value.Length - length)
                throw new ArgumentException("startIndex + length > value.Length");

            if (length == 0)
                return string.Empty;

            StringBuilder builder = new StringBuilder(length * 3 - 1);
            int end = startIndex + length;

            for (int i = startIndex; i < end; i++)
            {
                if (i > startIndex)
                    builder.Append('-');

                char high = (char)((value[i] >> 4) & 0x0f);
                char low = (char)(value[i] & 0x0f);

                if (high < 10)
                    high += '0';
                else
                {
                    high -= (char)10;
                    high += 'A';
                }

                if (low < 10)
                    low += '0';
                else
                {
                    low -= (char)10;
                    low += 'A';
                }
                builder.Append(high);
                builder.Append(low);
            }

            return builder.ToString();
        }
        #endregion
    }
}
