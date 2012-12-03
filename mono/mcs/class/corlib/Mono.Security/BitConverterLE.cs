//CHANGED
//
// Mono.Security.BitConverterLE.cs
//  Like System.BitConverter but always little endian
//
// Author:
//   Bernie Solomon
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

using System;
using flash.utils;

namespace Mono.Security
{
    internal sealed class BitConverterLE
    {
        private BitConverterLE()
        {
        }

        internal static byte[] GetBytes(bool value)
        {
            return new byte[] { value ? (byte)1 : (byte)0 };
        }

        internal static byte[] GetBytes(char value)
        {
            byte b0 = (byte)value;
            byte b1 = (byte)(value >> 8);
            return new byte[] { b0, b1 };
        }

        internal static byte[] GetBytes(short value)
        {
            byte b0 = (byte)value;
            byte b1 = (byte)(value >> 8);
            return new byte[] { b0, b1 };
        }

        internal static byte[] GetBytes(int value)
        {
            uint v = (uint)value;
            byte b0 = (byte)v;
            byte b1 = (byte)(v >> 8);
            byte b2 = (byte)(v >> 16);
            byte b3 = (byte)(v >> 24);
            return new byte[] { b0, b1, b2, b3 };
        }

        internal static byte[] GetBytes(long value)
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
            return new byte[] { b0, b1, b2, b3, b4, b5, b6, b7 };
        }

        internal static byte[] GetBytes(ushort value)
        {
            byte b0 = (byte)value;
            byte b1 = (byte)(value >> 8);
            return new byte[] { b0, b1 };
        }

        internal static byte[] GetBytes(uint value)
        {
            byte b0 = (byte)value;
            byte b1 = (byte)(value >> 8);
            byte b2 = (byte)(value >> 16);
            byte b3 = (byte)(value >> 24);
            return new byte[] { b0, b1, b2, b3 };
        }

        internal static byte[] GetBytes(ulong value)
        {
            byte b0 = (byte)value;
            byte b1 = (byte)(value >> 8);
            byte b2 = (byte)(value >> 16);
            byte b3 = (byte)(value >> 24);
            byte b4 = (byte)(value >> 32);
            byte b5 = (byte)(value >> 40);
            byte b6 = (byte)(value >> 48);
            byte b7 = (byte)(value >> 56);
            return new byte[] { b0, b1, b2, b3, b4, b5, b6, b7 };
        }

        private static ByteArray CreateByteArray()
        {
            ByteArray arr = new ByteArray();
            arr.endian = Endian.LITTLE_ENDIAN;
            return arr;
        }

        internal static byte[] GetBytes(float value)
        {
            ByteArray arr = CreateByteArray();
            arr.writeFloat(value);
            arr.position = 0;
            int n = 4;
            byte[] res = new byte[n];
            for (int i = 0; i < n; ++i)
                res[i] = (byte)arr.readByte();
            return res;
        }

        internal static byte[] GetBytes(double value)
        {
            ByteArray arr = CreateByteArray();
            arr.writeDouble(value);
            arr.position = 0;
            int n = 8;
            byte[] res = new byte[n];
            for (int i = 0; i < n; ++i)
                res[i] = (byte)arr.readByte();
            return res;
        }

        internal static bool ToBoolean(byte[] value, int startIndex)
        {
            return value[startIndex] != 0;
        }

        internal static char ToChar(byte[] value, int startIndex)
        {
            return (char)(value[startIndex]
                          | (value[startIndex + 1] << 8));
        }

        internal static short ToInt16(byte[] value, int startIndex)
        {
            return (short)ToUInt16(value, startIndex);
        }

        internal static int ToInt32(byte[] value, int startIndex)
        {
            return (int)ToUInt32(value, startIndex);
        }

        internal static long ToInt64(byte[] value, int startIndex)
        {
            return (long)ToUInt64(value, startIndex);
        }

        internal static ushort ToUInt16(byte[] value, int startIndex)
        {
            return (ushort)(value[startIndex]
                                | (value[startIndex + 1] << 8));
        }

        internal static uint ToUInt32(byte[] value, int startIndex)
        {
            return (uint)((uint)value[startIndex]
                          | ((uint)value[startIndex + 1] << 8)
                          | ((uint)value[startIndex + 2] << 16)
                          | ((uint)value[startIndex + 3] << 24));
        }

        internal static ulong ToUInt64(byte[] value, int startIndex)
        {
            return (ulong)((ulong)value[startIndex]
                           | ((ulong)value[startIndex + 1] << 8)
                           | ((ulong)value[startIndex + 2] << 16)
                           | ((ulong)value[startIndex + 3] << 24)
                           | ((ulong)value[startIndex + 4] << 32)
                           | ((ulong)value[startIndex + 5] << 40)
                           | ((ulong)value[startIndex + 6] << 48)
                           | ((ulong)value[startIndex + 7] << 56));
        }

        internal static float ToSingle(byte[] value, int startIndex)
        {
            ByteArray arr = CreateByteArray();
            while (startIndex < value.Length)
                arr.writeByte(value[startIndex++]);
            arr.position = 0;
            return (float)arr.readFloat();
        }

        internal static double ToDouble(byte[] value, int startIndex)
        {
            ByteArray arr = CreateByteArray();
            while (startIndex < value.Length)
                arr.writeByte(value[startIndex++]);
            arr.position = 0;
            return arr.readDouble();
        }
    }
}
