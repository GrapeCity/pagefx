//CHANGED
//
// System.Convert.cs
//
// Author:
//   Derek Holden (dholden@draper.com)
//   Duncan Mak (duncan@ximian.com)
//
// (C) Ximian, Inc.  http://www.ximian.com
//
//
// System.Convert class. This was written word for word off the 
// Library specification for System.Convert in the ECMA TC39 TG2 
// and TG3 working documents. The first page of which has a table
// for all legal conversion scenerios.
//
// This header and the one above it can be formatted however, just trying
// to keep it consistent w/ the existing mcs headers.
//
// This Convert class could be written another way, with each type 
// implementing IConvertible and defining their own conversion functions,
// and this class just calling the type's implementation. Or, they can 
// be defined here and the implementing type can use these functions when 
// defining their IConvertible interface. Byte's ToBoolean() calls 
// Convert.ToBoolean(byte), or Convert.ToBoolean(byte) calls 
// byte.ToBoolean(). The first case is what is done here.
//
// See http://lists.ximian.com/archives/public/mono-list/2001-July/000525.html
//
// There are also conversion functions that are not defined in
// the ECMA draft, such as there is no bool ToBoolean(DateTime value), 
// and placing that somewhere won't compile w/ this Convert since the
// function doesn't exist. However calling that when using Microsoft's
// System.Convert doesn't produce any compiler errors, it just throws
// an InvalidCastException at runtime.
//
// Whenever a decimal, double, or single is converted to an integer
// based type, it is even rounded. This uses Math.Round which only 
// has Round(decimal) and Round(double), so in the Convert from 
// single cases the value is passed to Math as a double. This 
// may not be completely necessary.
//
// The .NET Framework SDK lists DBNull as a member of this class
// as 'public static readonly object DBNull;'. 
//
// It should also be decided if all the cast return values should be
// returned as unchecked or not.
//
// All the XML function comments were auto generated which is why they
// sound someone redundant.
//
// TYPE | BOOL BYTE CHAR DT DEC DBL I16 I32 I64 SBYT SNGL STR UI16 UI32 UI64
// -----+--------------------------------------------------------------------
// BOOL |   X    X           X   X   X   X   X    X    X   X    X    X    X
// BYTE |   X    X    X      X   X   X   X   X    X    X   X    X    X    X
// CHAR |        X    X              X   X   X    X        X    X    X    X
// DT   |                 X                                X
// DEC  |   X    X           X   X   X   X   X    X    X   X    X    X    X
// DBL  |   X    X           X   X   X   X   X    X    X   X    X    X    X
// I16  |   X    X    X      X   X   X   X   X    X    X   X    X    X    X
// I32  |   X    X    X      X   X   X   X   X    X    X   X    X    X    X
// I64  |   X    X    X      X   X   X   X   X    X    X   X    X    X    X
// SBYT |   X    X    X      X   X   X   X   X    X    X   X    X    X    X
// SNGL |   X    X           X   X   X   X   X    X    X   X    X    X    X
// STR  |   X    X    X   X  X   X   X   X   X    X    X   X    X    X    X
// UI16 |   X    X    X      X   X   X   X   X    X    X   X    X    X    X
// UI32 |   X    X    X      X   X   X   X   X    X    X   X    X    X    X
// UI64 |   X    X    X      X   X   X   X   X    X    X   X    X    X    X
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

