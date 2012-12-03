//CHANGED

//
// System.Char.cs
//
// Authors:
//   Andreas Nahr (ClassDevelopment@A-SoftTech.com)
//   Miguel de Icaza (miguel@ximian.com)
//   Jackson Harper (jackson@ximian.com)
//
// (C) Ximian, Inc.  http://www.ximian.com
// Copyright (C) 2004-2005 Novell, Inc (http://www.novell.com)
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

// Note about the ToString()'s. ECMA says there's only a ToString() method, 
// BUT it is just a wrapper for ToString(null). However there is no other ToString
// in the docs. Turning to the NET framework sdk reveals that there is a 
// ToString(formatprovider) method, as well as a 'static ToString (char c)' method, 
// which appears to just be a Convert.ToString(char c) type method. ECMA also
// doesn't list this class as implementing IFormattable.

using System.Globalization;
using System.Runtime.CompilerServices;
#if NET_2_0
using System.Runtime.InteropServices;
#endif

namespace System
{
    [Serializable]
#if NET_2_0
	public struct Char : IComparable, IConvertible, IComparable <char>, IEquatable <char>
#else
    public struct Char : IComparable, IConvertible
#endif
    {
        public const char MaxValue = (char)0xffff;
        public const char MinValue = (char)0;

        internal char m_value;

        public int CompareTo(object v)
        {
            if (v == null)
                return 1;

            if (!(v is Char))
                throw new ArgumentException(Locale.GetText("Value is not a System.Char"));

            char xv = (char)v;
            if (m_value == xv)
                return 0;

            if (m_value > xv)
                return 1;
            else
                return -1;
        }

        public override bool Equals(object o)
        {
            if (!(o is Char))
                return false;

            return ((char)o) == m_value;
        }

//#if NET_2_0
		public int CompareTo (char value)
		{
			if (m_value == value)
				return 0;

			if (m_value > value)
				return 1;
			else
				return -1;
		}

        public bool Equals (char value)
		{
			return m_value == value;
		}
//#endif

#if NET_2_0
		public static string ConvertFromUtf32 (int utf32)
		{
			if (utf32 < 0 || utf32 > 0x10FFFF)
				throw new ArgumentOutOfRangeException ("utf32", "The argument must be from 0 to 0x10FFFF.");
			if (0xD800 <= utf32 && utf32 <= 0xDFFF)
				throw new ArgumentOutOfRangeException ("utf32", "The argument must not be in surrogate pair range.");
			if (utf32 < 0x10000)
				return new string ((char) utf32, 1);
			utf32 -= 0x10000;
			return new string (
				new char [] {(char) ((utf32 >> 10) + 0xD800),
				(char) (utf32 % 0x0400 + 0xDC00)});
		}

		public static int ConvertToUtf32 (char highSurrogate, char lowSurrogate)
		{
			if (highSurrogate < 0xD800 || 0xDBFF < highSurrogate)
				throw new ArgumentOutOfRangeException ("highSurrogate");
			if (lowSurrogate < 0xDC00 || 0xDFFF < lowSurrogate)
				throw new ArgumentOutOfRangeException ("lowSurrogate");

			return 0x10000 + ((highSurrogate - 0xD800) << 10) + (lowSurrogate - 0xDC00);
		}

		public static int ConvertToUtf32 (string s, int index)
		{
			if (s == null)
				throw new ArgumentNullException ("s");
			if (index < 0 || index >= s.Length)
				throw new ArgumentOutOfRangeException ("index");
			if (!Char.IsSurrogate (s [index]))
				return s [index];
			if (!Char.IsHighSurrogate (s [index])
			    || index == s.Length - 1
			    || !Char.IsLowSurrogate (s [index + 1]))
				throw new ArgumentException (String.Format ("The string contains invalid surrogate pair character at {0}", index));
			return ConvertToUtf32 (s [index], s [index + 1]);
		}

		public static bool IsSurrogatePair (char high, char low)
		{
			return '\uD800' <= high && high <= '\uDBFF'
				&& '\uDC00' <= low && low <= '\uDFFF';
		}

		public static bool IsSurrogatePair (string s, int index)
		{
			if (s == null)
				throw new ArgumentNullException ("s");
			if (index < 0 || index >= s.Length)
				throw new ArgumentOutOfRangeException ("index");
			return index + 1 < s.Length && IsSurrogatePair (s [index], s [index + 1]);
		}
#endif

        public override int GetHashCode()
        {
            return m_value;
        }

        #region GetNumericValue
        #region Numeric Data
        private static int[] zeroData;
        private static int[] oneData;
        private static byte[] rangeData3882;
        private static byte[] rangeData4979;
        private static byte[] rangeData8531;
        private static double[] NumericDataValues;

        private static int GetNumericData(int pos)
        {
            if (pos >= 48 && pos < 58) // most common case '0'-'9'
                return pos - 48;

            if (zeroData == null)
            {
                zeroData = new int[]
                    {
                        1632, 10, 1776, 10, 2406, 10, 2534, 10, 2662, 10, 2790, 10, 2918, 10, 3174, 10, 3302, 10, 3430,
                        10,
                        3664, 10, 3792, 10, 3872, 10, 4160, 10, 6112, 10, 6160, 10, 8320, 10,
                    };
                oneData = new int[]
                    {
                        2548, 4, 3047, 10, 4969, 10, 8544, 12, 8560, 12, 9312, 20, 9332, 20, 9352, 20, 10102, 10, 10112,
                        10,
                        10122, 10, 12321, 9, 12690, 4, 12832, 10, 12928, 10
                    };
                rangeData3882 = new byte[] {33, 34, 35, 36, 37, 38, 39, 40, 41, 56};
                rangeData4979 = new byte[] {20, 21, 22, 23, 24, 25, 26, 27, 28, 32};
                rangeData8531 = new byte[] {42, 43, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 1};
            }

            int cnt;
            int index = 0;
            int zeroDataCount = zeroData.Length >> 1;
            for (cnt = 0; cnt < zeroDataCount; ++cnt)
            {
                if (pos >= zeroData[index] && pos < (zeroData[index] + zeroData[index + 1]))
                {
                    return pos - zeroData[index];
                }
                index += 2;
            }

            int oneDataCount = oneData.Length >> 1;
            index = 0;
            for (cnt = 0; cnt < oneDataCount; ++cnt)
            {
                if (pos >= oneData[index] && pos < (oneData[index] + oneData[index + 1]))
                {
                    return pos - oneData[index] + 1;
                }
                index += 2;
            }

            if (pos == 178)
                return 2;
            if (pos == 179)
                return 3;
            if (pos == 185)
                return 1;
            if (pos == 188)
                return 44;
            if (pos == 189)
                return 33;
            if (pos == 190)
                return 45;
            if (pos == 2553)
                return 16;
            if (pos == 3057)
                return 28;
            if (pos == 3058)
                return 30;
            if (pos >= 3882 && pos < 3892)
                return rangeData3882[pos - 3882];
            if (pos >= 4979 && pos < 4989)
                return rangeData4979[pos - 4979];
            if (pos >= 5870 && pos < 5873)
                return 17 + pos - 5870;
            if (pos == 8304)
                return 0;
            if (pos >= 8308 && pos < 8314)
                return 4 + (pos - 8308);
            if (pos >= 8531 && pos < 8544)
                return rangeData8531[pos - 8531];
            if (pos == 8556)
                return 23;
            if (pos >= 8557 && pos < 8560)
                return 28 + (pos - 8557);
            if (pos == 8572)
                return 23;
            if (pos == 8573)
                return 28;
            if (pos == 8574)
                return 29;
            if (pos == 8575)
                return 30;
            if (pos == 8576)
                return 30;
            if (pos == 8577)
                return 31;
            if (pos == 8578)
                return 32;
            if (pos == 9450)
                return 0;
            if (pos == 12295)
                return 0;
            if (pos == 12344)
                return 10;
            if (pos == 12345)
                return 20;
            if (pos == 12346)
                return 21;
            return 57;
        }
        #endregion

        public static double GetNumericValue(char c)
        {
            if (c > (char)0x3289)
            {
                if (c >= (char)0xFF10 && c <= (char)0xFF19)
                    return (c - 0xFF10); // Numbers 0-9

                // Default not set data
                return -1;
            }
            else
            {
                if (NumericDataValues == null)
                {
                    NumericDataValues = new double[]
                        {
                            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
                            10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
                            20, 30, 40, 50, 60, 70, 80, 90, 100, 500,
                            1000, 5000, 10000, 1.0 / 2.0, 3.0 / 2.0, 5.0 / 2.0, 7.0 / 2.0, 9.0 / 2.0, 11.0 / 2.0,
                            13.0 / 2.0,
                            15.0 / 2.0, 17.0 / 2.0, 1.0 / 3.0, 2.0 / 3.0, 1.0 / 4.0, 3.0 / 4.0, 1.0 / 5.0, 2.0 / 5.0,
                            3.0 / 5.0, 4.0 / 5.0,
                            1.0 / 6.0, 5.0 / 6.0, 1.0 / 8.0, 3.0 / 8.0, 5.0 / 8.0, 7.0 / 8.0, -1.0 / 2.0, -1.0
                        };
                }
                return NumericDataValues[GetNumericData(c)];
            }
        }

        public static double GetNumericValue(string str, int index)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            if (index < 0 || index >= str.Length)
                throw new ArgumentOutOfRangeException(Locale.GetText(
                    "The value of index is less than zero, or greater than or equal to the length of str."));

            return GetNumericValue(str[index]);
        }
        #endregion

