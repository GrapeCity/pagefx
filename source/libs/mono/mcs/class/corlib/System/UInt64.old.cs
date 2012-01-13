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

using System;
using System.Globalization;

namespace System
{
    //[Serializable]
    /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;T:System.UInt64&quot;]/*"/>
    [CLSCompliant(false)]
    public struct UInt64 : IComparable, IFormattable, IConvertible
    {
        public const ulong MaxValue = 0xffffffffffffffff;
        public const ulong MinValue = 0;

        public UInt32 m_hi;// !! don't change order since stack type is [hi,lo]
        public UInt32 m_lo;

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.#ctor(System.UInt32,System.UInt32)&quot;]/*"/>
        public UInt64(UInt32 hi, UInt32 lo)
        {
            m_hi = hi;
            m_lo = lo;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;P:System.UInt64.Hi&quot;]/*"/>
        public UInt32 Hi
        {
            //[System.Diagnostics.DebuggerStepThroughAttribute]
            get { return m_hi; }
            //[System.Diagnostics.DebuggerStepThroughAttribute]
            set { m_hi = value; }
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;P:System.UInt64.Lo&quot;]/*"/>
        public UInt32 Lo
        {
            //[System.Diagnostics.DebuggerStepThroughAttribute]
            get { return m_lo; }
            //[System.Diagnostics.DebuggerStepThroughAttribute]
            set { m_lo = value; }
        }

        #region Conversion Routines

        // ========= explicit and implicit conversion routines
#if NODDCORLIB
		public static implicit operator UInt64(ulong val) {
			return new UInt64((UInt32) val, (UInt32) (val >> 32));
		}
		public static explicit operator ulong(UInt64 val) {
			return (ulong) val.Hi << 32 | val.Lo;
		}
#endif
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Implicit(System.Int64)~System.UInt64&quot;]/*"/>
        public static implicit operator UInt64(long val)
        {
            return new UInt64(val.m_hi, val.m_lo);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Explicit(System.UInt64)~System.Int64&quot;]/*"/>
        public static explicit operator long(UInt64 val)
        {
            return new Int64(val.m_hi, val.m_lo);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Implicit(System.UInt32)~System.UInt64&quot;]/*"/>
        public static implicit operator UInt64(UInt32 val)
        {
            return new UInt64(0, val);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Explicit(System.UInt64)~System.UInt32&quot;]/*"/>
        public static explicit operator UInt32(UInt64 val)
        {
            return val.m_lo;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Implicit(System.Int32)~System.UInt64&quot;]/*"/>
        public static implicit operator UInt64(Int32 val)
        {
            return new UInt64(val < 0 ? 0xFFFFffff : 0, (UInt32)val);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Explicit(System.UInt64)~System.Int32&quot;]/*"/>
        public static explicit operator Int32(UInt64 val)
        {
            return (Int32)val.Lo;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Implicit(System.UInt16)~System.UInt64&quot;]/*"/>
        public static implicit operator UInt64(UInt16 val)
        {
            return new UInt64(0, val);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Explicit(System.UInt64)~System.UInt16&quot;]/*"/>
        public static explicit operator UInt16(UInt64 val)
        {
            return (UInt16)val.Lo;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Implicit(System.Int16)~System.UInt64&quot;]/*"/>
        public static implicit operator UInt64(Int16 val)
        {
            return new UInt64(val < 0 ? 0xFFFFffff : 0, (UInt32)val);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Explicit(System.UInt64)~System.Int16&quot;]/*"/>
        public static explicit operator Int16(UInt64 val)
        {
            return (Int16)val.Lo;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Implicit(System.Byte)~System.UInt64&quot;]/*"/>
        public static implicit operator UInt64(byte val)
        {
            return new UInt64(0, val);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Explicit(System.UInt64)~System.Byte&quot;]/*"/>
        public static explicit operator byte(UInt64 val)
        {
            return (byte)val.Lo;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Implicit(System.SByte)~System.UInt64&quot;]/*"/>
        public static implicit operator UInt64(sbyte val)
        {
            return new UInt64(val < 0 ? 0xFFFFffff : 0, (UInt32)val);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Explicit(System.UInt64)~System.SByte&quot;]/*"/>
        public static explicit operator sbyte(UInt64 val)
        {
            return (sbyte)val.Lo;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Implicit(System.Single)~System.UInt64&quot;]/*"/>
        public static implicit operator UInt64(Single val)
        {
            UInt32 hi = (UInt32)(val / 4294967296.0);
            UInt32 lo = (UInt32)(val - 4294967296.0 * hi);
            return new UInt64(hi, lo);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Explicit(System.UInt64)~System.Single&quot;]/*"/>
        public static explicit operator Single(UInt64 val)
        {
            double d = 4294967296.0 * val.Hi + val.Lo;
            return (Single)d;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Implicit(System.Double)~System.UInt64&quot;]/*"/>
        public static implicit operator UInt64(Double val)
        {
            UInt32 hi = (UInt32)(val / 4294967296.0);
            UInt32 lo = (UInt32)(val - 4294967296.0 * hi);
            return new UInt64(hi, lo);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Explicit(System.UInt64)~System.Double&quot;]/*"/>
        public static explicit operator Double(UInt64 val)
        {
            double d = 4294967296.0 * val.Hi + val.Lo;
            return d;
        }

#if false
		public static implicit operator UInt64(Decimal val) {
			int[] bits = Decimal.GetBits(val);
			return new UInt64((UInt32) bits[0], (UInt32) bits[1]);
		}
#endif
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Explicit(System.UInt64)~System.Decimal&quot;]/*"/>
        public static explicit operator Decimal(UInt64 val)
        {
            return new Decimal((Int32)val.Lo, (Int32)val.Hi, 0, false, 0);
        }

        //		public static implicit operator UInt64(Boolean val) {
        //			return new UInt64(val ? (UInt32) 1 : 0, 0);
        //		}
        //		public static explicit operator Boolean(UInt64 val) {
        //			return val.Lo != 0 || val.Hi != 0;
        //		}
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Implicit(System.Char)~System.UInt64&quot;]/*"/>
        public static implicit operator UInt64(Char val)
        {
            return new UInt64(0, val);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Explicit(System.UInt64)~System.Char&quot;]/*"/>
        public static explicit operator Char(UInt64 val)
        {
            return (Char)val.Lo;
        }
        #endregion Conversion Routines

        #region UInt64 interface methods
        // ========= UInt64 interface methods ===============

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.CompareTo(System.Object)&quot;]/*"/>
        public int CompareTo(object value)
        {
            /* DDUInt64 */
            UInt64 value2;

            if (value == null)
                return 1;

            if (!(value is /* DDUInt64 */ UInt64))
                throw new ArgumentException(Locale.GetText("Value is not a System.UInt64."));

            value2 = (/* DDUInt64 */ UInt64)value;
            if (this.m_hi == value2.m_hi && this.m_lo == value2.m_lo)
                return 0;
            if (this.m_hi < value2.m_hi)
                return -1;
            if (this.m_hi > value2.m_hi)
                return 1;

            if (this.m_lo < value2.m_lo)
                return -1;
            if (this.m_lo > value2.m_lo)
                return 1;
            return 0;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.Equals(System.Object)&quot;]/*"/>
        public override bool Equals(object obj)
        {
            try
            {
                UInt64 val = (UInt64)obj;
                return this.m_hi == val.m_hi && this.m_lo == val.m_lo;
            }
            catch (InvalidCastException)
            {
                return false;
            }
#if false 
// Bug work around: 'is' test was failing
// see cr 17493
			if (!(obj is UInt64))
				return false;

			UInt64 val = (UInt64) obj;
			return this.m_hi == val.m_hi && this.m_lo == val.m_lo;
#endif
        }

        internal static bool opEquality(Int64 d1, UInt64 d2)
        {
            if (d1.m_lo != d2.m_lo)
                return false;
            if (d1.m_hi != d2.m_hi)
                return false;
            return true;
        }

        internal static bool opEquality(UInt64 d1, Int64 d2)
        {
            if (d1.m_lo != d2.m_lo)
                return false;
            if (d1.m_hi != d2.m_hi)
                return false;
            return true;
        }


        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.GetHashCode&quot;]/*"/>
        public override int GetHashCode()
        {
            return (int)(m_lo ^ m_hi);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.Parse(System.String)&quot;]/*"/>
        [CLSCompliant(false)]
        public static /* DDUInt64 */ UInt64 Parse(string s)
        {
            return Parse(s, NumberStyles.Integer, null);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.Parse(System.String,System.IFormatProvider)&quot;]/*"/>
        [CLSCompliant(false)]
        public static /* DDUInt64 */ UInt64 Parse(string s, IFormatProvider provider)
        {
            return Parse(s, NumberStyles.Integer, provider);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.Parse(System.String,System.Globalization.NumberStyles)&quot;]/*"/>
        [CLSCompliant(false)]
        public static /* DDUInt64 */ UInt64 Parse(string s, NumberStyles style)
        {
            return Parse(s, style, null);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.Parse(System.String,System.Globalization.NumberStyles,System.IFormatProvider)&quot;]/*"/>
        [CLSCompliant(false)]
        public static /* DDUInt64 */ UInt64 Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            if (s == null)
                throw new ArgumentNullException("s");

            if (s.Length == 0)
                throw new FormatException(Locale.GetText("Input string was not in the correct format."));

            NumberFormatInfo nfi;
            if (provider != null)
            {
                Type typeNFI = typeof(NumberFormatInfo);
                nfi = (NumberFormatInfo)provider.GetFormat(typeNFI);
            }
            else
            {
                //TODO
                //nfi = Thread.CurrentThread.CurrentCulture.NumberFormat;
                nfi = new NumberFormatInfo();
            }

            CheckStyle(style);

            bool AllowCurrencySymbol = (style & NumberStyles.AllowCurrencySymbol) != 0;
            bool AllowExponent = (style & NumberStyles.AllowExponent) != 0;
            bool AllowHexSpecifier = (style & NumberStyles.AllowHexSpecifier) != 0;
            bool AllowThousands = (style & NumberStyles.AllowThousands) != 0;
            bool AllowDecimalPoint = (style & NumberStyles.AllowDecimalPoint) != 0;
            bool AllowParentheses = (style & NumberStyles.AllowParentheses) != 0;
            bool AllowTrailingSign = (style & NumberStyles.AllowTrailingSign) != 0;
            bool AllowLeadingSign = (style & NumberStyles.AllowLeadingSign) != 0;
            bool AllowTrailingWhite = (style & NumberStyles.AllowTrailingWhite) != 0;
            bool AllowLeadingWhite = (style & NumberStyles.AllowLeadingWhite) != 0;

            int pos = 0;

            if (AllowLeadingWhite)
                pos = JumpOverWhite(pos, s, true);

            bool foundOpenParentheses = false;
            bool negative = false;
            bool foundSign = false;
            bool foundCurrency = false;

            // Pre-number stuff
            if (AllowParentheses && s[pos] == '(')
            {
                foundOpenParentheses = true;
                foundSign = true;
                negative = true; // MS always make the number negative when there parentheses
                // even when NumberFormatInfo.NumberNegativePattern != 0!!!
                pos++;
                if (AllowLeadingWhite)
                    pos = JumpOverWhite(pos, s, true);

                if (s.Substring(pos, nfi.NegativeSign.Length) == nfi.NegativeSign)
                    throw new FormatException(Locale.GetText("Input string was not in the correct format."));
                if (s.Substring(pos, nfi.PositiveSign.Length) == nfi.PositiveSign)
                    throw new FormatException(Locale.GetText("Input string was not in the correct format."));
            }

            if (AllowLeadingSign && !foundSign)
            {
                // Sign + Currency
                FindSign(ref pos, s, nfi, ref foundSign, ref negative);
                if (foundSign)
                {
                    if (AllowLeadingWhite)
                        pos = JumpOverWhite(pos, s, true);
                    if (AllowCurrencySymbol)
                    {
                        FindCurrency(ref pos, s, nfi,
                            ref foundCurrency);
                        if (foundCurrency && AllowLeadingWhite)
                            pos = JumpOverWhite(pos, s, true);
                    }
                }
            }

            if (AllowCurrencySymbol && !foundCurrency)
            {
                // Currency + sign
                FindCurrency(ref pos, s, nfi, ref foundCurrency);
                if (foundCurrency)
                {
                    if (AllowLeadingWhite)
                        pos = JumpOverWhite(pos, s, true);
                    if (foundCurrency)
                    {
                        if (!foundSign && AllowLeadingSign)
                        {
                            FindSign(ref pos, s, nfi, ref foundSign,
                                ref negative);
                            if (foundSign && AllowLeadingWhite)
                                pos = JumpOverWhite(pos, s, true);
                        }
                    }
                }
            }

            UInt32 lo = 0;
            UInt32 hi = 0;
            int nDigits = 0;
            bool decimalPointFound = false;
            UInt32 digitValue;
            char hexDigit;

            // Number stuff
            // Just the same as Int32, but this one adds instead of substract
            do
            {

                if (!Int32.ValidDigit(s[pos], AllowHexSpecifier))
                {
                    if (AllowThousands && FindOther(ref pos, s, nfi.NumberGroupSeparator))
                        continue;
                    else
                        if (!decimalPointFound && AllowDecimalPoint &&
                        FindOther(ref pos, s, nfi.NumberDecimalSeparator))
                        {
                            decimalPointFound = true;
                            continue;
                        }
                    break;
                }
                else if (AllowHexSpecifier)
                {
                    nDigits++;
                    hexDigit = s[pos++];
                    if (Char.IsDigit(hexDigit))
                        digitValue = (uint)(hexDigit - '0');
                    else if (Char.IsLower(hexDigit))
                        digitValue = (uint)(hexDigit - 'a' + 10);
                    else
                        digitValue = (uint)(hexDigit - 'A' + 10);
                    Mult64by16to64(lo, hi, 16, out lo, out hi);
                    Add64(lo, hi, digitValue, 0, out lo, out hi);
                    //number = checked (number * 16 + digitValue);
                }
                else if (decimalPointFound)
                {
                    nDigits++;
                    // Allows decimal point as long as it's only 
                    // followed by zeroes.
                    if (s[pos++] != '0')
                        throw new OverflowException(Locale.GetText("Value too large or too small."));
                }
                else
                {
                    nDigits++;
                    digitValue = (uint)(s[pos++] - '0');

                    Mult64by16to64(lo, hi, 10, out lo, out hi);
                    Add64(lo, hi, digitValue, 0, out lo, out hi);
                }
            } while (pos < s.Length);

            // Post number stuff
            if (nDigits == 0)
                throw new FormatException(Locale.GetText("Input string was not in the correct format."));

            if (AllowTrailingSign && !foundSign)
            {
                // Sign + Currency
                FindSign(ref pos, s, nfi, ref foundSign, ref negative);
                if (foundSign)
                {
                    if (AllowTrailingWhite)
                        pos = JumpOverWhite(pos, s, true);
                    if (AllowCurrencySymbol)
                        FindCurrency(ref pos, s, nfi, ref foundCurrency);
                }
            }

            if (AllowCurrencySymbol && !foundCurrency)
            {
                // Currency + sign
                FindCurrency(ref pos, s, nfi, ref foundCurrency);
                if (foundCurrency)
                {
                    if (AllowTrailingWhite)
                        pos = JumpOverWhite(pos, s, true);
                    if (!foundSign && AllowTrailingSign)
                        FindSign(ref pos, s, nfi, ref foundSign, ref negative);
                }
            }

            if (AllowTrailingWhite && pos < s.Length)
                pos = JumpOverWhite(pos, s, false);

            if (foundOpenParentheses)
            {
                if (pos >= s.Length || s[pos++] != ')')
                    throw new FormatException(Locale.GetText
                        ("Input string was not in the correct format."));
                if (AllowTrailingWhite && pos < s.Length)
                    pos = JumpOverWhite(pos, s, false);
            }

            if (pos < s.Length && s[pos] != '\u0000')
                throw new FormatException(Locale.GetText("Input string was not in the correct format."));

            // -0 is legal but other negative values are not
            if (negative && (lo != 0 || hi != 0))
            {
                throw new OverflowException(
                    Locale.GetText("Negative number"));
            }

            return new /* DDUInt64 */ UInt64(hi, lo);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.ToString&quot;]/*"/>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.ToString(System.IFormatProvider)&quot;]/*"/>
        public string ToString(IFormatProvider provider)
        {
            return ToString(null, provider);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.ToString(System.String)&quot;]/*"/>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.ToString(System.String,System.IFormatProvider)&quot;]/*"/>
        public string ToString(string format, IFormatProvider provider)
        {
            if (format != null && (format.StartsWith("d") || format.StartsWith("D")))
                format = "G" + format.Substring(1);
            Decimal d = new Decimal((int)m_lo, (int)m_hi, 0, false, 0);
            String s = d.ToString(format, provider);
            return s;
#if false
			NumberFormatInfo nfi = NumberFormatInfo.GetInstance (provider);

			// use "G" when format is null or String.Empty
			if ((format == null) || (format.Length == 0))
				format = "G";
			return NumberFormatter.NumberToString (format, nfi, value__);
#endif
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.DumpHex&quot;]/*"/>
        public string DumpHex()
        {
            return UInt64.DumpHex(m_lo, m_hi);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.DumpHex(System.UInt32,System.UInt32)&quot;]/*"/>
        public static string DumpHex(UInt32 lo, UInt32 hi)
        {
            //Console.WriteLine ("in DumpHex: '0x{0}:{1}'", hi, lo);
            String s;
            //s = "0x" + m_hi.ToString("x") + ":" + m_lo.ToString("x");
            Decimal dhi = hi, dlo = lo;
            s = "0x" + dhi.ToString("x") + ":" + dlo.ToString("x");
            return s;
        }
        #endregion UInt64 interface methods

        #region IConvertible
        // =========== IConvertible Methods =========== //
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.GetTypeCode&quot;]/*"/>
        internal static System.TypeCode __typeCode = TypeCode.UInt64;
        public TypeCode GetTypeCode()
        {
            return TypeCode.UInt64;
        }

        internal UInt64 boxU()
        {
            return this;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return (m_hi > 0 || m_lo > 0);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            if (m_hi != 0 || m_lo > 0xFF)
                throw new OverflowException("value was too large for an unsigned byte.");
            return System.Convert.ToByte(m_lo);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            if (m_hi != 0 || m_lo > 0xFFFF)
                throw new OverflowException("value was too large for a character.");
            return System.Convert.ToChar(m_lo);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return System.Convert.ToDateTime(m_lo);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return new Decimal((int)m_lo, (int)m_hi, 0, false, 0);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            double d = 4294967296.0 * m_hi + m_lo;
            return d;
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            if (m_hi != 0 || m_lo >= 0x8000)
                throw new OverflowException("value was too large for an Int16.");
            return System.Convert.ToInt16(m_lo);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            if (m_hi != 0 || m_lo == 0x80000000)
                throw new OverflowException("value was too large for an Int32.");
            return System.Convert.ToInt32(m_lo);
        }
#if NODDCORLIB
		long IConvertible.ToInt64 (IFormatProvider provider) {
			long val = (long) (ulong) m_hi << 32 | m_lo;
			return System.Convert.ToInt64 (val);
		}

#else
        Int64 IConvertible.ToInt64(IFormatProvider provider)
        {
            if (m_hi >= 0x80000000)
                throw new OverflowException("value was too large for an Int64.");
            return (Int64)((UInt64)m_hi << 32 | m_lo);
            //return new Int64(m_lo, m_hi);
        }
#endif

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            if (m_hi != 0 || m_lo >= 0x80)
                throw new OverflowException("value was too large for a signed byte.");
            return System.Convert.ToSByte(m_lo);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            Double d = 4294967296.0 * m_hi + m_lo;
            Single s = (Single)d;
            return s;
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            //return System.Convert.ToType (value__, conversionType, provider);
            return null;
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            if (m_hi != 0 || m_lo > 0xFFFF)
                throw new OverflowException("value was too large for a UInt16.");
            return System.Convert.ToUInt16(m_lo);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            if (m_hi != 0)
                throw new OverflowException("value was too large for a UInt32.");
            return System.Convert.ToUInt32(m_lo);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            ulong u = (ulong)m_hi << 32 | m_lo;
            return u;
            //return new UInt64(m_lo, m_hi);
        }
        #endregion IConvertible

        #region Parsing routines from Int32
        // ========== internal routines from Int32 ===============
        internal static void CheckStyle(NumberStyles style)
        {
            if ((style & NumberStyles.AllowHexSpecifier) != 0)
            {
                NumberStyles ne = style ^ NumberStyles.AllowHexSpecifier;
                if ((ne & NumberStyles.AllowLeadingWhite) != 0)
                    ne ^= NumberStyles.AllowLeadingWhite;
                if ((ne & NumberStyles.AllowTrailingWhite) != 0)
                    ne ^= NumberStyles.AllowTrailingWhite;
                if (ne != 0)
                    throw new ArgumentException(
                        "With AllowHexSpecifier only " +
                        "AllowLeadingWhite and AllowTrailingWhite " +
                        "are permitted.");
            }
        }

        internal static int JumpOverWhite(int pos, string s, bool excp)
        {
            while (pos < s.Length && Char.IsWhiteSpace(s[pos]))
                pos++;

            if (excp && pos >= s.Length)
                throw new FormatException("Input string was not in the correct format.");

            return pos;
        }

        internal static void FindSign(ref int pos, string s, NumberFormatInfo nfi,
            ref bool foundSign, ref bool negative)
        {
            if ((pos + nfi.NegativeSign.Length) <= s.Length &&
                s.Substring(pos, nfi.NegativeSign.Length) == nfi.NegativeSign)
            {
                negative = true;
                foundSign = true;
                pos += nfi.NegativeSign.Length;
            }
            else if ((pos + nfi.PositiveSign.Length) < s.Length &&
                s.Substring(pos, nfi.PositiveSign.Length) == nfi.PositiveSign)
            {
                negative = false;
                pos += nfi.PositiveSign.Length;
                foundSign = true;
            }
        }

        internal static void FindCurrency(ref int pos,
            string s,
            NumberFormatInfo nfi,
            ref bool foundCurrency)
        {
            if ((pos + nfi.CurrencySymbol.Length) <= s.Length &&
                s.Substring(pos, nfi.CurrencySymbol.Length) == nfi.CurrencySymbol)
            {
                foundCurrency = true;
                pos += nfi.CurrencySymbol.Length;
            }
        }

        internal static bool FindExponent(ref int pos, string s)
        {
            int i = s.IndexOfAny(new char[] { 'e', 'E' }, pos);
            if (i < 0)
                return false;
            if (++i == s.Length)
                return false;
            if (s[i] == '+' || s[i] == '-')
                if (++i == s.Length)
                    return false;
            if (!Char.IsDigit(s[i]))
                return false;
            for (; i < s.Length; ++i)
                if (!Char.IsDigit(s[i]))
                    break;
            pos = i;
            return true;
        }

        internal static bool FindOther(ref int pos,
            string s,
            string other)
        {
            if ((pos + other.Length) <= s.Length &&
                s.Substring(pos, other.Length) == other)
            {
                pos += other.Length;
                return true;
            }

            return false;
        }

        #endregion Int32 parsing routines

        #region Internal Math Routines
        // ================== Internal Math Routines =====================

        internal static void Mult64by16to64(UInt32 alo, UInt32 ahi, UInt16 b, out UInt32 clo, out UInt32 chi)
        {
            UInt32 val, mid, carry0, carry1;
            UInt16 a0, a1, a2, a3;
            UInt16 h0, h1, h2, h3, h4;
            a0 = (UInt16)alo;
            a1 = (UInt16)(alo >> 16);
            a2 = (UInt16)ahi;
            a3 = (UInt16)(ahi >> 16);

            val = ((UInt32)a0) * b;
            h0 = (UInt16)val;

            val >>= 16; carry0 = 0;
            mid = ((UInt32)a1) * b;
            val += mid; if (val < mid) ++carry0;
            h1 = (UInt16)val;

            val >>= 16; carry1 = 0;
            mid = ((UInt32)a2) * b;
            val += mid; if (val < mid) ++carry1;
            h2 = (UInt16)val;

            val >>= 16; val += carry0;
            mid = ((UInt32)a3) * b;
            val += mid;
            h3 = (UInt16)val;

            val >>= 16; val += carry1;
            h4 = (UInt16)val;
            if (h4 != 0)
                throw new OverflowException(Locale.GetText("Overflow on adding decimal number"));

            clo = ((UInt32)h1) << 16 | h0;
            chi = ((UInt32)h3) << 16 | h2;

        }

        internal static void Mult32by32to64(UInt32 a, UInt32 b, out UInt32 clo, out UInt32 chi)
        {
            Mult32by32to64((UInt16)a, (UInt16)(a >> 16), (UInt16)b, (UInt16)(b >> 16),
                out clo, out chi);
        }
        private static void Mult32by32to64(UInt16 alo, UInt16 ahi, UInt16 blo, UInt16 bhi,
            out UInt32 clo, out UInt32 chi)
        {

            UInt32 a, b, c, d;
            UInt16 h0, h1, h2, h3, carry;

            a = ((UInt32)alo) * blo;
            h0 = (UInt16)a;

            //Console.WriteLine("Mult32by32to64 alo={0} blo={1} a={2}",alo,blo,a);

            a >>= 16;
            carry = 0;
            b = ((UInt32)alo) * bhi;
            c = ((UInt32)ahi) * blo;
            a += b; if (a < b) ++carry;
            a += c; if (a < c) ++carry;
            h1 = (UInt16)a;

            a >>= 16;
            d = ((UInt32)ahi) * bhi;
            a += d;
            h2 = (UInt16)a;

            a >>= 16;
            a += carry;
            h3 = (UInt16)a;

            clo = ((UInt32)h1) << 16 | h0;
            chi = ((UInt32)h3) << 16 | h2;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.Mult64by64to128(System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32)&quot;]/*"/>
        internal static Int128 Mult64by64to128(UInt32 alo, UInt32 ahi, UInt32 blo, UInt32 bhi)
        {
            UInt32 p1_lo, p1_hi, p2_lo, p2_hi, p3_lo, p3_hi, p4_lo, p4_hi;
            UInt32 sum;
            UInt32 carry0, carry1;

            Int128 result = new Int128();

            Mult32by32to64(alo, blo, out p1_lo, out p1_hi);

            //Console.WriteLine("Mult 64by64to128 alo={0} blo={1} p1_lo={2} p1_hi={3}",alo,blo,p1_lo,p1_hi);

            result.c0 = p1_lo;

            sum = p1_hi; carry0 = 0;
            Mult32by32to64(ahi, blo, out p2_lo, out p2_hi);
            Mult32by32to64(alo, bhi, out p3_lo, out p3_hi);
            sum += p2_lo; if (sum < p2_lo) ++carry0;
            sum += p3_lo; if (sum < p3_lo) ++carry0;
            result.c1 = sum;


            sum = p2_hi; carry1 = 0;
            Mult32by32to64(ahi, bhi, out p4_lo, out p4_hi);
            sum += carry0; if (sum < carry0) ++carry1;
            sum += p3_hi; if (sum < p3_hi) ++carry1;
            sum += p4_lo; if (sum < p4_hi) ++carry1;
            result.c2 = sum;

            sum = p4_hi;
            sum += carry1;
            result.c3 = sum;
            return result;
        }

        internal static UInt32 Add64(UInt32 alo, UInt32 ahi, UInt32 blo, UInt32 bhi,
            out UInt32 clo, out UInt32 chi)
        {
            UInt32 carry;
            alo += blo;
            if (alo < blo) ahi++;
            ahi += bhi;
            carry = (ahi < bhi) ? (UInt32)1 : 0;
            clo = alo;
            chi = ahi;
            return carry;
        }

        internal static UInt32 Add64Ovf(UInt32 alo, UInt32 ahi, UInt32 blo, UInt32 bhi,
            out UInt32 clo, out UInt32 chi)
        {
            UInt32 carry;

            alo = alo + blo;
            if (alo < blo)
                ahi = ahi + 1; /* carry */
            ahi = ahi + bhi;
            carry = (ahi < bhi) ? (UInt32)1 : 0;

            if (carry == 1)
                throw new OverflowException();

            clo = alo;
            chi = ahi;
            return carry;
        }

        internal static void Add64OvfSigned(UInt32 alo, UInt32 ahi, UInt32 blo, UInt32 bhi,
            out UInt32 clo, out UInt32 chi)
        {
            uint isNeg1 = ahi & 0x80000000;
            uint isNeg2 = bhi & 0x80000000;
            if (isNeg1 == 0 && isNeg2 == 0)
            {
                alo = alo + blo;
                if (alo < blo)
                    ahi = ahi + 1; /* carry */

                ahi = ahi + bhi;

                if (ahi < bhi)
                    throw new OverflowException();
                if ((ahi & 0x80000000) != 0)
                {
                    // result is negative although input was positive
                    throw new OverflowException();
                }
                clo = alo;
                chi = ahi;
            }
            else
            {
                alo = alo + blo;
                if (alo < blo)
                    ahi = ahi + 1; /* carry */

                ahi = ahi + bhi;

                if (ahi < bhi && (isNeg1 != 0 && isNeg2 != 0))
                    throw new OverflowException();

                clo = alo;
                chi = ahi;
            }
        }

        internal static void Sub64(UInt32 alo, UInt32 ahi, UInt32 blo, UInt32 bhi,
            out UInt32 clo, out UInt32 chi)
        {
            UInt32 dlo, dhi;
            dlo = alo - blo;
            if (alo < blo) --ahi;	// Borrow
            dhi = ahi - bhi;

            //Console.WriteLine("     Sub64 res {0}.{1}",dlo,dhi);
            clo = dlo;
            chi = dhi;
        }

        internal static void Sub64Ovf(UInt32 alo, UInt32 ahi, UInt32 blo, UInt32 bhi, out UInt32 clo, out UInt32 chi)
        {
            UInt32 dlo, dhi;
            //dlo = Avm.Global.SubUncheckedUInt32(alo, blo);
            dlo = alo - blo;
            if (alo < blo)
            {
                //ahi = Avm.Global.SubUncheckedUInt32(ahi, 1);// Borrow
                ahi = ahi - 1;// Borrow
            }
            //dhi = Avm.Global.SubUncheckedUInt32(ahi, bhi);
            dhi = ahi - bhi;

            //if (Avm.Global.Int32Ovf(dhi))
            //    throw new OverflowException();

            //Console.WriteLine("     Sub64 res {0}.{1}",dlo,dhi);
            clo = dlo;
            chi = dhi;
        }

        private static Int32 Compare(/* DDUInt64 */ UInt64 a, /* DDUInt64 */ UInt64 b)
        {
            if (a.Hi < b.Hi) return -1;
            if (a.Hi > b.Hi) return 1;
            if (a.Lo < b.Lo) return -1;
            if (a.Lo > b.Lo) return 1;
            return 0;
        }
        private static Int32 Compare(UInt32 alo, UInt32 ahi, UInt32 blo, UInt32 bhi)
        {
            if (ahi < bhi) return -1;
            if (ahi > bhi) return 1;
            if (alo < blo) return -1;
            if (alo > blo) return 1;
            return 0;
        }
        internal static bool div64by16(UInt32 plo, UInt32 phi, UInt32 factor,
            out UInt32 qlo, out UInt32 qhi, out UInt32 pRest)
        {

            UInt32 a, b, c;

            a = (UInt16)(phi >> 16);
            b = a / factor;
            a -= b * factor;
            a <<= 16;
            a |= (UInt16)phi;
            c = a / factor;
            a -= c * factor;
            a <<= 16;
            qhi = b << 16 | (UInt16)c;

            a |= (UInt16)(plo >> 16);
            b = a / factor;
            a -= b * factor;
            a <<= 16;
            a |= (UInt16)plo;
            c = a / factor;
            a -= c * factor;
            qlo = b << 16 | (UInt16)c;

            pRest = (UInt32)(UInt16)a;

            a <<= 1;
            return (a >= factor || (a == factor && (c & 1) == 1)) ? true : false;
        }
        private static UInt32 lshift(ref UInt32 alo, ref UInt32 ahi)
        {
            UInt32 carry0 = (alo >> 31);
            UInt32 carry1 = (ahi >> 31);
            alo <<= 1;
            ahi <<= 1;
            ahi += carry0;
            return carry1;
        }
        private static void rshift(ref UInt32 alo, ref UInt32 ahi)
        {
            UInt32 carry = (ahi & 1) << 31;
            ahi >>= 1;
            alo = (alo >> 1) | carry;
        }
        /// <summary>
        /// Divides two 64 bits numbers returning both quotient and remainder
        /// </summary>
        /// <param name="alo">low 32 bits of dividend</param>
        /// <param name="ahi">high 32 bits of dividend</param>
        /// <param name="blo">low 32 bits of divisor</param>
        /// <param name="bhi">high 32 bits of divisor</param>
        /// <param name="pQlo">low 32 bits of quotient</param>
        /// <param name="pQhi">high 32 bits of quotient</param>
        /// <param name="pRlo">low 32 bits of remainder</param>
        /// <param name="pRhi">high 32 bits of remainder</param>
        internal static void Div64by64(UInt32 alo, UInt32 ahi, UInt32 blo, UInt32 bhi,
            out UInt32 pQlo, out UInt32 pQhi, out UInt32 pRlo, out UInt32 pRhi)
        {

            //			Console.WriteLine ("In Div64by64 {0} / {1}", 
            //				UInt64.DumpHex(alo, ahi), UInt64.DumpHex (blo, bhi));
            UInt32 dlo = 0, dhi = 0; // High 64 bits of dividend
            UInt32 qlo = 0, qhi = 0; // Quotient
            UInt32 carry;
            Int32 cmp;

            for (int j = 0; j < 64; ++j)
            {
                // loop 64 times
                //     D.A (128 bits) <<= 1
                //     quotient <<= 1
                //     if D >= B then D -= B, Q |= 1

                carry = lshift(ref alo, ref ahi); // Do 128 bit shift of D & A
                lshift(ref dlo, ref dhi);
                dlo += carry;
                lshift(ref qlo, ref qhi);

                cmp = Compare(dlo, dhi, blo, bhi); // Dividend bigger than divisor?
                if (cmp >= 0)
                {
                    Sub64(dlo, dhi, blo, bhi, out dlo, out dhi);
                    qlo += 1;
                }
            }
            pQlo = qlo;
            pQhi = qhi;
            pRlo = dlo;
            pRhi = dhi;
        }
        #endregion Internal Math Routines

        #region Operators
        // ============== Operators =================
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Addition(System.UInt64,System.UInt64)&quot;]/*"/>
        public static /* DDUInt64 */ UInt64 operator +(/* DDUInt64 */ UInt64 d1, /* DDUInt64 */ UInt64 d2)
        {
            Add64(d1.Lo, d1.Hi, d2.Lo, d2.Hi, out d1.m_lo, out d1.m_hi);
            return d1;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Increment(System.UInt64)&quot;]/*"/>
        public static /* DDUInt64 */ UInt64 operator ++(/* DDUInt64 */ UInt64 d1)
        {
            /* DDUInt64 */
            UInt64 d2 = new /* DDUInt64 */ UInt64(0, 1);
            Add64(d1.Lo, d1.Hi, d2.Lo, d2.Hi, out d1.m_lo, out d1.m_hi);
            return d1;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Addition(System.UInt64)&quot;]/*"/>
        public static /* DDUInt64 */ UInt64 operator +(/* DDUInt64 */ UInt64 d)
        {
            return d;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Subtraction(System.UInt64,System.UInt64)&quot;]/*"/>
        public static /* DDUInt64 */ UInt64 operator -(/* DDUInt64 */ UInt64 d1, /* DDUInt64 */ UInt64 d2)
        {
            UInt64 res;
            Sub64(d1.Lo, d1.Hi, d2.Lo, d2.Hi, out res.m_lo, out res.m_hi);
            return res;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Decrement(System.UInt64)&quot;]/*"/>
        public static /* DDUInt64 */ UInt64 operator --(/* DDUInt64 */ UInt64 d1)
        {
            /* DDUInt64 */
            UInt64 d2 = new /* DDUInt64 */ UInt64(0, 1);
            UInt64 res;
            Sub64(d1.Lo, d1.Hi, d2.Lo, d2.Hi, out res.m_lo, out res.m_hi);
            return res;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Multiply(System.UInt64,System.UInt64)&quot;]/*"/>
        public static /* DDUInt64 */ UInt64 operator *(/* DDUInt64 */ UInt64 d1, /* DDUInt64 */ UInt64 d2)
        {
            Int128 result = Mult64by64to128(d1.Lo, d1.Hi, d2.Lo, d2.Hi);
            return new UInt64(result.c1, result.c0);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Division(System.UInt64,System.UInt64)&quot;]/*"/>
        public static /* DDUInt64 */ UInt64 operator /(/* DDUInt64 */ UInt64 d1, /* DDUInt64 */ UInt64 d2)
        {
            /* DDUInt64 */
            UInt64 result;
            UInt32 qlo, qhi, rlo, rhi, rest;

            //Console.WriteLine ("in UInt64 operator /: {0} / {1}", d1.DumpHex(), d2.DumpHex());
            if (d2.Lo == 0 && d2.Hi == 0)
                throw new DivideByZeroException();
            if (d1.Lo == 0 && d1.Hi == 0)
                return new /* DDUInt64 */ UInt64(0, 0);
            if (d1.Lo == d2.Lo && d1.Hi == d2.Hi)
                return new /* DDUInt64 */ UInt64(0, 1);
            if (d2.Hi == 0 && d2.Lo == 1)
                return new /* DDUInt64 */ UInt64(d1.Hi, d1.Lo);

            if (d2.Hi == 0 && d2.Lo <= UInt16.MaxValue)
                div64by16(d1.Lo, d1.Hi, d2.Lo, out qlo, out qhi, out rest);
            else
                Div64by64(d1.Lo, d1.Hi, d2.Lo, d2.Hi, out qlo, out qhi, out rlo, out rhi);

            result = new /* DDUInt64 */ UInt64(qhi, qlo);
            return result;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Modulus(System.UInt64,System.UInt64)&quot;]/*"/>
        public static /* DDUInt64 */ UInt64 operator %(/* DDUInt64 */ UInt64 d1, /* DDUInt64 */ UInt64 d2)
        {
            /* DDUInt64 */
            UInt64 result;
            UInt32 qlo, qhi, rlo, rhi;

            if (d2.Lo == 0 && d2.Hi == 0)
                throw new DivideByZeroException();
            if (d1.Lo == 0 && d1.Hi == 0)
                return new /* DDUInt64 */ UInt64(0, 0);
            if (d1.Lo == d2.Lo && d1.Hi == d2.Hi)
                return new /* DDUInt64 */ UInt64(0, 0);

            if (d2.Hi == 0 && d2.Lo <= UInt16.MaxValue)
            {
                rhi = 0;
                div64by16(d1.Lo, d1.Hi, d2.Lo, out qlo, out qhi, out rlo);
            }
            else
                Div64by64(d1.Lo, d1.Hi, d2.Lo, d2.Hi, out qlo, out qhi, out rlo, out rhi);

            result = new /* DDUInt64 */ UInt64(rhi, rlo);
            return result;
        }
        public static /* DDUInt64 */ UInt64 operator ~(/* DDUInt64 */ UInt64 d1)
        {
            d1.Lo = ~d1.Lo;
            d1.Hi = ~d1.Hi;
            return d1;
        }

        public static /* DDUInt64 */ UInt64 operator &(/* DDUInt64 */ UInt64 d1, /* DDUInt64 */ UInt64 d2)
        {
            d1.Lo &= d2.Lo;
            d1.Hi &= d2.Hi;
            return d1;
        }
        public static /* DDUInt64 */ UInt64 operator |(/* DDUInt64 */ UInt64 d1, /* DDUInt64 */ UInt64 d2)
        {
            d1.Lo |= d2.Lo;
            d1.Hi |= d2.Hi;
            return d1;
        }
        public static /* DDUInt64 */ UInt64 operator ^(/* DDUInt64 */ UInt64 d1, /* DDUInt64 */ UInt64 d2)
        {
            d1.Lo ^= d2.Lo;
            d1.Hi ^= d2.Hi;
            return d1;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Equality(System.UInt64,System.UInt64)&quot;]/*"/>
        public static bool operator ==(/* DDUInt64 */ UInt64 d1, /* DDUInt64 */ UInt64 d2)
        {
            return Compare(d1, d2) == 0;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_Inequality(System.UInt64,System.UInt64)&quot;]/*"/>
        public static bool operator !=(/* DDUInt64 */ UInt64 d1, /* DDUInt64 */ UInt64 d2)
        {
            return Compare(d1, d2) != 0;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_GreaterThan(System.UInt64,System.UInt64)&quot;]/*"/>
        public static bool operator >(/* DDUInt64 */ UInt64 d1, /* DDUInt64 */ UInt64 d2)
        {
            return Compare(d1, d2) > 0;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_GreaterThanOrEqual(System.UInt64,System.UInt64)&quot;]/*"/>
        public static bool operator >=(/* DDUInt64 */ UInt64 d1, /* DDUInt64 */ UInt64 d2)
        {
            return Compare(d1, d2) >= 0;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_LessThan(System.UInt64,System.UInt64)&quot;]/*"/>
        public static bool operator <(/* DDUInt64 */ UInt64 d1, /* DDUInt64 */ UInt64 d2)
        {
            return Compare(d1, d2) < 0;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_LessThanOrEqual(System.UInt64,System.UInt64)&quot;]/*"/>
        public static bool operator <=(/* DDUInt64 */ UInt64 d1, /* DDUInt64 */ UInt64 d2)
        {
            return Compare(d1, d2) <= 0;
        }

        public static /* DDUInt64 */ UInt64 operator <<(/* DDUInt64 */ UInt64 d1, Int32 n)
        {
            UInt32 lo = d1.m_lo, hi = d1.m_hi;
            n = n & 0x3f;
            while (n-- > 0)
            {
                lshift(ref lo, ref hi);
            }
            return new UInt64(hi, lo);
        }

        public static /* DDUInt64 */ UInt64 operator >>(/* DDUInt64 */ UInt64 d1, Int32 n)
        {
            UInt32 lo = d1.m_lo, hi = d1.m_hi;
            n = n & 0x3f;
            while (n-- > 0)
                rshift(ref lo, ref hi);
            return new UInt64(hi, lo);
        }

        #endregion Operators


        #region Operators 32 bit
        // ============== Operators 32 bit =================
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_AdditionUInt32(System.UInt64,System.UInt32)&quot;]/*"/>
        public static UInt64 op_AdditionUInt32(UInt64 d1, UInt32 d2)
        {
            Add64(d1.Lo, d1.Hi, d2, 0, out d1.m_lo, out d1.m_hi);
            return d1;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_SubtractionUInt32(System.UInt64,System.UInt32)&quot;]/*"/>
        public static UInt64 op_SubtractionUInt32(UInt64 d1, UInt32 d2)
        {
            Sub64(d1.Lo, d1.Hi, d2, 0, out d1.m_lo, out d1.m_hi);
            return d1;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_MultiplyUInt32(System.UInt64,System.UInt32)&quot;]/*"/>
        public static UInt64 op_MultiplyUInt32(UInt64 d1, UInt32 d2)
        {
            Int128 result;
            if (d2 <= UInt16.MaxValue)
            {
                UInt32 c0, c1;
                Mult64by16to64(d1.Lo, d1.Hi, (UInt16)d2, out c0, out c1);
                return new UInt64(c1, c0);
            }
            else
            {
                result = Mult64by64to128(d1.Lo, d1.Hi, d2, 0);
                return new UInt64(result.c1, result.c0);
            }
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_DivisionUInt32(System.UInt64,System.UInt32)&quot;]/*"/>
        public static UInt64 op_DivisionUInt32(UInt64 d1, UInt32 d2)
        {
            UInt64 result;
            UInt32 qlo, qhi, rlo, rhi, rest;

            if (d2 == 0)
                throw new DivideByZeroException();
            if (d1.Lo == 0 && d1.Hi == 0)
                return new UInt64(0, 0);
            if (d1.Lo == d2 && d1.Hi == 0)
                return new UInt64(0, 1);
            if (d2 == 1)
                return new UInt64(d1.Hi, d1.Lo);

            if (d2 <= UInt16.MaxValue)
                div64by16(d1.Lo, d1.Hi, d2, out qlo, out qhi, out rest);
            else
                Div64by64(d1.Lo, d1.Hi, d2, 0, out qlo, out qhi, out rlo, out rhi);

            result = new UInt64(qhi, qlo);
            return result;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_ModulusUInt32(System.UInt64,System.UInt32)&quot;]/*"/>
        public static UInt64 op_ModulusUInt32(UInt64 d1, UInt32 d2)
        {
            UInt64 result;
            UInt32 qlo, qhi, rlo, rhi;

            if (d2 == 0)
                throw new DivideByZeroException();
            if (d1.Lo == 0 && d1.Hi == 0)
                return new UInt64(0, 0);
            if (d1.Lo == d2 && d1.Hi == 0)
                return new UInt64(0, 0);

            if (d2 <= UInt16.MaxValue)
            {
                rhi = 0;
                div64by16(d1.Lo, d1.Hi, d2, out qlo, out qhi, out rlo);
            }
            else
                Div64by64(d1.Lo, d1.Hi, d2, 0, out qlo, out qhi, out rlo, out rhi);

            result = new UInt64(rhi, rlo);
            return result;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_BitwiseAndUInt32(System.UInt64,System.UInt32)&quot;]/*"/>
        public static UInt64 op_BitwiseAndUInt32(UInt64 d1, UInt32 d2)
        {
            d1.Lo &= d2;
            return d1;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_BitwiseOrUInt32(System.UInt64,System.UInt32)&quot;]/*"/>
        public static UInt64 op_BitwiseOrUInt32(UInt64 d1, UInt32 d2)
        {
            d1.Lo |= d2;
            return d1;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_ExclusiveOrUInt32(System.UInt64,System.UInt32)&quot;]/*"/>
        public static UInt64 op_ExclusiveOrUInt32(UInt64 d1, UInt32 d2)
        {
            d1.Lo ^= d2;
            return d1;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_EqualityUInt32(System.UInt64,System.UInt32)&quot;]/*"/>
        public static bool op_EqualityUInt32(UInt64 d1, UInt32 d2)
        {
            return Compare(d1.Lo, d1.Hi, d2, 0) == 0;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_InequalityUInt32(System.UInt64,System.UInt32)&quot;]/*"/>
        public static bool op_InequalityUInt32(UInt64 d1, UInt32 d2)
        {
            return Compare(d1.Lo, d1.Hi, d2, 0) != 0;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_GreaterThanUInt32(System.UInt64,System.UInt32)&quot;]/*"/>
        public static bool op_GreaterThanUInt32(UInt64 d1, UInt32 d2)
        {
            return Compare(d1.Lo, d1.Hi, d2, 0) > 0;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_GreaterThanOrEqualUInt32(System.UInt64,System.UInt32)&quot;]/*"/>
        public static bool op_GreaterThanOrEqualUInt32(UInt64 d1, UInt32 d2)
        {
            return Compare(d1.Lo, d1.Hi, d2, 0) >= 0;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_LessThanUInt32(System.UInt64,System.UInt32)&quot;]/*"/>
        public static bool op_LessThanUInt32(UInt64 d1, UInt32 d2)
        {
            return Compare(d1.Lo, d1.Hi, d2, 0) < 0;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.UInt64.op_LessThanOrEqualUInt32(System.UInt64,System.UInt32)&quot;]/*"/>
        public static bool op_LessThanOrEqualUInt32(UInt64 d1, UInt32 d2)
        {
            return Compare(d1.Lo, d1.Hi, d2, 0) <= 0;
        }
        #endregion Operators 32 bit
    }
}