using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{

    //	[CLSCompliant(false)]
#if NET_2_0
	public static class Convert {
#else
    public sealed class Convert
    {
        private Convert()
        {
        }
#endif

        public static TypeCode GetTypeCode(object value)
        {
            if (value == null)
                return TypeCode.Empty;
            else
                return Type.GetTypeCode(value.GetType());
        }

        public static bool IsDBNull(object value)
        {
            if (value is DBNull)
                return true;
            else
                return false;
        }

        // Fields
        public static readonly object DBNull = System.DBNull.Value;

        #region ToBase64

        static readonly ToBase64Transform toBase64Transform = new ToBase64Transform();

        private static readonly byte[] dbase64 =
            {
                128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128,
                128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128,
                128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 62, 128, 128, 128, 63,
                52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 128, 128, 128, 0, 128, 128,
                128, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
                15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 128, 128, 128, 128, 128,
                128, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
                41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51
            };

        static byte[] InternalFromBase64String(string str, bool allowWhitespaceOnly)
        {
            char[] c = str.ToCharArray();
            return InternalFromBase64CharArray(c, 0, str.Length, allowWhitespaceOnly);
        }

        static byte[] InternalFromBase64CharArray(char[] arr, int offset, int length, bool allowWhitespaceOnly)
        {
            int ignored = 0;
            char last = '\0', prev_last = '\0';
            for (int i = 0; i < length; i++)
            {
                char c = arr[offset + i];
                if (c >= dbase64.Length)
                    throw new FormatException("Invalid character found.");
                else if (char.IsWhiteSpace(c))
                {
                    ignored++;
                }
                else
                {
                    prev_last = last;
                    last = c;
                }
            }

            int olength = length - ignored;

            if (allowWhitespaceOnly && olength == 0)
            {
                return new byte[0];
            }

            if ((olength & 3) != 0 || olength <= 0)
                throw new FormatException("Invalid length.");

            olength = (olength * 3) / 4;
            if (last == '=')
                olength--;

            if (prev_last == '=')
                olength--;

            byte[] result = new byte[olength];
            int res_ptr = 0;
            int[] a = new int[4];
            int[] b = new int[4];
            for (int i = 0; i < length; )
            {
                int k;

                for (k = 0; k < 4 && i < length; )
                {
                    char c = arr[i++];
                    if (char.IsWhiteSpace(c))
                        continue;

                    a[k] = c;
                    if (((b[k] = dbase64[c]) & 0x80) != 0)
                    {
                        throw new FormatException("Invalid character found.");
                    }
                    k++;
                }

                result[res_ptr++] = (byte)((b[0] << 2) | (b[1] >> 4));
                if (a[2] != '=')
                    result[res_ptr++] = (byte)((b[1] << 4) | (b[2] >> 2));
                if (a[3] != '=')
                    result[res_ptr++] = (byte)((b[2] << 6) | b[3]);

                while (i < length && char.IsWhiteSpace(arr[i]))
                    i++;
            }

            return result;
        }

        public static byte[] FromBase64CharArray(char[] inArray, int offset, int length)
        {
            if (inArray == null)
                throw new ArgumentNullException("inArray");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "offset < 0");
            if (length < 0)
                throw new ArgumentOutOfRangeException("offset", "length < 0");
            // avoid integer overflow
            if (offset > inArray.Length - length)
                throw new ArgumentOutOfRangeException("offset", "offset + length > array.Length");

            return InternalFromBase64CharArray(inArray, offset, length, false);
        }

        public static byte[] FromBase64String(string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");

            if (s.Length == 0)
            {
                return new byte[0];
            }

#if NET_2_0
			return InternalFromBase64String (s, true);
#else
            return InternalFromBase64String(s, false);
#endif
        }

        public static int ToBase64CharArray(byte[] inArray, int offsetIn, int length,
                            char[] outArray, int offsetOut)
        {
            if (inArray == null)
                throw new ArgumentNullException("inArray");
            if (outArray == null)
                throw new ArgumentNullException("outArray");
            if (offsetIn < 0 || length < 0 || offsetOut < 0)
                throw new ArgumentOutOfRangeException("offsetIn", "offsetIn, length, offsetOut < 0");
            // avoid integer overflow
            if (offsetIn > inArray.Length - length)
                throw new ArgumentOutOfRangeException("offsetIn", "offsetIn + length > array.Length");

            // note: normally ToBase64Transform doesn't support multiple block processing

            byte[] outArr = toBase64Transform.InternalTransformFinalBlock(inArray, offsetIn, length);


            char[] cOutArr = new ASCIIEncoding().GetChars(outArr);

            // avoid integer overflow
            if (offsetOut > outArray.Length - cOutArr.Length)
                throw new ArgumentOutOfRangeException("offsetOut", "offsetOut + cOutArr.Length > outArray.Length");

            Array.Copy(cOutArr, 0, outArray, offsetOut, cOutArr.Length);

            return cOutArr.Length;
        }

        public static string ToBase64String(byte[] inArray)
        {
            if (inArray == null)
                throw new ArgumentNullException("inArray");

            return ToBase64String(inArray, 0, inArray.Length);
        }

        public static string ToBase64String(byte[] inArray, int offset, int length)
        {
            if (inArray == null)
                throw new ArgumentNullException("inArray");
            if (offset < 0 || length < 0)
                throw new ArgumentOutOfRangeException("offset", "offset < 0 || length < 0");
            // avoid integer overflow
            if (offset > inArray.Length - length)
                throw new ArgumentOutOfRangeException("offset", "offset + length > array.Length");

            // note: normally ToBase64Transform doesn't support multiple block processing
            byte[] outArr = toBase64Transform.InternalTransformFinalBlock(inArray, offset, length);

            return (new ASCIIEncoding().GetString(outArr));
        }

#if NET_2_0
		[ComVisible (true)]
		public static string ToBase64String (byte[] inArray, Base64FormattingOptions options)
		{
			if (inArray == null)
				throw new ArgumentNullException ("inArray");
			return ToBase64String (inArray, 0, inArray.Length, options);
		}

		[ComVisible (true)]
		public static string ToBase64String (byte[] inArray, int offset, int length, Base64FormattingOptions options)
		{
			if (inArray == null)
				throw new ArgumentNullException ("inArray");
			if (offset < 0 || length < 0)
				throw new ArgumentOutOfRangeException ("offset < 0 || length < 0");
			// avoid integer overflow
			if (offset > inArray.Length - length)
				throw new ArgumentOutOfRangeException ("offset + length > array.Length");

			if (options == Base64FormattingOptions.InsertLineBreaks)
				return ToBase64StringBuilderWithLine (inArray, offset, length).ToString ();
			else
				return Encoding.ASCII.GetString (toBase64Transform.InternalTransformFinalBlock (inArray, offset, length));
		}

		public static int ToBase64CharArray (byte[] inArray, int offsetIn, int length, 
						    char[] outArray, int offsetOut, Base64FormattingOptions options)
		{
			if (inArray == null)
				throw new ArgumentNullException ("inArray");
			if (outArray == null)
				throw new ArgumentNullException ("outArray");
			if (offsetIn < 0 || length < 0 || offsetOut < 0)
				throw new ArgumentOutOfRangeException ("offsetIn, length, offsetOut < 0");
			// avoid integer overflow
			if (offsetIn > inArray.Length - length)
				throw new ArgumentOutOfRangeException ("offsetIn + length > array.Length");

			// note: normally ToBase64Transform doesn't support multiple block processing
			if (options == Base64FormattingOptions.InsertLineBreaks) {
				StringBuilder sb = ToBase64StringBuilderWithLine (inArray, offsetIn, length);
				sb.CopyTo (0, outArray, offsetOut, sb.Length);
				return sb.Length;
			} else {
				byte[] outArr = toBase64Transform.InternalTransformFinalBlock (inArray, offsetIn, length);
			
				char[] cOutArr = Encoding.ASCII.GetChars (outArr);
			
				// avoid integer overflow
				if (offsetOut > outArray.Length - cOutArr.Length)
					throw new ArgumentOutOfRangeException ("offsetOut + cOutArr.Length > outArray.Length");
			
				Array.Copy (cOutArr, 0, outArray, offsetOut, cOutArr.Length);
				return cOutArr.Length;
			}
		}

		static StringBuilder ToBase64StringBuilderWithLine (byte [] inArray, int offset, int length)
		{
			BinaryReader reader = new BinaryReader (new MemoryStream (inArray, offset, length));
			byte[] b = null;

			StringBuilder sb = new StringBuilder ();
			do {
				// 54 bytes of input makes for 72 bytes of output.
				b = reader.ReadBytes (54);
				if (b.Length > 0)
					sb.AppendLine (Encoding.ASCII.GetString (toBase64Transform.InternalTransformFinalBlock (b, 0, b.Length)));
			} while (b.Length > 0);
			return sb;
		}
#endif
        #endregion

        #region ToBoolean

        public static bool ToBoolean(bool value)
        {
            return value;
        }

        public static bool ToBoolean(byte value)
        {
            return (value != 0);
        }

        public static bool ToBoolean(char value)
        {
            throw new InvalidCastException(Locale.GetText("Can't convert char to bool"));
        }

        internal static bool ToBoolean(DateTime value)
        {
            throw new InvalidCastException(Locale.GetText("Can't convert date to bool"));
        }

        public static bool ToBoolean(decimal value)
        {
            return (value != 0M);
        }

        public static bool ToBoolean(double value)
        {
            return (value != 0);
        }

        public static bool ToBoolean(float value)
        {
            return (value != 0f);
        }

        public static bool ToBoolean(int value)
        {
            return (value != 0);
        }

        public static bool ToBoolean(long value)
        {
            return (value != 0);
        }

        [CLSCompliant(false)]
        public static bool ToBoolean(sbyte value)
        {
            return (value != 0);
        }

        public static bool ToBoolean(short value)
        {
            return (value != 0);
        }

        public static bool ToBoolean(string value)
        {
            if (value == null)
                return false; // LAMESPEC: Spec says throw ArgumentNullException
            return Boolean.Parse(value);
        }

        public static bool ToBoolean(string value, IFormatProvider provider)
        {
            if (value == null)
                return false; // LAMESPEC: Spec says throw ArgumentNullException
            return Boolean.Parse(value); // provider is ignored.
        }

        [CLSCompliant(false)]
        public static bool ToBoolean(uint value)
        {
            return (value != 0);
        }

        [CLSCompliant(false)]
        public static bool ToBoolean(ulong value)
        {
            return (value != 0);
        }

        [CLSCompliant(false)]
        public static bool ToBoolean(ushort value)
        {
            //if (value == null)
            //	return false;
            return (value != 0);
        }

        public static bool ToBoolean(object value)
        {
            if (value == null)
                return false;
            return ToBoolean(value, null);
        }

        public static bool ToBoolean(object value, IFormatProvider provider)
        {
            if (value == null)
                return false;
            return ((IConvertible)value).ToBoolean(provider);
        }

        #endregion // ========== Byte Conversions ========== //

        #region ToByte
        public static byte ToByte(bool value)
        {
            return (byte)(value ? 1 : 0);
        }

        public static byte ToByte(byte value)
        {
            return value;
        }

        public static byte ToByte(char value)
        {
            if (value > Byte.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Byte.MaxValue"));

            return (byte)value;
        }

        internal static byte ToByte(DateTime value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        public static byte ToByte(decimal value)
        {
            if (value > Byte.MaxValue || value < Byte.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Byte.MaxValue or less than Byte.MinValue"));

            // Returned Even-Rounded
            return (byte)(Math.Round(value));
        }

        public static byte ToByte(double value)
        {
            if (value > Byte.MaxValue || value < Byte.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Byte.MaxValue or less than Byte.MinValue"));

            // This and the float version of ToByte are the only ones
            // the spec listed as checking for .NaN and Infinity overflow
            if (Double.IsNaN(value) || Double.IsInfinity(value))
                throw new OverflowException(Locale.GetText(
                    "Value is equal to Double.NaN, Double.PositiveInfinity, or Double.NegativeInfinity"));

            // Returned Even-Rounded
            return (byte)(Math.Round(value));
        }

        public static byte ToByte(float value)
        {
            if (value > Byte.MaxValue || value < Byte.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Byte.MaxValue or less than Byte.Minalue"));

            // This and the double version of ToByte are the only ones
            // the spec listed as checking for .NaN and Infinity overflow
            if (Single.IsNaN(value) || Single.IsInfinity(value))
                throw new OverflowException(Locale.GetText(
                    "Value is equal to Single.NaN, Single.PositiveInfinity, or Single.NegativeInfinity"));

            // Returned Even-Rounded, pass it as a double, could have this
            // method just call Convert.ToByte ( (double)value)
            return (byte)(Math.Round((double)value));
        }

        public static byte ToByte(int value)
        {
            if (value > Byte.MaxValue || value < Byte.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Byte.MaxValue or less than Byte.MinValue"));

            return (byte)value;
        }

        public static byte ToByte(long value)
        {
            if (value > Byte.MaxValue || value < Byte.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Byte.MaxValue or less than Byte.MinValue"));

            return (byte)value;
        }

        [CLSCompliant(false)]
        public static byte ToByte(sbyte value)
        {
            if (value < Byte.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is less than Byte.MinValue"));

            return (byte)value;
        }

        public static byte ToByte(short value)
        {
            if (value > Byte.MaxValue || value < Byte.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Byte.MaxValue or less than Byte.MinValue"));

            return (byte)value;
        }

        public static byte ToByte(string value)
        {
            if (value == null)
                return 0; // LAMESPEC: Spec says throw ArgumentNullException
            return Byte.Parse(value);
        }

        public static byte ToByte(string value, IFormatProvider provider)
        {
            if (value == null)
                return 0; // LAMESPEC: Spec says throw ArgumentNullException
            return Byte.Parse(value, provider);
        }

        public static byte ToByte(string value, int fromBase)
        {
            int retVal = ConvertFromBase(value, fromBase, true);

            if (retVal < (int)Byte.MinValue || retVal > (int)Byte.MaxValue)
                throw new OverflowException();
            else
                return (byte)retVal;
        }

        [CLSCompliant(false)]
        public static byte ToByte(uint value)
        {
            if (value > Byte.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Byte.MaxValue"));

            return (byte)value;
        }

        [CLSCompliant(false)]
        public static byte ToByte(ulong value)
        {
            if (value > Byte.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Byte.MaxValue"));

            return (byte)value;
        }

        [CLSCompliant(false)]
        public static byte ToByte(ushort value)
        {
            if (value > Byte.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Byte.MaxValue"));

            return (byte)value;
        }

        public static byte ToByte(object value)
        {
            if (value == null)
                return 0;
            return ToByte(value, null);
        }

        public static byte ToByte(object value, IFormatProvider provider)
        {
            if (value == null)
                return 0;
            return ((IConvertible)value).ToByte(provider);
        }
        #endregion

        #region ToChar
        internal static char ToChar(bool value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        public static char ToChar(byte value)
        {
            return (char)value;
        }

        public static char ToChar(char value)
        {
            return value;
        }

        internal static char ToChar(DateTime value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        public static char ToChar(decimal value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        public static char ToChar(double value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        public static char ToChar(int value)
        {
            if (value > Char.MaxValue || value < Char.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Char.MaxValue or less than Char.MinValue"));

            return (char)value;
        }

        public static char ToChar(long value)
        {
            if (value > Char.MaxValue || value < Char.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Char.MaxValue or less than Char.MinValue"));

            return (char)value;
        }

        public static char ToChar(float value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        [CLSCompliant(false)]
        public static char ToChar(sbyte value)
        {
            if (value < Char.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is less than Char.MinValue"));

            return (char)value;
        }

        public static char ToChar(short value)
        {
            if (value < Char.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is less than Char.MinValue"));

            return (char)value;
        }

        public static char ToChar(string value)
        {
            return Char.Parse(value);
        }

        public static char ToChar(string value, IFormatProvider provider)
        {
            return Char.Parse(value); // provider is ignored.
        }

        [CLSCompliant(false)]
        public static char ToChar(uint value)
        {
            if (value > Char.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Char.MaxValue"));

            return (char)value;
        }

        [CLSCompliant(false)]
        public static char ToChar(ulong value)
        {
            if (value > Char.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Char.MaxValue"));

            return (char)value;
        }

        [CLSCompliant(false)]
        public static char ToChar(ushort value)
        {
            if (value > Char.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Char.MaxValue"));

            return (char)value;
        }

        public static char ToChar(object value)
        {
            if (value == null)
                return '\0';
            return ToChar(value, null);
        }

        public static char ToChar(object value, IFormatProvider provider)
        {
            if (value == null)
                return '\0';
            return ((IConvertible)value).ToChar(provider);
        }
        #endregion

        #region ToDateTime
        public static DateTime ToDateTime(string value)
        {
            if (value == null)
                return DateTime.MinValue; // LAMESPEC: Spec says throw ArgumentNullException
            return DateTime.Parse(value);
        }

        public static DateTime ToDateTime(string value, IFormatProvider provider)
        {
            if (value == null)
                return DateTime.MinValue; // LAMESPEC: Spec says throw ArgumentNullException
            return DateTime.Parse(value, provider);
        }

        internal static DateTime ToDateTime(bool value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        internal static DateTime ToDateTime(byte value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        internal static DateTime ToDateTime(char value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        internal static DateTime ToDateTime(DateTime value)
        {
            return value;
        }

        internal static DateTime ToDateTime(decimal value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        internal static DateTime ToDateTime(double value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        internal static DateTime ToDateTime(short value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        internal static DateTime ToDateTime(int value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        internal static DateTime ToDateTime(long value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        internal static DateTime ToDateTime(float value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        public static DateTime ToDateTime(object value)
        {
            if (value == null)
                return DateTime.MinValue;
            return ToDateTime(value, null);
        }

        public static DateTime ToDateTime(object value, IFormatProvider provider)
        {
            if (value == null)
                return DateTime.MinValue;
            return ((IConvertible)value).ToDateTime(provider);
        }

        [CLSCompliant(false)]
        internal static DateTime ToDateTime(sbyte value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }
        [CLSCompliant(false)]
        internal static DateTime ToDateTime(ushort value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        [CLSCompliant(false)]
        internal static DateTime ToDateTime(uint value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        [CLSCompliant(false)]
        internal static DateTime ToDateTime(ulong value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }
        #endregion

        #region ToDecimal
        public static decimal ToDecimal(bool value)
        {
            return value ? 1 : 0;
        }

        public static decimal ToDecimal(byte value)
        {
            return (decimal)value;
        }

        public static decimal ToDecimal(char value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        public static decimal ToDecimal(DateTime value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        public static decimal ToDecimal(decimal value)
        {
            return value;
        }

        public static decimal ToDecimal(double value)
        {
            //if (value > (double)Decimal.MaxValue || value < (double)Decimal.MinValue)
            if ((decimal)value > Decimal.MaxValue || (decimal)value < Decimal.MinValue)
                throw new OverflowException("Value is greater than Decimal.MaxValue or less than Decimal.MinValue");

            return (decimal)value;
        }

        public static decimal ToDecimal(float value)
        {
            return (decimal)value;
        }

        public static decimal ToDecimal(int value)
        {
            return (decimal)value;
        }

        public static decimal ToDecimal(long value)
        {
            //return (decimal)value;
            return new Decimal(value);
        }

        [CLSCompliant(false)]
        public static decimal ToDecimal(sbyte value)
        {
            return (decimal)value;
        }

        public static decimal ToDecimal(short value)
        {
            return (decimal)value;
        }

        public static decimal ToDecimal(string value)
        {
            if (value == null)
                return new Decimal(0); // LAMESPEC: Spec says throw ArgumentNullException
            return Decimal.Parse(value);
        }

        public static decimal ToDecimal(string value, IFormatProvider provider)
        {
            if (value == null)
                return new Decimal(0); // LAMESPEC: Spec says throw ArgumentNullException
            return Decimal.Parse(value, provider);
        }

        [CLSCompliant(false)]
        public static decimal ToDecimal(uint value)
        {
            return (decimal)value;
        }

        [CLSCompliant(false)]
        public static decimal ToDecimal(ulong value)
        {
            return new Decimal(value);
        }

        [CLSCompliant(false)]
        public static decimal ToDecimal(ushort value)
        {
            return (decimal)value;
        }

        public static decimal ToDecimal(object value)
        {
            if (value == null)
                return new Decimal(0);
            return ToDecimal(value, null);
        }

        public static decimal ToDecimal(object value, IFormatProvider provider)
        {
            if (value == null)
                return new Decimal(0);
            return ((IConvertible)value).ToDecimal(provider);
        }
        #endregion

        #region ToDouble
        public static double ToDouble(bool value)
        {
            return value ? 1 : 0;
        }

        public static double ToDouble(byte value)
        {
            return (double)value;
        }

        public static double ToDouble(char value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        internal static double ToDouble(DateTime value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        public static double ToDouble(decimal value)
        {
            return (double)value;
        }

        public static double ToDouble(double value)
        {
            return value;
        }

        public static double ToDouble(float value)
        {
            return (double)value;
        }

        public static double ToDouble(int value)
        {
            return (double)value;
        }

        public static double ToDouble(long value)
        {
            return (double)value;
        }

        [CLSCompliant(false)]
        public static double ToDouble(sbyte value)
        {
            return (double)value;
        }

        public static double ToDouble(short value)
        {
            return (double)value;
        }

        public static double ToDouble(string value)
        {
            if (value == null)
                return 0.0; // LAMESPEC: Spec says throw ArgumentNullException
            return Double.Parse(value);
        }

        public static double ToDouble(string value, IFormatProvider provider)
        {
            if (value == null)
                return 0.0; // LAMESPEC: Spec says throw ArgumentNullException
            return Double.Parse(value, provider);
        }

        [CLSCompliant(false)]
        public static double ToDouble(uint value)
        {
            return (double)value;
        }

        [CLSCompliant(false)]
        public static double ToDouble(ulong value)
        {
            return (double)value;
        }

        [CLSCompliant(false)]
        public static double ToDouble(ushort value)
        {
            return (double)value;
        }

        public static double ToDouble(object value)
        {
            if (value == null)
                return 0.0;
            return ToDouble(value, null);
        }

        public static double ToDouble(object value, IFormatProvider provider)
        {
            if (value == null)
                return 0.0;
            return ((IConvertible)value).ToDouble(provider);
        }
        #endregion

        #region ToInt16
        public static short ToInt16(bool value)
        {
            return (short)(value ? 1 : 0);
        }

        public static short ToInt16(byte value)
        {
            return (short)value;
        }

        public static short ToInt16(char value)
        {
            if (value > Int16.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int16.MaxValue"));

            return (short)value;
        }

        internal static short ToInt16(DateTime value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        public static short ToInt16(decimal value)
        {
            if (value > Int16.MaxValue || value < Int16.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int16.MaxValue or less than Int16.MinValue"));

            // Returned Even-Rounded
            return (short)(Math.Round(value));
        }

        public static short ToInt16(double value)
        {
            if (value > Int16.MaxValue || value < Int16.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int16.MaxValue or less than Int16.MinValue"));

            // Returned Even-Rounded
            return (short)(Math.Round(value));
        }

        public static short ToInt16(float value)
        {
            if (value > Int16.MaxValue || value < Int16.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int16.MaxValue or less than Int16.MinValue"));

            // Returned Even-Rounded, use Math.Round pass as a double.
            return (short)Math.Round((double)value);
        }

        public static short ToInt16(int value)
        {
            if (value > Int16.MaxValue || value < Int16.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int16.MaxValue or less than Int16.MinValue"));

            return (short)value;
        }

        public static short ToInt16(long value)
        {
            if (value > Int16.MaxValue || value < Int16.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int16.MaxValue or less than Int16.MinValue"));

            return (short)value;
        }

        [CLSCompliant(false)]
        public static short ToInt16(sbyte value)
        {
            return (short)value;
        }

        public static short ToInt16(short value)
        {
            return value;
        }

        public static short ToInt16(string value)
        {
            if (value == null)
                return 0; // LAMESPEC: Spec says throw ArgumentNullException
            return Int16.Parse(value);
        }

        public static short ToInt16(string value, IFormatProvider provider)
        {
            if (value == null)
                return 0; // LAMESPEC: Spec says throw ArgumentNullException
            return Int16.Parse(value, provider);
        }

        public static short ToInt16(string value, int fromBase)
        {
            int result = ConvertFromBase(value, fromBase, false);
            if (fromBase != 10)
            {
                if (result > ushort.MaxValue)
                {
                    throw new OverflowException("Value was either too large or too small for an Int16.");
                }

                // note: no sign are available to detect negatives
                if (result > Int16.MaxValue)
                {
                    // return negative 2's complement
                    return Convert.ToInt16(-(65536 - result));
                }
            }
            return Convert.ToInt16(result);
        }

        [CLSCompliant(false)]
        public static short ToInt16(uint value)
        {
            if (value > Int16.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int16.MaxValue"));

            return (short)value;
        }

        [CLSCompliant(false)]
        public static short ToInt16(ulong value)
        {
            if (value > (ulong)Int16.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int16.MaxValue"));
            return (short)value;
        }

        [CLSCompliant(false)]
        public static short ToInt16(ushort value)
        {
            if (value > Int16.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int16.MaxValue"));

            return (short)value;
        }

        public static short ToInt16(object value)
        {
            if (value == null)
                return 0;
            return ToInt16(value, null);
        }

        public static short ToInt16(object value, IFormatProvider provider)
        {
            if (value == null)
                return 0;
            return ((IConvertible)value).ToInt16(provider);
        }
        #endregion

        #region ToInt32
        public static int ToInt32(bool value)
        {
            return value ? 1 : 0;
        }

        public static int ToInt32(byte value)
        {
            return (int)value;
        }

        public static int ToInt32(char value)
        {
            return (int)value;
        }

        internal static int ToInt32(DateTime value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        public static int ToInt32(decimal value)
        {
            if (value > Int32.MaxValue || value < Int32.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int32.MaxValue or less than Int32.MinValue"));

            // Returned Even-Rounded
            return (int)(Math.Round(value));
        }

        public static int ToInt32(double value)
        {
            if (value > Int32.MaxValue || value < Int32.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int32.MaxValue or less than Int32.MinValue"));

            // Returned Even-Rounded
            return (int)(Math.Round(value));
        }

        public static int ToInt32(float value)
        {
            if (value > Int32.MaxValue || value < Int32.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int32.MaxValue or less than Int32.MinValue"));

            // Returned Even-Rounded, pass as a double, could just call
            // Convert.ToInt32 ( (double)value);
            return (int)(Math.Round((double)value));
        }

        public static int ToInt32(int value)
        {
            return value;
        }

        public static int ToInt32(long value)
        {
            if (value > Int32.MaxValue || value < Int32.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int32.MaxValue or less than Int32.MinValue"));

            return (int)value;
        }

        [CLSCompliant(false)]
        public static int ToInt32(sbyte value)
        {
            return (int)value;
        }

        public static int ToInt32(short value)
        {
            return (int)value;
        }

        public static int ToInt32(string value)
        {
            if (value == null)
                return 0; // LAMESPEC: Spec says throw ArgumentNullException
            return Int32.Parse(value);
        }

        public static int ToInt32(string value, IFormatProvider provider)
        {
            if (value == null)
                return 0; // LAMESPEC: Spec says throw ArgumentNullException
            return Int32.Parse(value, provider);
        }


        public static int ToInt32(string value, int fromBase)
        {
            return ConvertFromBase(value, fromBase, false);
        }

        [CLSCompliant(false)]
        public static int ToInt32(uint value)
        {
            if (value > Int32.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int32.MaxValue"));

            return (int)value;
        }

        [CLSCompliant(false)]
        public static int ToInt32(ulong value)
        {
            if (value > Int32.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int32.MaxValue"));

            return (int)value;
        }

        [CLSCompliant(false)]
        public static int ToInt32(ushort value)
        {
            return (int)value;
        }

        public static int ToInt32(object value)
        {
            if (value == null)
                return 0;
            return ToInt32(value, null);
        }

        public static int ToInt32(object value, IFormatProvider provider)
        {
            if (value == null)
                return 0;
            return ((IConvertible)value).ToInt32(provider);
        }
        #endregion

        #region ToInt64
        public static long ToInt64(bool value)
        {
            return value ? 1 : 0;
        }

        public static long ToInt64(byte value)
        {
            return (long)(ulong)value;
        }

        public static long ToInt64(char value)
        {
            return (long)value;
        }

        internal static long ToInt64(DateTime value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        public static long ToInt64(decimal value)
        {
            if (value > Int64.MaxValue || value < Int64.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int64.MaxValue or less than Int64.MinValue"));

            // Returned Even-Rounded
            return (long)(Math.Round(value));
        }

        public static long ToInt64(double value)
        {
            if (value > Int64.MaxValue || value < Int64.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int64.MaxValue or less than Int64.MinValue"));

            // Returned Even-Rounded
            return (long)(Math.Round(value));
        }

        public static long ToInt64(float value)
        {
            if (value > Int64.MaxValue || value < Int64.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int64.MaxValue or less than Int64.MinValue"));

            // Returned Even-Rounded, pass to Math as a double, could
            // just call Convert.ToInt64 ( (double)value);
            return (long)(Math.Round((double)value));
        }

        public static long ToInt64(int value)
        {
            return (long)value;
        }

        public static long ToInt64(long value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static long ToInt64(sbyte value)
        {
            return (long)value;
        }

        public static long ToInt64(short value)
        {
            return (long)value;
        }

        public static long ToInt64(string value)
        {
            if (value == null)
                return 0; // LAMESPEC: Spec says throw ArgumentNullException
            return Int64.Parse(value);
        }

        public static long ToInt64(string value, IFormatProvider provider)
        {
            if (value == null)
                return 0; // LAMESPEC: Spec says throw ArgumentNullException
            return Int64.Parse(value, provider);
        }

        public static long ToInt64(string value, int fromBase)
        {
            return ConvertFromBase64(value, fromBase, false);
        }

        [CLSCompliant(false)]
        public static long ToInt64(uint value)
        {
            return (long)(ulong)value;
        }

        [CLSCompliant(false)]
        public static long ToInt64(ulong value)
        {
            if (UInt64.Compare(value, Int64.MaxValue) > 0)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than Int64.MaxValue"));

            return (long)value;
        }

        [CLSCompliant(false)]
        public static long ToInt64(ushort value)
        {
            return (long)(ulong)value;
        }

        public static long ToInt64(object value)
        {
            if (value == null)
                return 0;
            return ToInt64(value, null);
        }

        public static long ToInt64(object value, IFormatProvider provider)
        {
            if (value == null)
                return 0;
            return ((IConvertible)value).ToInt64(provider);
        }
        #endregion

        #region ToSByte
        [CLSCompliant(false)]
        public static sbyte ToSByte(bool value)
        {
            return (sbyte)(value ? 1 : 0);
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(byte value)
        {
            if (value > SByte.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than SByte.MaxValue"));

            return (sbyte)value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(char value)
        {
            if (value > SByte.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than SByte.MaxValue"));

            return (sbyte)value;
        }

        [CLSCompliant(false)]
        internal static sbyte ToSByte(DateTime value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(decimal value)
        {
            if (value > SByte.MaxValue || value < SByte.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than SByte.MaxValue or less than SByte.MinValue"));

            // Returned Even-Rounded
            return (sbyte)(Math.Round(value));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(double value)
        {
            if (value > SByte.MaxValue || value < SByte.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than SByte.MaxValue or less than SByte.MinValue"));

            // Returned Even-Rounded
            return (sbyte)(Math.Round(value));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(float value)
        {
            if (value > SByte.MaxValue || value < SByte.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than SByte.MaxValue or less than SByte.Minalue"));

            // Returned Even-Rounded, pass as double to Math
            return (sbyte)(Math.Round((double)value));
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(int value)
        {
            if (value > SByte.MaxValue || value < SByte.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than SByte.MaxValue or less than SByte.MinValue"));

            return (sbyte)value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(long value)
        {
            if (value > SByte.MaxValue || value < SByte.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than SByte.MaxValue or less than SByte.MinValue"));

            return (sbyte)value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(sbyte value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(short value)
        {
            if (value > SByte.MaxValue || value < SByte.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than SByte.MaxValue or less than SByte.MinValue"));

            return (sbyte)value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(string value)
        {
            if (value == null)
                return 0; // LAMESPEC: Spec says throw ArgumentNullException
            return SByte.Parse(value);
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(string value, IFormatProvider provider)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            return SByte.Parse(value, provider);
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(string value, int fromBase)
        {
            int result = ConvertFromBase(value, fromBase, false);
            if (fromBase != 10)
            {
                // note: no sign are available to detect negatives
                if (result > SByte.MaxValue)
                {
                    // return negative 2's complement
                    return Convert.ToSByte(-(256 - result));
                }
            }
            return Convert.ToSByte(result);
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(uint value)
        {
            if (value > SByte.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than SByte.MaxValue"));

            return (sbyte)value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(ulong value)
        {
            if (value > (ulong)SByte.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than SByte.MaxValue"));

            return (sbyte)value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(ushort value)
        {
            if (value > SByte.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than SByte.MaxValue"));

            return (sbyte)value;
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(object value)
        {
            if (value == null)
                return 0;
            return ToSByte(value, null);
        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(object value, IFormatProvider provider)
        {
            if (value == null)
                return 0;
            return ((IConvertible)value).ToSByte(provider);
        }
        #endregion

        #region ToSingle
        public static float ToSingle(bool value)
        {
            return value ? 1 : 0;
        }

        public static float ToSingle(byte value)
        {
            return (float)value;
        }

        public static float ToSingle(Char value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        internal static float ToSingle(DateTime value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        public static float ToSingle(decimal value)
        {
            return (float)value;
        }

        public static float ToSingle(double value)
        {
            return (float)value;
        }

        public static float ToSingle(float value)
        {
            return value;
        }

        public static float ToSingle(int value)
        {
            return (float)value;
        }

        public static float ToSingle(long value)
        {
            return (float)value;
        }

        [CLSCompliant(false)]
        public static float ToSingle(sbyte value)
        {
            return (float)value;
        }

        public static float ToSingle(short value)
        {
            return (float)value;
        }

        public static float ToSingle(string value)
        {
            if (value == null)
                return 0.0f; // LAMESPEC: Spec says throw ArgumentNullException
            return Single.Parse(value);
        }

        public static float ToSingle(string value, IFormatProvider provider)
        {
            if (value == null)
                return 0.0f; // LAMESPEC: Spec says throw ArgumentNullException
            return Single.Parse(value, provider);
        }

        [CLSCompliant(false)]
        public static float ToSingle(uint value)
        {
            return (float)value;
        }

        [CLSCompliant(false)]
        public static float ToSingle(ulong value)
        {
            return (float)value;
        }

        [CLSCompliant(false)]
        public static float ToSingle(ushort value)
        {
            return (float)value;
        }

        public static float ToSingle(object value)
        {
            if (value == null)
                return 0.0f;
            return ToSingle(value, null);
        }

        //		[CLSCompliant (false)]
        public static float ToSingle(object value, IFormatProvider provider)
        {
            if (value == null)
                return 0.0f;
            return ((IConvertible)value).ToSingle(provider);
        }
        #endregion

        #region ToString
        public static string ToString(bool value)
        {
            return value.ToString();
        }

        public static string ToString(bool value, IFormatProvider provider)
        {
            return value.ToString(); // the same as ToString (bool).
        }

        public static string ToString(byte value)
        {
            return value.ToString();
        }

        public static string ToString(byte value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

        public static string ToString(byte value, int toBase)
        {
            if (value == 0)
                return "0";
            if (toBase == 10)
                return value.ToString();
            if (toBase == 8)
                return ConvertToBase8((ulong)value);

            byte[] val = BitConverter.GetBytes(value);

            switch (toBase)
            {
                case 2:
                    return ConvertToBase2(val);
                case 16:
                    return ConvertToBase16(val);
                default:
                    throw new ArgumentException(Locale.GetText("toBase is not valid."));
            }
        }

        public static string ToString(char value)
        {
            return value.ToString();
        }

        public static string ToString(char value, IFormatProvider provider)
        {
            return value.ToString(); // the same as ToString (char)
        }

        public static string ToString(DateTime value)
        {
            return value.ToString();
        }

        public static string ToString(DateTime value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

#if NOT_PFX
public static string ToString(DateTime value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }
#endif

        public static string ToString(decimal value)
        {
            return value.ToString();
        }

        public static string ToString(decimal value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

        public static string ToString(double value)
        {
            return value.ToString();
        }

        public static string ToString(double value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

        public static string ToString(float value)
        {
            return value.ToString();
        }

        public static string ToString(float value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

        public static string ToString(int value)
        {
            return value.ToString();
        }

        public static string ToString(int value, int toBase)
        {
            if (value == 0)
                return "0";
            if (toBase == 10)
                return value.ToString();
            if (toBase == 8)
                return ConvertToBase8((ulong)(uint)value);

            byte[] val = BitConverter.GetBytes(value);

            switch (toBase)
            {
                case 2:
                    return ConvertToBase2(val);
                case 16:
                    return ConvertToBase16(val);
                default:
                    throw new ArgumentException(Locale.GetText("toBase is not valid."));
            }
        }

        public static string ToString(int value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

        public static string ToString(long value)
        {
            return value.ToString();
        }

        public static string ToString(long value, int toBase)
        {
            if (value == 0)
                return "0";
            if (toBase == 10)
                return value.ToString();

            switch (toBase)
            {
                case 2:
                    return ConvertToBase2(BitConverter.GetBytes(value));
                case 8:
                    return ConvertToBase8((ulong)value);
                case 16:
                    return ConvertToBase16(BitConverter.GetBytes(value));
                default:
                    throw new ArgumentException(Locale.GetText("toBase is not valid."));
            }
        }

        public static string ToString(long value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

        public static string ToString(object value)
        {
            return ToString(value, null);
        }

        public static string ToString(object value, IFormatProvider provider)
        {
            if (value is IConvertible)
                return ((IConvertible)value).ToString(provider);
            else if (value != null)
                return value.ToString();
            return String.Empty;
        }

        [CLSCompliant(false)]
        public static string ToString(sbyte value)
        {
            return value.ToString();
        }

        [CLSCompliant(false)]
        public static string ToString(sbyte value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

        public static string ToString(short value)
        {
            return value.ToString();
        }

        public static string ToString(short value, int toBase)
        {
            if (value == 0)
                return "0";
            if (toBase == 10)
                return value.ToString();
            if (toBase == 8)
                return ConvertToBase8((ulong)(ushort)value);

            byte[] val = BitConverter.GetBytes(value);

            switch (toBase)
            {
                case 2:
                    return ConvertToBase2(val);
                case 16:
                    return ConvertToBase16(val);
                default:
                    throw new ArgumentException(Locale.GetText("toBase is not valid."));
            }
        }

        public static string ToString(short value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

        internal static string ToString(string value)
        {
            return value;
        }

        internal static string ToString(string value, IFormatProvider provider)
        {
            return value; // provider is ignored.
        }

        [CLSCompliant(false)]
        public static string ToString(uint value)
        {
            return value.ToString();
        }

        [CLSCompliant(false)]
        public static string ToString(uint value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

        [CLSCompliant(false)]
        public static string ToString(ulong value)
        {
            return value.ToString();
        }

        [CLSCompliant(false)]
        public static string ToString(ulong value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }

        [CLSCompliant(false)]
        public static string ToString(ushort value)
        {
            return value.ToString();
        }

        [CLSCompliant(false)]
        public static string ToString(ushort value, IFormatProvider provider)
        {
            return value.ToString(provider);
        }
        #endregion

        #region ToUInt16
        [CLSCompliant(false)]
        public static ushort ToUInt16(bool value)
        {
            return (ushort)(value ? 1 : 0);
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(byte value)
        {
            return (ushort)value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(char value)
        {
            return (ushort)value;
        }

        [CLSCompliant(false)]
        internal static ushort ToUInt16(DateTime value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(decimal value)
        {
            if (value > UInt16.MaxValue || value < UInt16.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than UInt16.MaxValue or less than UInt16.MinValue"));

            // Returned Even-Rounded
            return (ushort)(Math.Round(value));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(double value)
        {
            if (value > UInt16.MaxValue || value < UInt16.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than UInt16.MaxValue or less than UInt16.MinValue"));

            // Returned Even-Rounded
            return (ushort)(Math.Round(value));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(float value)
        {
            if (value > UInt16.MaxValue || value < UInt16.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than UInt16.MaxValue or less than UInt16.MinValue"));

            // Returned Even-Rounded, pass as double to Math
            return (ushort)(Math.Round((double)value));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(int value)
        {
            if (value > UInt16.MaxValue || value < UInt16.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than UInt16.MaxValue or less than UInt16.MinValue"));

            return (ushort)value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(long value)
        {
            if (value > UInt16.MaxValue || value < UInt16.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than UInt16.MaxValue or less than UInt16.MinValue"));

            return (ushort)value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(sbyte value)
        {
            if (value < UInt16.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is less than UInt16.MinValue"));

            return (ushort)value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(short value)
        {
            if (value < UInt16.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is less than UInt16.MinValue"));

            return (ushort)value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(string value)
        {
            if (value == null)
                return 0; // LAMESPEC: Spec says throw ArgumentNullException
            return UInt16.Parse(value);
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(string value, IFormatProvider provider)
        {
            if (value == null)
                return 0; // LAMESPEC: Spec says throw ArgumentNullException
            return UInt16.Parse(value, provider);
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(string value, int fromBase)
        {
            return ToUInt16(ConvertFromBase(value, fromBase, true));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(uint value)
        {
            if (value > UInt16.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than UInt16.MaxValue"));

            return (ushort)value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(ulong value)
        {
            if (value > (ulong)UInt16.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than UInt16.MaxValue"));

            return (ushort)value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(ushort value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(object value)
        {
            if (value == null)
                return 0;
            return ToUInt16(value, null);
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(object value, IFormatProvider provider)
        {
            if (value == null)
                return 0;
            return ((IConvertible)value).ToUInt16(provider);
        }
        #endregion

        #region ToUInt32
        [CLSCompliant(false)]
        public static uint ToUInt32(bool value)
        {
            return (uint)(value ? 1 : 0);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(byte value)
        {
            return (uint)value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(char value)
        {
            return (uint)value;
        }

        [CLSCompliant(false)]
        internal static uint ToUInt32(DateTime value)
        {
            throw new InvalidCastException("This conversion is not supported.");
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(decimal value)
        {
            if (value > UInt32.MaxValue || value < UInt32.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than UInt32.MaxValue or less than UInt32.MinValue"));

            // Returned Even-Rounded
            return (uint)(Math.Round(value));
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(double value)
        {
            if (value > UInt32.MaxValue || value < UInt32.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than UInt32.MaxValue or less than UInt32.MinValue"));

            // Returned Even-Rounded
            return (uint)(Math.Round(value));
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(float value)
        {
            if (value > UInt32.MaxValue || value < UInt32.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than UInt32.MaxValue or less than UInt32.MinValue"));

            // Returned Even-Rounded, pass as double to Math
            return (uint)(Math.Round((double)value));
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(int value)
        {
            if (value < UInt32.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is less than UInt32.MinValue"));

            return (uint)value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(long value)
        {
            if (value > UInt32.MaxValue || value < UInt32.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than UInt32.MaxValue or less than UInt32.MinValue"));

            return (uint)value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(sbyte value)
        {
            if (value < UInt32.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is less than UInt32.MinValue"));

            return (uint)value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(short value)
        {
            if (value < UInt32.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is less than UInt32.MinValue"));

            return (uint)value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(string value)
        {
            if (value == null)
                return 0; // LAMESPEC: Spec says throw ArgumentNullException
            return UInt32.Parse(value);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(string value, IFormatProvider provider)
        {
            if (value == null)
                return 0; // LAMESPEC: Spec says throw ArgumentNullException
            return UInt32.Parse(value, provider);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(string value, int fromBase)
        {
            return (uint)ConvertFromBase(value, fromBase, true);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(uint value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(ulong value)
        {
            if (value > UInt32.MaxValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than UInt32.MaxValue"));

            return (uint)value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(ushort value)
        {
            return (uint)value;
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(object value)
        {
            if (value == null)
                return 0;
            return ToUInt32(value, null);
        }

        [CLSCompliant(false)]
        public static uint ToUInt32(object value, IFormatProvider provider)
        {
            if (value == null)
                return 0;
            return ((IConvertible)value).ToUInt32(provider);
        }
        #endregion

        #region ToUInt64
        [CLSCompliant(false)]
        public static ulong ToUInt64(bool value)
        {
            return (ulong)(value ? 1 : 0);
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(byte value)
        {
            return (ulong)value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(char value)
        {
            return (ulong)value;
        }

        [CLSCompliant(false)]
        internal static ulong ToUInt64(DateTime value)
        {
            throw new InvalidCastException("The conversion is not supported.");
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(decimal value)
        {
            if (value > UInt64.MaxValue || value < UInt64.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than UInt64.MaxValue or less than UInt64.MinValue"));

            // Returned Even-Rounded
            return (ulong)(Math.Round(value));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(double value)
        {
            if (value > UInt64.MaxValue || value < UInt64.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than UInt64.MaxValue or less than UInt64.MinValue"));

            // Returned Even-Rounded
            return (ulong)(Math.Round(value));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(float value)
        {
            if (value > UInt64.MaxValue || value < UInt64.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is greater than UInt64.MaxValue or less than UInt64.MinValue"));

            // Returned Even-Rounded, pass as a double to Math
            return (ulong)(Math.Round((double)value));
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(int value)
        {
            if (value < (int)UInt64.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is less than UInt64.MinValue"));

            return (ulong)value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(long value)
        {
            if (value < (long)UInt64.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is less than UInt64.MinValue"));

            return (ulong)value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(sbyte value)
        {
            if (value < (sbyte)UInt64.MinValue)
                throw new OverflowException
                ("Value is less than UInt64.MinValue");

            return (ulong)value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(short value)
        {
            if (value < (short)UInt64.MinValue)
                throw new OverflowException(Locale.GetText(
                    "Value is less than UInt64.MinValue"));

            return (ulong)value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(string value)
        {
            if (value == null)
                return 0; // LAMESPEC: Spec says throw ArgumentNullException
            return UInt64.Parse(value);
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(string value, IFormatProvider provider)
        {
            if (value == null)
                return 0; // LAMESPEC: Spec says throw ArgumentNullException
            return UInt64.Parse(value, provider);
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(string value, int fromBase)
        {
            return (ulong)ConvertFromBase64(value, fromBase, true);
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(uint value)
        {
            return (ulong)value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(ulong value)
        {
            return value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(ushort value)
        {
            return (ulong)value;
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(object value)
        {
            if (value == null)
                return 0;
            return ToUInt64(value, null);
        }

        [CLSCompliant(false)]
        public static ulong ToUInt64(object value, IFormatProvider provider)
        {
            if (value == null)
                return 0;
            return ((IConvertible)value).ToUInt64(provider);
        }
        #endregion

        #region Helper Functions
        public static object ChangeType(object value, Type conversionType)
        {
            if ((value != null) && (conversionType == null))
                throw new ArgumentNullException("conversionType");
            CultureInfo ci = CultureInfo.CurrentCulture;
            IFormatProvider provider;
            if (conversionType == typeof(DateTime))
                provider = ci.DateTimeFormat;
            else
                provider = ci.NumberFormat;
            return ToType(value, conversionType, provider);
        }

        public static object ChangeType(object value, TypeCode typeCode)
        {
            CultureInfo ci = CultureInfo.CurrentCulture;
            InitConversionTable();
            Type conversionType = conversionTable[(int)typeCode];
            IFormatProvider provider;
            if (conversionType == typeof(DateTime))
            {
                provider = ci.DateTimeFormat;
            }
            else
            {
                provider = ci.NumberFormat;
            }
            return ToType(value, conversionType, provider);
        }

        public static object ChangeType(object value, Type conversionType, IFormatProvider provider)
        {
            if ((value != null) && (conversionType == null))
                throw new ArgumentNullException("conversionType");
            return ToType(value, conversionType, provider);
        }

        public static object ChangeType(object value, TypeCode typeCode, IFormatProvider provider)
        {
            InitConversionTable();
            Type conversionType = conversionTable[(int)typeCode];
            return ToType(value, conversionType, provider);
        }

        private static bool NotValidBase(int value)
        {
            if ((value == 2) || (value == 8) ||
               (value == 10) || (value == 16))
                return false;

            return true;
        }

        private static int ConvertFromBase(string value, int fromBase, bool unsigned)
        {
            if (NotValidBase(fromBase))
                throw new ArgumentException("fromBase is not valid.");
            if (value == null)
                return 0;

            int chars = 0;
            int result = 0;
            int digitValue;

            int i = 0;
            int len = value.Length;
            bool negative = false;

            // special processing for some bases
            switch (fromBase)
            {
                case 10:
                    if (value.Substring(i, 1) == "-")
                    {
                        if (unsigned)
                        {
                            throw new OverflowException(
                                Locale.GetText("The string was being parsed as"
                                + " an unsigned number and could not have a"
                                + " negative sign."));
                        }
                        negative = true;
                        i++;
                    }
                    break;
                case 16:
                    if (value.Substring(i, 1) == "-")
                    {
                        throw new ArgumentException("String cannot contain a "
                            + "minus sign if the base is not 10.");
                    }
                    if (len >= i + 2)
                    {
                        // 0x00 or 0X00
                        if ((value[i] == '0') && ((value[i + 1] == 'x') || (value[i + 1] == 'X')))
                        {
                            i += 2;
                        }
                    }
                    break;
                default:
                    if (value.Substring(i, 1) == "-")
                    {
                        throw new ArgumentException("String cannot contain a "
                            + "minus sign if the base is not 10.");
                    }
                    break;
            }

            if (len == i)
            {
                throw new FormatException("Could not find any parsable digits.");
            }

            if (value[i] == '+')
            {
                i++;
            }

            while (i < len)
            {
                char c = value[i++];
                if (Char.IsNumber(c))
                {
                    digitValue = c - '0';
                }
                else if (Char.IsLetter(c))
                {
                    digitValue = Char.ToLowerInvariant(c) - 'a' + 10;
                }
                else
                {
                    if (chars > 0)
                    {
                        throw new FormatException("Additional unparsable "
                            + "characters are at the end of the string.");
                    }
                    else
                    {
                        throw new FormatException("Could not find any parsable"
                            + " digits.");
                    }
                }

                if (digitValue >= fromBase)
                {
                    if (chars > 0)
                    {
                        throw new FormatException("Additional unparsable "
                            + "characters are at the end of the string.");
                    }
                    else
                    {
                        throw new FormatException("Could not find any parsable"
                            + " digits.");
                    }
                }

                result = (fromBase) * result + digitValue;
                chars++;
            }

            if (chars == 0)
                throw new FormatException("Could not find any parsable digits.");

            if (negative)
                return -result;
            else
                return result;
        }

        // note: this has nothing to do with base64 encoding (just base and Int64)
        private static long ConvertFromBase64(string value, int fromBase, bool unsigned)
        {
            if (NotValidBase(fromBase))
                throw new ArgumentException("fromBase is not valid.");
            if (value == null)
                return 0;

            int chars = 0;
            int digitValue = -1;
            long result = 0;
            bool negative = false;

            int i = 0;
            int len = value.Length;

            // special processing for some bases
            switch (fromBase)
            {
                case 10:
                    if (value.Substring(i, 1) == "-")
                    {
                        if (unsigned)
                        {
                            throw new OverflowException(
                                Locale.GetText("The string was being parsed as"
                                + " an unsigned number and could not have a"
                                + " negative sign."));
                        }
                        negative = true;
                        i++;
                    }
                    break;
                case 16:
                    if (value.Substring(i, 1) == "-")
                    {
                        throw new ArgumentException("String cannot contain a "
                            + "minus sign if the base is not 10.");
                    }
                    if (len >= i + 2)
                    {
                        // 0x00 or 0X00
                        if ((value[i] == '0') && ((value[i + 1] == 'x') || (value[i + 1] == 'X')))
                        {
                            i += 2;
                        }
                    }
                    break;
                default:
                    if (value.Substring(i, 1) == "-")
                    {
                        throw new ArgumentException("String cannot contain a "
                            + "minus sign if the base is not 10.");
                    }
                    break;
            }

            if (len == i)
            {
                throw new FormatException("Could not find any parsable digits.");
            }

            if (value[i] == '+')
            {
                i++;
            }

            while (i < len)
            {
                char c = value[i++];
                if (Char.IsNumber(c))
                {
                    digitValue = c - '0';
                }
                else if (Char.IsLetter(c))
                {
                    digitValue = Char.ToLowerInvariant(c) - 'a' + 10;
                }
                else
                {
                    if (chars > 0)
                    {
                        throw new FormatException("Additional unparsable "
                            + "characters are at the end of the string.");
                    }
                    else
                    {
                        throw new FormatException("Could not find any parsable"
                            + " digits.");
                    }
                }

                if (digitValue >= fromBase)
                {
                    if (chars > 0)
                    {
                        throw new FormatException("Additional unparsable "
                            + "characters are at the end of the string.");
                    }
                    else
                    {
                        throw new FormatException("Could not find any parsable"
                            + " digits.");
                    }
                }

                result = (fromBase * result + digitValue);
                chars++;
            }

            if (chars == 0)
                throw new FormatException("Could not find any parsable digits.");

            if (negative)
                return -1 * result;
            else
                return result;
        }

        private static void EndianSwap(ref byte[] value)
        {
            byte[] buf = new byte[value.Length];
            for (int i = 0; i < value.Length; i++)
                buf[i] = value[value.Length - 1 - i];
            value = buf;
        }

        private static string ConvertToBase2(byte[] value)
        {
            if (!BitConverter.IsLittleEndian)
                EndianSwap(ref value);
            StringBuilder sb = new StringBuilder();
            for (int i = value.Length - 1; i >= 0; i--)
            {
                byte b = value[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((b & 0x80) == 0x80)
                    {
                        sb.Append('1');
                    }
                    else
                    {
                        if (sb.Length > 0)
                            sb.Append('0');
                    }
                    b <<= 1;
                }
            }
            return sb.ToString();
        }

        private static ulong ToULong(byte[] value)
        {
            switch (value.Length)
            {
                case 1:
                    return (ulong)value[0];
                case 2:
                    return (ulong)BitConverter.ToUInt16(value, 0);
                case 4:
                    return (ulong)BitConverter.ToUInt32(value, 0);
                case 8:
                    return BitConverter.ToUInt64(value, 0);
                default:
                    throw new ArgumentException("value");
            }
        }

        private static string ConvertToBase8(ulong l)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 21; i >= 0; i--)
            {
                // 3 bits at the time
                //l = l >> i * 3;
                //Console.WriteLine("{0:X}", l);
                char val = (char)((l >> i * 3) & 0x7);
                if ((val != 0) || (sb.Length > 0))
                {
                    val += '0';
                    sb.Append(val);
                }
            }
            return sb.ToString();
        }

        private static string ConvertToBase16(byte[] value)
        {
            if (!BitConverter.IsLittleEndian)
                EndianSwap(ref value);
            StringBuilder sb = new StringBuilder();
            for (int i = value.Length - 1; i >= 0; i--)
            {
                char high = (char)((value[i] >> 4) & 0x0f);
                if ((high != 0) || (sb.Length > 0))
                {
                    if (high < 10)
                        high += '0';
                    else
                    {
                        high -= (char)10;
                        high += 'a';
                    }
                    sb.Append(high);
                }

                char low = (char)(value[i] & 0x0f);
                if ((low != 0) || (sb.Length > 0))
                {
                    if (low < 10)
                        low += '0';
                    else
                    {
                        low -= (char)10;
                        low += 'a';
                    }
                    sb.Append(low);
                }
            }
            return sb.ToString();
        }

        // Lookup table for the conversion ToType method. Order
        // is important! Used by ToType for comparing the target
        // type, and uses hardcoded array indexes.
        private static Type[] conversionTable;

        private static void InitConversionTable()
        {
            if (conversionTable == null)
            {
                conversionTable = new Type[]
                {
                    // Valid ICovnertible Types
                    null, //	0 empty
                    typeof(object), //  1 TypeCode.Object
                    typeof(DBNull), //  2 TypeCode.DBNull
                    typeof(Boolean), //  3 TypeCode.Boolean
                    typeof(Char), //  4 TypeCode.Char
                    typeof(SByte), //  5 TypeCode.SByte
                    typeof(Byte), //  6 TypeCode.Byte
                    typeof(Int16), //  7 TypeCode.Int16
                    typeof(UInt16), //  8 TypeCode.UInt16
                    typeof(Int32), //  9 TypeCode.Int32
                    typeof(UInt32), // 10 TypeCode.UInt32
                    typeof(Int64), // 11 TypeCode.Int64
                    typeof(UInt64), // 12 TypeCode.UInt64
                    typeof(Single), // 13 TypeCode.Single
                    typeof(Double), // 14 TypeCode.Double
                    typeof(Decimal), // 15 TypeCode.Decimal
                    typeof(DateTime), // 16 TypeCode.DateTime
                    null, // 17 null.
                    typeof(String), // 18 TypeCode.String
                };
            }
        }

        // Function to convert an object to another type and return
        // it as an object. In place for the core data types to use
        // when implementing IConvertible. Uses hardcoded indexes in 
        // the conversionTypes array, so if modify carefully.
        internal static object ToType(object value, Type conversionType, IFormatProvider provider)
        {
            if (value == null)
            {
                if ((conversionType != null) && conversionType.IsValueType)
                {
#if NET_2_0
					throw new InvalidCastException ("Null object can not be converted to a value type.");
#else
                    //
                    // Bug compatibility with 1.0
                    //
                    throw new NullReferenceException("Null object can not be converted to a value type.");
#endif
                }
                else
                    return null;
            }

            if (conversionType == null)
                throw new InvalidCastException("Cannot cast to destination type.");

            if (value.GetType() == conversionType)
                return value;

            IConvertible convertValue = value as IConvertible;
            if (convertValue == null)
                throw new InvalidCastException(
                    (Locale.GetText("Value is not a convertible object: " + value.GetType() + " to " +
                                    conversionType.FullName)));

            if (conversionType == null) // 0 Empty
                throw new ArgumentNullException();

            InitConversionTable();
            
            if (conversionType == conversionTable[1]) // 1 TypeCode.Object
                return value;

            if (conversionType == conversionTable[2]) // 2 TypeCode.DBNull
                throw new InvalidCastException("Cannot cast to DBNull, it's not IConvertible");

            if (conversionType == conversionTable[3]) // 3 TypeCode.Boolean
                return convertValue.ToBoolean(provider);

            if (conversionType == conversionTable[4]) // 4 TypeCode.Char
                return convertValue.ToChar(provider);

            if (conversionType == conversionTable[5]) // 5 TypeCode.SByte
                return convertValue.ToSByte(provider);

            if (conversionType == conversionTable[6]) // 6 TypeCode.Byte
                return convertValue.ToByte(provider);

            if (conversionType == conversionTable[7]) // 7 TypeCode.Int16
                return convertValue.ToInt16(provider);

            if (conversionType == conversionTable[8]) // 8 TypeCode.UInt16
                return convertValue.ToUInt16(provider);

            if (conversionType == conversionTable[9]) // 9 TypeCode.Int32
                return convertValue.ToInt32(provider);

            if (conversionType == conversionTable[10]) // 10 TypeCode.UInt32
                return convertValue.ToUInt32(provider);

            if (conversionType == conversionTable[11]) // 11 TypeCode.Int64
                return convertValue.ToInt64(provider);

            if (conversionType == conversionTable[12]) // 12 TypeCode.UInt64
                return convertValue.ToUInt64(provider);

            if (conversionType == conversionTable[13]) // 13 TypeCode.Single
                return convertValue.ToSingle(provider);

            if (conversionType == conversionTable[14]) // 14 TypeCode.Double
                return convertValue.ToDouble(provider);

            if (conversionType == conversionTable[15]) // 15 TypeCode.Decimal
                return convertValue.ToDecimal(provider);

            if (conversionType == conversionTable[16]) // 16 TypeCode.DateTime
                return convertValue.ToDateTime(provider);

            if (conversionType == conversionTable[18]) // 18 TypeCode.String
                return convertValue.ToString(provider);

            throw new InvalidCastException(Locale.GetText("Unknown target conversion type"));
        }
        #endregion
    }
}