        #region Unicode Category
        private static readonly byte[] categoryForLatin1 = new byte[]
            {
                14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
                14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
                11, 24, 24, 24, 26, 24, 24, 24, 20, 21, 24, 25, 24, 19, 24, 24,
                8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 24, 24, 25, 25, 25, 24,
                24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 24, 21, 27, 18,
                27, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 20, 25, 21, 25, 14,
                14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
                14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14,
                11, 24, 26, 26, 26, 26, 28, 28, 27, 28, 1, 22, 25, 19, 28, 27,
                28, 25, 10, 10, 27, 1, 28, 24, 27, 10, 1, 23, 10, 10, 10, 24,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 1, 25, 1, 1, 1, 1, 1, 1, 1, 1
            };

        private static bool IsLatin1(char ch)
        {
            return (ch <= '\x00ff');
        }

        private static UnicodeCategory GetLatin1UnicodeCategory(char ch)
        {
            return (UnicodeCategory)categoryForLatin1[ch];
        }

        public static UnicodeCategory GetUnicodeCategory(char ch)
        {
            if (IsLatin1(ch))
            {
                return GetLatin1UnicodeCategory(ch);
            }
            return CharUnicodeUnfo.GetUnicodeCategory(ch);
        }

        public static UnicodeCategory GetUnicodeCategory(string str, int index)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            if (index < 0 || index >= str.Length)
                throw new ArgumentOutOfRangeException(Locale.GetText(
                    "The value of index is less than zero, or greater than or equal to the length of str."));

