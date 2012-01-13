//
// System.UInt64.cs
//
// Author:
//   Miguel de Icaza (miguel@ximian.com)
//
// (C) Ximian, Inc.  http://www.ximian.com
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
using System.Threading;

namespace System
{
	[Serializable]
	[CLSCompliant (false)]
#if NET_2_0
	[System.Runtime.InteropServices.ComVisible (true)]
#endif
	public struct UInt64 : IFormattable, IConvertible, IComparable
#if NET_2_0
		, IComparable<UInt64>, IEquatable <UInt64>
#endif
	{
		public const ulong MaxValue = 0xffffffffffffffff;
		public const ulong MinValue = 0;

		internal ulong m_value;

		public int CompareTo (object value)
		{
			if (value == null)
				return 1;

			if (!(value is System.UInt64))
				throw new ArgumentException (Locale.GetText ("Value is not a System.UInt64."));

			if (this.m_value == (ulong) value)
				return 0;

			if (this.m_value < (ulong) value)
				return -1;

			return 1;
		}

		public override bool Equals (object obj)
		{
			if (!(obj is System.UInt64))
				return false;

			return ((ulong) obj) == m_value;
		}

		public override int GetHashCode ()
		{
			return (int)(m_value & 0xffffffff) ^ (int)(m_value >> 32);
		}

//#if NET_2_0
		public int CompareTo (ulong value)
		{
			if (m_value == value)
				return 0;
			if (m_value > value)
				return 1;
			else
				return -1;
		}

		public bool Equals (ulong value)
		{
			return value == m_value;
		}
//#endif

		[CLSCompliant (false)]
		public static ulong Parse (string s)
		{
			return Parse (s, NumberStyles.Integer, null);
		}

		[CLSCompliant (false)]
		public static ulong Parse (string s, IFormatProvider provider)
		{
			return Parse (s, NumberStyles.Integer, provider);
		}

		[CLSCompliant (false)]
		public static ulong Parse (string s, NumberStyles style)
		{
			return Parse (s, style, null);
		}

		internal static bool Parse (string s, NumberStyles style, IFormatProvider provider, bool tryParse, out ulong result, out Exception exc)
		{
			result = 0;
			exc = null;

			if (s == null) {
				if (!tryParse)
					exc = new ArgumentNullException ("s");
				return false;
			}

			if (s.Length == 0) {
				if (!tryParse)
					exc = Int32.GetFormatException ();
				return false;
			}

			NumberFormatInfo nfi;
			if (provider != null) {
				Type typeNFI = typeof (NumberFormatInfo);
				nfi = (NumberFormatInfo) provider.GetFormat (typeNFI);
			}
			else
				nfi = CultureInfo.CurrentCulture.NumberFormat;

			if (!Int32.CheckStyle (style, tryParse, ref exc))
				return false;

			bool AllowCurrencySymbol = (style & NumberStyles.AllowCurrencySymbol) != 0;
			bool AllowHexSpecifier = (style & NumberStyles.AllowHexSpecifier) != 0;
			bool AllowThousands = (style & NumberStyles.AllowThousands) != 0;
			bool AllowDecimalPoint = (style & NumberStyles.AllowDecimalPoint) != 0;
			bool AllowParentheses = (style & NumberStyles.AllowParentheses) != 0;
			bool AllowTrailingSign = (style & NumberStyles.AllowTrailingSign) != 0;
			bool AllowLeadingSign = (style & NumberStyles.AllowLeadingSign) != 0;
			bool AllowTrailingWhite = (style & NumberStyles.AllowTrailingWhite) != 0;
			bool AllowLeadingWhite = (style & NumberStyles.AllowLeadingWhite) != 0;

			int pos = 0;

			if (AllowLeadingWhite && !Int32.JumpOverWhite (ref pos, s, true, tryParse, ref exc))
				return false;

			bool foundOpenParentheses = false;
			bool negative = false;
			bool foundSign = false;
			bool foundCurrency = false;

			// Pre-number stuff
			if (AllowParentheses && s [pos] == '(') {
				foundOpenParentheses = true;
				foundSign = true;
				negative = true; // MS always make the number negative when there parentheses
						 // even when NumberFormatInfo.NumberNegativePattern != 0!!!
				pos++;
				if (AllowLeadingWhite && !Int32.JumpOverWhite (ref pos, s, true, tryParse, ref exc))
					return false;

				if (s.Substring (pos, nfi.NegativeSign.Length) == nfi.NegativeSign) {
					if (!tryParse)
						exc = Int32.GetFormatException ();
					return false;
				}
				if (s.Substring (pos, nfi.PositiveSign.Length) == nfi.PositiveSign) {
					if (!tryParse)
						exc = Int32.GetFormatException ();
					return false;
				}
			}

			if (AllowLeadingSign && !foundSign) {
				// Sign + Currency
				Int32.FindSign (ref pos, s, nfi, ref foundSign, ref negative);
				if (foundSign) {
					if (AllowLeadingWhite && !Int32.JumpOverWhite (ref pos, s, true, tryParse, ref exc))
						return false;
					if (AllowCurrencySymbol) {
						Int32.FindCurrency (ref pos, s, nfi,
								    ref foundCurrency);
						if (foundCurrency && AllowLeadingWhite &&
								!Int32.JumpOverWhite (ref pos, s, true, tryParse, ref exc))
							return false;
					}
				}
			}

			if (AllowCurrencySymbol && !foundCurrency) {
				// Currency + sign
				Int32.FindCurrency (ref pos, s, nfi, ref foundCurrency);
				if (foundCurrency) {
					if (AllowLeadingWhite && !Int32.JumpOverWhite (ref pos, s, true, tryParse, ref exc))
						return false;
					if (foundCurrency) {
						if (!foundSign && AllowLeadingSign) {
							Int32.FindSign (ref pos, s, nfi, ref foundSign,
									ref negative);
							if (foundSign && AllowLeadingWhite &&
									!Int32.JumpOverWhite (ref pos, s, true, tryParse, ref exc))
								return false;
						}
					}
				}
			}

			ulong number = 0;
			int nDigits = 0;
			bool decimalPointFound = false;
			ulong digitValue;
			char hexDigit;

			// Number stuff
			// Just the same as Int32, but this one adds instead of substract
			do {

				if (!Int32.ValidDigit (s [pos], AllowHexSpecifier)) {
					if (AllowThousands && Int32.FindOther (ref pos, s, nfi.NumberGroupSeparator))
						continue;
					else
						if (!decimalPointFound && AllowDecimalPoint && 
						    Int32.FindOther (ref pos, s, nfi.NumberDecimalSeparator)) {
							decimalPointFound = true;
							continue;
						}
					break;
				}
				else if (AllowHexSpecifier) {
					nDigits++;
					hexDigit = s [pos++];
					if (Char.IsDigit (hexDigit))
						digitValue = (ulong) (hexDigit - '0');
					else if (Char.IsLower (hexDigit))
						digitValue = (ulong) (hexDigit - 'a' + 10);
					else
						digitValue = (ulong) (hexDigit - 'A' + 10);

					if (tryParse){
						// Any number above 32 will do 
						bool can_overflow = number > 0xffff;

						number = number * 16 + digitValue;

						if (can_overflow && number < 16)
							return false;
					} else
						number = checked (number * 16 + digitValue);
				}
				else if (decimalPointFound) {
					nDigits++;
					// Allows decimal point as long as it's only 
					// followed by zeroes.
					if (s [pos++] != '0') {
						if (!tryParse)
							exc = new OverflowException (Locale.GetText ("Value too large or too small."));
						return false;
					}
				}
				else {
					nDigits++;

					try {
						number = checked (number * 10 + (ulong) (s [pos++] - '0'));
					}
					catch (OverflowException) {
						if (!tryParse)
							exc = new OverflowException (Locale.GetText ("Value too large or too small."));
						return false;
					}
				}
			} while (pos < s.Length);

			// Post number stuff
			if (nDigits == 0) {
				if (!tryParse)
					exc = Int32.GetFormatException ();
				return false;
			}

			if (AllowTrailingSign && !foundSign) {
				// Sign + Currency
				Int32.FindSign (ref pos, s, nfi, ref foundSign, ref negative);
				if (foundSign) {
					if (AllowTrailingWhite && !Int32.JumpOverWhite (ref pos, s, true, tryParse, ref exc))
						return false;
					if (AllowCurrencySymbol)
						Int32.FindCurrency (ref pos, s, nfi, ref foundCurrency);
				}
			}

			if (AllowCurrencySymbol && !foundCurrency) {
				// Currency + sign
				Int32.FindCurrency (ref pos, s, nfi, ref foundCurrency);
				if (foundCurrency) {
					if (AllowTrailingWhite && !Int32.JumpOverWhite (ref pos, s, true, tryParse, ref exc))
						return false;
					if (!foundSign && AllowTrailingSign)
						Int32.FindSign (ref pos, s, nfi, ref foundSign, ref negative);
				}
			}

			if (AllowTrailingWhite && pos < s.Length && !Int32.JumpOverWhite (ref pos, s, false, tryParse, ref exc))
				return false;

			if (foundOpenParentheses) {
				if (pos >= s.Length || s [pos++] != ')') {
					if (!tryParse)
						exc = Int32.GetFormatException ();
					return false;
				}
				if (AllowTrailingWhite && pos < s.Length && !Int32.JumpOverWhite (ref pos, s, false, tryParse, ref exc))
					return false;
			}

			if (pos < s.Length && s [pos] != '\u0000') {
				if (!tryParse)
					exc = Int32.GetFormatException ();
				return false;
			}

			// -0 is legal but other negative values are not
			if (negative && (number > 0)) {
				if (!tryParse)
					exc = new OverflowException (
					    Locale.GetText ("Negative number"));
				return false;
			}

			result = number;
			return true;
		}

		[CLSCompliant (false)]
		public static ulong Parse (string s, NumberStyles style, IFormatProvider fp) 
		{
			Exception exc;
			ulong res;

			if (!Parse (s, style, fp, false, out res, out exc))
				throw exc;

			return res;
		}


#if NET_2_0
		[CLSCompliant (false)]
		public static bool TryParse (string s, out ulong result) 
		{
			Exception exc;
			if (!Parse (s, NumberStyles.Integer, null, true, out result, out exc)) {
				result = 0;
				return false;
			}

			return true;
		}

		[CLSCompliant (false)]
		public static bool TryParse (string s, NumberStyles style, IFormatProvider provider, out ulong result) 
		{
			Exception exc;
			if (!Parse (s, style, provider, true, out result, out exc)) {
				result = 0;
				return false;
			}

			return true;
		}
#endif

		public override string ToString ()
		{
			return NumberFormatter.FormatGeneral (new NumberFormatter.NumberStore (m_value));
		}

		public string ToString (IFormatProvider provider)
		{
			return NumberFormatter.FormatGeneral (new NumberFormatter.NumberStore (m_value), provider);
		}

		public string ToString (string format)
		{
			return ToString (format, null);
		}

		public string ToString (string format, IFormatProvider provider)
		{
			NumberFormatInfo nfi = NumberFormatInfo.GetInstance (provider);
			return NumberFormatter.NumberToString (format, m_value, nfi);
		}

		// =========== IConvertible Methods =========== //
		public TypeCode GetTypeCode ()
		{
			return TypeCode.UInt64;
		}

		bool IConvertible.ToBoolean (IFormatProvider provider)
		{
			return System.Convert.ToBoolean (m_value);
		}

		byte IConvertible.ToByte (IFormatProvider provider)
		{
			return System.Convert.ToByte (m_value);
		}

		char IConvertible.ToChar (IFormatProvider provider)
		{
			return System.Convert.ToChar (m_value);
		}

		DateTime IConvertible.ToDateTime (IFormatProvider provider)
		{
			return System.Convert.ToDateTime (m_value);
		}

		decimal IConvertible.ToDecimal (IFormatProvider provider)
		{
			return System.Convert.ToDecimal (m_value);
		}

		double IConvertible.ToDouble (IFormatProvider provider)
		{
			return System.Convert.ToDouble (m_value);
		}

		short IConvertible.ToInt16 (IFormatProvider provider)
		{
			return System.Convert.ToInt16 (m_value);
		}

		int IConvertible.ToInt32 (IFormatProvider provider)
		{
			return System.Convert.ToInt32 (m_value);
		}

		long IConvertible.ToInt64 (IFormatProvider provider)
		{
			return System.Convert.ToInt64 (m_value);
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return System.Convert.ToSByte (m_value);
		}

		float IConvertible.ToSingle (IFormatProvider provider)
		{
			return System.Convert.ToSingle (m_value);
		}

		object IConvertible.ToType (Type conversionType, IFormatProvider provider)
		{
			return System.Convert.ToType (m_value, conversionType, provider);
		}

		ushort IConvertible.ToUInt16 (IFormatProvider provider)
		{
			return System.Convert.ToUInt16 (m_value);
		}

		uint IConvertible.ToUInt32 (IFormatProvider provider)
		{
			return System.Convert.ToUInt32 (m_value);
		}

		ulong IConvertible.ToUInt64 (IFormatProvider provider)
		{
			return m_value;
		}
	}
}