            return GetUnicodeCategory(str[index]);
        }
        #endregion

        public static bool IsControl(char c)
        {
            return GetUnicodeCategory(c) == UnicodeCategory.Control;
        }

        public static bool IsControl(string str, int index)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            if (index < 0 || index >= str.Length)
                throw new ArgumentOutOfRangeException(Locale.GetText(
                    "The value of index is less than zero, or greater than or equal to the length of str."));

            return IsControl(str[index]);
        }

        public static bool IsDigit(char c)
        {
            if (c >= '0' && c <= '9')
                return true;
            return GetUnicodeCategory(c) == UnicodeCategory.DecimalDigitNumber;
        }

        public static bool IsDigit(string str, int index)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            if (index < 0 || index >= str.Length)
                throw new ArgumentOutOfRangeException(Locale.GetText(
                    "The value of index is less than zero, or greater than or equal to the length of str."));

            return IsDigit(str[index]);
        }

#if NET_2_0
        public static bool IsLowSurrogate (char c)
		{
			return c >= '\uDC00' && c <= '\uDFFF';
		}

		public static bool IsLowSurrogate (string s, int index)
		{
			if (s == null) 
				throw new ArgumentNullException ("s");
			
			if (index < 0 || index >= s.Length)
				throw new ArgumentOutOfRangeException ("index");
			
			return IsLowSurrogate (s [index]);
		}

		public static bool IsHighSurrogate (char c)
		{
			return c >= '\uD800' && c <= '\uDBFF';
		}

		public static bool IsHighSurrogate (string s, int index)
		{
			if (s == null) 
				throw new ArgumentNullException ("s");
			
			if (index < 0 || index >= s.Length)
				throw new ArgumentOutOfRangeException ("index");
			
			return IsHighSurrogate (s [index]);
		}
#endif

        public static bool IsLetter(char c)
        {
            //this is most frequent case
            if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                return true;
            return GetUnicodeCategory(c) <= UnicodeCategory.OtherLetter;
        }

        public static bool IsLetter(string str, int index)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            if (index < 0 || index >= str.Length)
                throw new ArgumentOutOfRangeException(Locale.GetText(
                    "The value of index is less than zero, or greater than or equal to the length of str."));

            return IsLetter(str[index]);
        }

        public static bool IsLetterOrDigit(char c)
        {
            UnicodeCategory Category = GetUnicodeCategory(c);
            switch (Category)
            {
                case UnicodeCategory.DecimalDigitNumber:
                case UnicodeCategory.UppercaseLetter:
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.TitlecaseLetter:
                case UnicodeCategory.ModifierLetter:
                case UnicodeCategory.OtherLetter:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsLetterOrDigit(string str, int index)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            if (index < 0 || index >= str.Length)
                throw new ArgumentOutOfRangeException(Locale.GetText(
                    "The value of index is less than zero, or greater than or equal to the length of str."));

            return IsLetterOrDigit(str[index]);
        }

        public static bool IsLower(char c)
        {
            return GetUnicodeCategory(c) == UnicodeCategory.LowercaseLetter;
        }

        public static bool IsLower(string str, int index)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            if (index < 0 || index >= str.Length)
                throw new ArgumentOutOfRangeException(Locale.GetText(
                    "The value of index is less than zero, or greater than or equal to the length of str."));

            return IsLower(str[index]);
        }

        public static bool IsNumber(char c)
        {
            UnicodeCategory Category = GetUnicodeCategory(c);
            switch (Category)
            {
                case UnicodeCategory.DecimalDigitNumber:
                case UnicodeCategory.LetterNumber:
                case UnicodeCategory.OtherNumber:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsNumber(string str, int index)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            if (index < 0 || index >= str.Length)
                throw new ArgumentOutOfRangeException(Locale.GetText(
                    "The value of index is less than zero, or greater than or equal to the length of str."));

            return IsNumber(str[index]);
        }

        public static bool IsPunctuation(char c)
        {
            UnicodeCategory Category = GetUnicodeCategory(c);
            switch (Category)
            {
                case UnicodeCategory.ConnectorPunctuation:
                case UnicodeCategory.DashPunctuation:
                case UnicodeCategory.OpenPunctuation:
                case UnicodeCategory.ClosePunctuation:
                case UnicodeCategory.InitialQuotePunctuation:
                case UnicodeCategory.FinalQuotePunctuation:
                case UnicodeCategory.OtherPunctuation:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsPunctuation(string str, int index)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            if (index < 0 || index >= str.Length)
                throw new ArgumentOutOfRangeException(Locale.GetText(
                    "The value of index is less than zero, or greater than or equal to the length of str."));

            return IsPunctuation(str[index]);
        }

        public static bool IsSeparator(char c)
        {
            UnicodeCategory Category = GetUnicodeCategory(c);
            switch (Category)
            {
                case UnicodeCategory.SpaceSeparator:
                case UnicodeCategory.LineSeparator:
                case UnicodeCategory.ParagraphSeparator:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsSeparator(string str, int index)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            if (index < 0 || index >= str.Length)
                throw new ArgumentOutOfRangeException(Locale.GetText(
                    "The value of index is less than zero, or greater than or equal to the length of str."));

            return IsSeparator(str[index]);
        }

        public static bool IsSurrogate(char c)
        {
            return GetUnicodeCategory(c) == UnicodeCategory.Surrogate;
        }

        public static bool IsSurrogate(string str, int index)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            if (index < 0 || index >= str.Length)
                throw new ArgumentOutOfRangeException(Locale.GetText(
                    "The value of index is less than zero, or greater than or equal to the length of str."));

            return IsSurrogate(str[index]);
        }

        public static bool IsSymbol(char c)
        {
            UnicodeCategory Category = GetUnicodeCategory(c);
            switch (Category)
            {
                case UnicodeCategory.MathSymbol:
                case UnicodeCategory.CurrencySymbol:
                case UnicodeCategory.ModifierSymbol:
                case UnicodeCategory.OtherSymbol:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsSymbol(string str, int index)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            if (index < 0 || index >= str.Length)
                throw new ArgumentOutOfRangeException(Locale.GetText(
                    "The value of index is less than zero, or greater than or equal to the length of str."));

            return IsSymbol(str[index]);
        }

        public static bool IsUpper(char c)
        {
            return GetUnicodeCategory(c) == UnicodeCategory.UppercaseLetter;
        }

        public static bool IsUpper(string str, int index)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            if (index < 0 || index >= str.Length)
                throw new ArgumentOutOfRangeException(Locale.GetText(
                    "The value of index is less than zero, or greater than or equal to the length of str."));

            return IsUpper(str[index]);
        }

        public static bool IsWhiteSpace(char c)
        {
            switch (c)
            {
                case (char)0x9:
                case (char)0x0a:
                case (char)0x0b:
                case (char)0x0c:
                case (char)0x0d:
                case (char)0x85: // NEL 
                case (char)0x2028: // Line Separator
                case (char)0x2029: // Paragraph Separator
//#if NET_2_0
				case (char)0x205F: // Medium Mathematical Space
//#endif
                    return true;

                default:
                    return GetUnicodeCategory(c) == UnicodeCategory.SpaceSeparator;
            }
        }

        public static bool IsWhiteSpace(string str, int index)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            if (index < 0 || index >= str.Length)
                throw new ArgumentOutOfRangeException(Locale.GetText(
                    "The value of index is less than zero, or greater than or equal to the length of str."));

            return IsWhiteSpace(str[index]);
        }

#if NET_2_0
		public static bool TryParse (string s, out char result)
		{
			if (s == null || s.Length != 1) {
				result = (char) 0;
				return false;
			}

			result = s [0];
			return true;
		}
#endif

        public static char Parse(string str)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            if (str.Length != 1)
                throw new FormatException("string contains more than one character.");

            return str[0];
        }

        public static char ToLower(char c)
        {
            return ToLower(c, CultureInfo.CurrentCulture);
        }

#if NET_2_0
		public static char ToLowerInvariant (char c)
#else
        internal static char ToLowerInvariant(char c)
#endif
        {
            return (char)Avm.String.fromCharCode(c).toLowerCase().charCodeAt(0);
        }

        public static char ToLower(char c, CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException("culture");
            if (culture.LCID == 0x007F) // Invariant
                return ToLowerInvariant(c);

            return culture.TextInfo.ToLower(c);
        }

        public static char ToUpper(char c)
        {
            return ToUpper(c, CultureInfo.CurrentCulture);
        }

#if NET_2_0
		public static char ToUpperInvariant (char c)
#else
        internal static char ToUpperInvariant(char c)
#endif
        {
            return (char)Avm.String.fromCharCode(c).toUpperCase().charCodeAt(0);
        }

        public static char ToUpper(char c, CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException("culture");
            if (culture.LCID == 0x007F) // Invariant
                return ToUpperInvariant(c);

            return culture.TextInfo.ToUpper(c);
        }

        public override string ToString()
        {
            // LAMESPEC: ECMA draft lists this as returning ToString (null), 
            // However it doesn't list another ToString() method.
            //return new String(m_value, 1);
            return Avm.String.fromCharCode(m_value);
        }

        public static string ToString(char c)
        {
            return new String(c, 1);
        }

        public string ToString(IFormatProvider fp)
        {
            // LAMESPEC: ECMA draft doesn't say Char implements IFormattable
            return new String(m_value, 1);
        }

        #region IConvertible Members
        public TypeCode GetTypeCode()
        {
            return TypeCode.Char;
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ToType(m_value, conversionType, provider);
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(m_value);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return m_value;
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(m_value);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(m_value);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(m_value);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(m_value);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(m_value);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(m_value);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(m_value);
        }
        #endregion
    }
}

