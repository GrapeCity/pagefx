//
// System.Int64.cs
//
// Author:
//   Miguel de Icaza (miguel@ximian.com)
//
// (C) Ximian, Inc.  http://www.ximian.com
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
using System.Globalization;

// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! When Int64 is compiled pass by value = pass by reference for flash! Carefull! !!!!!!!!!!!!!!!!!

namespace System
{

    //[Serializable]
    /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;T:System.Int64&quot;]/*"/>
    public struct Int64 : IComparable, IFormattable, IConvertible
    {

        public const long MaxValue = 0x7fffffffffffffff;
        public const long MinValue = -9223372036854775808;

        private const UInt32 SIGN_FLAG = 0x80000000;

        public UInt32 m_hi;// !! don't change order since stack type is [hi,lo] , conv_u8 depends on this also CodeNames.LongIntLo/HiIndex defines this as constant
        public UInt32 m_lo;

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.#ctor(System.UInt32,System.UInt32)&quot;]/*"/>
        public Int64(UInt32 hi, UInt32 lo)
        {
            m_hi = hi;
            m_lo = lo;
        }

        internal UInt64 boxU()
        {
            return new UInt64(this.m_hi, this.m_lo);
        }

        internal static Int64 FromInt32(Int32 lo)
        {
            return new Int64(lo < 0 ? (uint)0xFFFFFFFF : 0, (uint)lo);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;P:System.Int64.Hi&quot;]/*"/>
        public UInt32 Hi
        {
            //[System.Diagnostics.DebuggerStepThroughAttribute]
            get { return m_hi; }
            //[System.Diagnostics.DebuggerStepThroughAttribute]
            set { m_hi = value; }
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;P:System.Int64.Lo&quot;]/*"/>
        public UInt32 Lo
        {
            //[System.Diagnostics.DebuggerStepThroughAttribute]
            get { return m_lo; }
            //[System.Diagnostics.DebuggerStepThroughAttribute]
            set { m_lo = value; }
        }

        #region Conversion routines
        // ========= explicit and implicit conversion routines
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Implicit(System.UInt64)~System.Int64&quot;]/*"/>
        public static implicit operator Int64(ulong val)
        {
            return new Int64(val.m_hi, val.m_lo);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Explicit(System.Int64)~System.UInt64&quot;]/*"/>
        public static explicit operator ulong(Int64 val)
        {
            return new UInt64(val.m_hi, val.m_lo);
        }
#if NODDCORLIB
		public static implicit operator Int64(long val) {
			return new Int64((UInt32) (val >> 32),(UInt32) val);
		}
		public static explicit operator long(Int64 val) {
			return (long) ((ulong) val.Hi << 32 | (UInt32) val.Lo);
		}
#endif
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Implicit(System.UInt32)~System.Int64&quot;]/*"/>
        public static implicit operator Int64(UInt32 val)
        {
            return new Int64(0, val);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Explicit(System.Int64)~System.UInt32&quot;]/*"/>
        public static explicit operator UInt32(Int64 val)
        {
            return val.Lo;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Implicit(System.Int32)~System.Int64&quot;]/*"/>
        public static implicit operator Int64(Int32 val)
        {
            return new Int64(val < 0 ? 0xFFFFffff : 0, (UInt32)val);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Explicit(System.Int64)~System.Int32&quot;]/*"/>
        public static explicit operator Int32(Int64 val)
        {
            return (Int32)val.Lo;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Implicit(System.UInt16)~System.Int64&quot;]/*"/>
        public static implicit operator Int64(UInt16 val)
        {
            return new Int64(0, val);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Explicit(System.Int64)~System.UInt16&quot;]/*"/>
        public static explicit operator UInt16(Int64 val)
        {
            return (UInt16)val.Lo;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Implicit(System.Int16)~System.Int64&quot;]/*"/>
        public static implicit operator Int64(Int16 val)
        {
            return new Int64(val < 0 ? 0xFFFFffff : 0, (UInt32)val);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Explicit(System.Int64)~System.Int16&quot;]/*"/>
        public static explicit operator Int16(Int64 val)
        {
            return (Int16)val.Lo;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Implicit(System.Byte)~System.Int64&quot;]/*"/>
        public static implicit operator Int64(byte val)
        {
            return new Int64(0, val);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Explicit(System.Int64)~System.Byte&quot;]/*"/>
        public static explicit operator byte(Int64 val)
        {
            return (byte)val.Lo;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Implicit(System.SByte)~System.Int64&quot;]/*"/>
        public static implicit operator Int64(sbyte val)
        {
            return new Int64(val < 0 ? 0xFFFFffff : 0, (UInt32)val);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Explicit(System.Int64)~System.SByte&quot;]/*"/>
        public static explicit operator sbyte(Int64 val)
        {
            return (sbyte)val.Lo;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Implicit(System.Single)~System.Int64&quot;]/*"/>
        public static implicit operator Int64(Single val)
        {
            UInt32 hi, lo;
            Int64 res;
            if (val < 0)
            {
                val = -val;
                hi = (UInt32)(val / 4294967296.0);
                lo = (UInt32)(val - 4294967296.0 * hi);
                res = new Int64(hi, lo);
                res.opNeg();
            }
            else
            {
                hi = (UInt32)(val / 4294967296.0);
                lo = (UInt32)(val - 4294967296.0 * hi);
                res = new Int64(hi, lo);
            }
            return res;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Explicit(System.Int64)~System.Single&quot;]/*"/>
        public static explicit operator Single(Int64 val)
        {
            Int32 hi = (Int32)val.Hi;
            double d = 4294967296.0 * hi + val.Lo;
            return (Single)d;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Implicit(System.Double)~System.Int64&quot;]/*"/>
        public static implicit operator Int64(Double val)
        {
            Int64 res;
            UInt32 hi, lo;
            if (val < 0)
            {
                val = -val;
                hi = (UInt32)(val / 4294967296.0);
                lo = (UInt32)(val - 4294967296.0 * hi);
                res = new Int64(hi, lo);
                res.opNeg();
            }
            else
            {
                hi = (UInt32)(val / 4294967296.0);
                lo = (UInt32)(val - 4294967296.0 * hi);
                res = new Int64(hi, lo);
            }
            return res;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Explicit(System.Int64)~System.Double&quot;]/*"/>
        public static explicit operator Double(Int64 val)
        {
            Int32 hi = (Int32)val.Hi;
            double d = 4294967296.0 * hi + val.Lo;
            return d;
        }
#if false
		public static implicit operator Int64(Decimal val) {
			int[] bits = Decimal.GetBits(val);
			Int64 result = new Int64((UInt32) bits[1],(UInt32) bits[0]);
			if (val < 0)
				return -result;
			return result;
		}
#endif
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Explicit(System.Int64)~System.Decimal&quot;]/*"/>
        public static explicit operator Decimal(Int64 val)
        {
            if ((val.Hi & SIGN_FLAG) != 0)
            {
                val.opNeg();
                return new Decimal((Int32)val.Lo, (Int32)val.Hi, 0, true, 0);
            }
            return new Decimal((Int32)val.Lo, (Int32)val.Hi, 0, false, 0);
        }

        //		public static implicit operator UInt64(Boolean val) {
        //			return new UInt64(val ? (UInt32) 1 : 0, 0);
        //		}
        //		public static explicit operator Boolean(UInt64 val) {
        //			return val.Lo != 0 || val.Hi != 0;
        //		}
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Implicit(System.Char)~System.Int64&quot;]/*"/>
        public static implicit operator Int64(Char val)
        {
            return new Int64(0, val);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Explicit(System.Int64)~System.Char&quot;]/*"/>
        public static explicit operator Char(Int64 val)
        {
            return (Char)val.Lo;
        }
        #endregion Conversion routines
        // ========= UInt64 interface methods ===============

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.CompareTo(System.Object)&quot;]/*"/>
        public int CompareTo(object value)
        {
            /* DDInt64 */
            Int64 value2;

            if (value == null)
                return 1;

            if (!(value is Int64))
            {
                //Console.WriteLine("Int64 compare {0} ",value.GetType().FullName);
                throw new ArgumentException(Locale.GetText("Value is not a System.Int64"));
            }

            value2 = (Int64)value;
            if ((Int32)this.Hi < (Int32)value2.Hi)
                return -1;
            if ((Int32)this.Hi > (Int32)value2.Hi)
                return 1;
            if (this.Lo < value2.Lo)
                return -1;
            if (this.Lo > value2.Lo)
                return 1;
            return 0;
        }


        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.Equals(System.Object)&quot;]/*"/>
        public override bool Equals(object o)
        {
            //Console.WriteLine("Int64::Equals called");
            if (!(o is Int64))
            {
                //Console.WriteLine("object not int64");
                return false;
            }
            long v = (long)o;
            //Console.WriteLine("\t{0}:{1} {2}:{3}",this.m_hi,this.m_lo,v.m_hi,v.m_lo);
            return this.m_hi == v.m_hi && this.m_lo == v.m_lo;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.GetHashCode&quot;]/*"/>
        public override int GetHashCode()
        {
            return (int)(this.m_hi ^ this.m_lo);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.Parse(System.String)&quot;]/*"/>
        public static Int64 Parse(string s)
        {
            return Parse(s, NumberStyles.Integer, null);
#if false
			long val = 0;
			int len;
			int i;
			int sign = 1;
			bool digits_seen = false;
			
			if (s == null)
				throw new ArgumentNullException ("s");

			len = s.Length;

			char c;
			for (i = 0; i < len; i++){
				c = s [i];
				if (!Char.IsWhiteSpace (c))
					break;
			}
			
			if (i == len)
				throw new FormatException ();

			c = s [i];
			if (c == '+')
				i++;
			else if (c == '-'){
				sign = -1;
				i++;
			}
			
			for (; i < len; i++){
				c = s [i];

				if (c >= '0' && c <= '9'){
					val = checked (val * 10 + (c - '0') * sign);
					digits_seen = true;
				} else {
					if (Char.IsWhiteSpace (c)){
						for (i++; i < len; i++){
							if (!Char.IsWhiteSpace (s [i]))
								throw new FormatException ();
						}
						break;
					} else
						throw new FormatException ();
				}
			}
			if (!digits_seen)
				throw new FormatException ();
			
			return val;
#endif
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.Parse(System.String,System.IFormatProvider)&quot;]/*"/>
        public static Int64 Parse(string s, IFormatProvider fp)
        {
            return Parse(s, NumberStyles.Integer, fp);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.Parse(System.String,System.Globalization.NumberStyles)&quot;]/*"/>
        public static Int64 Parse(string s, NumberStyles style)
        {
            return Parse(s, style, null);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.Parse(System.String,System.Globalization.NumberStyles,System.IFormatProvider)&quot;]/*"/>
        public static Int64 Parse(string s, NumberStyles style, IFormatProvider fp)
        {
            if (s == null)
                throw new ArgumentNullException();

            if (s.Length == 0)
                throw new FormatException("Input string was not " +
                    "in the correct format: s.Length==0.");

            NumberFormatInfo nfi;
            if (fp != null)
            {
                Type typeNFI = typeof(System.Globalization.NumberFormatInfo);
                nfi = (NumberFormatInfo)fp.GetFormat(typeNFI);
            }
            else
            {
                //TODO
                //nfi = Thread.CurrentThread.CurrentCulture.NumberFormat;
                nfi = new NumberFormatInfo();
            }

            UInt64.CheckStyle(style);

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
                pos = UInt64.JumpOverWhite(pos, s, true);

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
                    pos = UInt64.JumpOverWhite(pos, s, true);

                if (s.Substring(pos, nfi.NegativeSign.Length) == nfi.NegativeSign)
                    throw new FormatException("Input string was not in the correct " +
                        "format: Has Negative Sign.");
                if (s.Substring(pos, nfi.PositiveSign.Length) == nfi.PositiveSign)
                    throw new FormatException("Input string was not in the correct " +
                        "format: Has Positive Sign.");
            }

            if (AllowLeadingSign && !foundSign)
            {
                // Sign + Currency
                UInt64.FindSign(ref pos, s, nfi, ref foundSign, ref negative);
                if (foundSign)
                {
                    if (AllowLeadingWhite)
                        pos = UInt64.JumpOverWhite(pos, s, true);
                    if (AllowCurrencySymbol)
                    {
                        UInt64.FindCurrency(ref pos, s, nfi,
                            ref foundCurrency);
                        if (foundCurrency && AllowLeadingWhite)
                            pos = UInt64.JumpOverWhite(pos, s, true);
                    }
                }
            }

            if (AllowCurrencySymbol && !foundCurrency)
            {
                // Currency + sign
                UInt64.FindCurrency(ref pos, s, nfi, ref foundCurrency);
                if (foundCurrency)
                {
                    if (AllowLeadingWhite)
                        pos = UInt64.JumpOverWhite(pos, s, true);
                    if (foundCurrency)
                    {
                        if (!foundSign && AllowLeadingSign)
                        {
                            UInt64.FindSign(ref pos, s, nfi, ref foundSign,
                                ref negative);
                            if (foundSign && AllowLeadingWhite)
                                pos = UInt64.JumpOverWhite(pos, s, true);
                        }
                    }
                }
            }

            UInt32 lo = 0;
            UInt32 hi = 0;
            //long number = 0;
            int nDigits = 0;
            bool decimalPointFound = false;
            int digitValue;
            char hexDigit;

            // Number stuff
            do
            {

                if (!Int32.ValidDigit(s[pos], AllowHexSpecifier))
                {
                    if (AllowThousands &&
                        (UInt64.FindOther(ref pos, s, nfi.NumberGroupSeparator)
                        || UInt64.FindOther(ref pos, s, nfi.CurrencyGroupSeparator)))
                        continue;
                    else
                        if (!decimalPointFound && AllowDecimalPoint &&
                        (UInt64.FindOther(ref pos, s, nfi.NumberDecimalSeparator)
                        || UInt64.FindOther(ref pos, s, nfi.CurrencyDecimalSeparator)))
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
                        digitValue = (int)(hexDigit - '0');
                    else if (Char.IsLower(hexDigit))
                        digitValue = (int)(hexDigit - 'a' + 10);
                    else
                        digitValue = (int)(hexDigit - 'A' + 10);

                    UInt64.Mult64by16to64(lo, hi, 16, out lo, out hi);
                    UInt64.Add64(lo, hi, (UInt32)digitValue, 0, out lo, out hi);

                    //ulong unumber = (ulong)number;
                    //number = (long)checked(unumber * 16ul + (ulong)digitValue);
                }
                else if (decimalPointFound)
                {
                    nDigits++;
                    // Allows decimal point as long as it's only 
                    // followed by zeroes.
                    if (s[pos++] != '0')
                        throw new OverflowException("Value too large or too " +
                            "small.");
                }
                else
                {
                    nDigits++;
                    digitValue = (int)(s[pos++] - '0');

                    UInt64.Mult64by16to64(lo, hi, 10, out lo, out hi);
                    UInt64.Add64(lo, hi, (UInt32)digitValue, 0, out lo, out hi);

                    //try {
                    //	// Calculations done as negative
                    //	// (abs (MinValue) > abs (MaxValue))
                    //	number = checked (
                    //		number * 10 - 
                    //		(long) (s [pos++] - '0')
                    //		);
                    //} catch (OverflowException) {
                    //	throw new OverflowException ("Value too large or too " +
                    //		"small.");
                    //}
                }
            } while (pos < s.Length);

            // Post number stuff
            if (nDigits == 0)
                throw new FormatException("Input string was not in the correct format: nDigits == 0.");

            if (AllowTrailingSign && !foundSign)
            {
                // Sign + Currency
                UInt64.FindSign(ref pos, s, nfi, ref foundSign, ref negative);
                if (foundSign)
                {
                    if (AllowTrailingWhite)
                        pos = UInt64.JumpOverWhite(pos, s, true);
                    if (AllowCurrencySymbol)
                        UInt64.FindCurrency(ref pos, s, nfi,
                            ref foundCurrency);
                }
            }

            if (AllowCurrencySymbol && !foundCurrency)
            {
                // Currency + sign
                if (nfi.CurrencyPositivePattern == 3 && s[pos++] != ' ')
                    throw new FormatException("Input string was not in the correct format: no space between number and currency symbol.");

                UInt64.FindCurrency(ref pos, s, nfi, ref foundCurrency);
                if (foundCurrency && pos < s.Length)
                {
                    if (AllowTrailingWhite)
                        pos = UInt64.JumpOverWhite(pos, s, true);
                    if (!foundSign && AllowTrailingSign)
                        UInt64.FindSign(ref pos, s, nfi, ref foundSign,
                            ref negative);
                }
            }

            if (AllowTrailingWhite && pos < s.Length)
                pos = UInt64.JumpOverWhite(pos, s, false);

            if (foundOpenParentheses)
            {
                if (pos >= s.Length || s[pos++] != ')')
                    throw new FormatException("Input string was not in the correct " +
                        "format: No room for close parens.");
                if (AllowTrailingWhite && pos < s.Length)
                    pos = UInt64.JumpOverWhite(pos, s, false);
            }

            if (pos < s.Length && s[pos] != '\u0000')
                throw new FormatException("Input string was not in the correct format: Did not parse entire string. pos = "
                    + pos + " s.Length = " + s.Length);

            Int64 res = new Int64(hi, lo);
            if (negative)
                res.opNeg();
            return res;

            //if (!negative && !AllowHexSpecifier)
            //	number = -number;
            //return number;
        }

        private string ToStringX()
        {
            string s;
            s = this.m_hi.ToString() + ":" + this.m_lo.ToString();
            return s;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.ToString&quot;]/*"/>
        public override string ToString()
        {
            //Console.WriteLine("Int64::ToString call");
            return ToString(null, null);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.ToString(System.IFormatProvider)&quot;]/*"/>
        public string ToString(IFormatProvider fp)
        {
            return ToString(null, fp);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.ToString(System.String)&quot;]/*"/>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.ToString(System.String,System.IFormatProvider)&quot;]/*"/>
        public string ToString(string format, IFormatProvider fp)
        {
            Decimal d;
            UInt32 lo, hi;

            //Console.WriteLine("Int64ToStringIFormatProvider format1={0}",format);

            if (format != null && (format.StartsWith("d") || format.StartsWith("D")))
                format = "G" + format.Substring(1);

            //Console.WriteLine("Int64ToStringIFormatProvider format2={0}",format);

            if ((m_hi & SIGN_FLAG) != 0)
            {
                // Special case of Int64.MinValue
                lo = (~this.m_lo) + 1;
                hi = ~this.m_hi;
                if (lo == 0)
                    ++hi;
                d = new Decimal((int)lo, (int)hi, 0, true, 0);
            }
            else
            {
                d = new Decimal((int)m_lo, (int)m_hi, 0, false, 0);
            }

            //Console.WriteLine("Calling decimal.ToString decimal {0}:{1}:{2}:{3}",d.ss32,d.hi32,d.mid32,d.lo32);
            String s = d.ToString(format, fp);
            return s;
#if false
			NumberFormatInfo nfi = NumberFormatInfo.GetInstance( fp );
			
			// use "G" when format is null or String.Empty
			if ((format == null) || (format.Length == 0))
				format = "G";
			return IntegerFormatter.NumberToString (format, nfi, value__);
#endif
        }

        #region IConvertible
        // =========== IConvertible Methods =========== //

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.GetTypeCode&quot;]/*"/>
        internal static System.TypeCode __typeCode = TypeCode.Int64;
        public TypeCode GetTypeCode()
        {
            return TypeCode.Int64;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return (m_hi > 0 || m_lo > 0);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            if (m_hi != 0 || m_lo > 0xFF)
                throw new OverflowException("value was either too large or too small for an unsigned byte.");
            return System.Convert.ToByte(m_lo);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            if (m_hi != 0 || m_lo > 0xFFFF)
                throw new OverflowException("value was either too large or too small for a character.");
            return System.Convert.ToChar(m_lo);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
            //return System.Convert.ToDateTime (value__);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return (Decimal)this;
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            Int32 hi = (Int32)m_hi;
            double d = 4294967296.0 * hi + m_lo;
            return d;
        }

        // don't remove.Used by conv_r8
        internal static double ToDoubleInternal(Int64 d)
        {
            int intHi = (Int32)d.m_hi;
            return (4294967296.0 * intHi) + d.m_lo;
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            if (m_hi != 0 || m_lo >= 0x8000)
                if (m_hi != 0xFFFFffff && m_lo != 0xFFFF8000)
                    throw new OverflowException("value was either too large or too small for an Int16.");
            return System.Convert.ToInt16((Int32)m_lo);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            if (m_hi != 0 || m_lo == 0x80000000)
                if (m_hi != 0xFFFFffff && m_hi != 0x8000000)
                    throw new OverflowException("value was either too large or too small for an Int32.");

            return System.Convert.ToInt32((Int32)m_lo);
        }
#if NODDCORLIB
		long IConvertible.ToInt64 (IFormatProvider provider) {
			long val = (long) (ulong) m_hi << 32 | m_lo;
			return System.Convert.ToInt64 (val);
		}
#else
        Int64 IConvertible.ToInt64(IFormatProvider provider)
        {
            return new Int64(m_hi, m_lo);
        }
#endif

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            if (m_hi != 0 || m_lo >= 0x80)
                if (m_hi != 0xFFFFffff && m_hi != 0xFFFFFF80)
                    throw new OverflowException("value was either too large or too small for a signed byte.");

            return System.Convert.ToSByte((Int32)m_lo);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            Int32 hi = (Int32)m_hi;
            double d = 4294967296.0 * hi + m_lo;
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
                throw new OverflowException("value was either too large or too small for a UInt16.");
            return System.Convert.ToUInt16((Int32)m_lo);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            if (m_hi != 0)
                throw new OverflowException("value was either too large or too small for a UInt32.");
            return System.Convert.ToUInt32((Int32)m_lo);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            ulong u = (ulong)m_hi << 32 | m_lo;
            return u;
        }

        #endregion IConvertible

        #region Internal Math Routines
        // ================== Internal Math Routines =====================

        internal void opNeg()
        {
            this.m_lo = (~this.m_lo) + 1;
            this.m_hi = ~this.m_hi;
            if (this.m_lo == 0)
                ++this.m_hi;
        }

        private static Int64 op_Negate(Int64 val)
        {
            val.m_lo = (~val.m_lo) + 1;
            val.m_hi = ~val.m_hi;
            if (val.m_lo == 0)
                val.m_hi++;
            return val;
        }

        private static Int32 Compare(Int64 a, Int64 b)
        {
            //			Flash.Native.Actions.Trace("a.m_hi=");
            //			Flash.Native.Actions.Trace(a.m_hi);
            //			Flash.Native.Actions.Trace("b.m_hi=");
            //			Flash.Native.Actions.Trace(b.m_hi);
            //			
            if ((int)(a.m_hi) < (int)(b.m_hi))
            {
                //Console.WriteLine("   a.hi<b.hi");
                return -1;
            }
            if ((int)(a.m_hi) > (int)(b.m_hi))
            {
                //				Flash.Native.Actions.Trace("ax=");
                //				Flash.Native.Actions.Trace(a.m_hi);
                //				Flash.Native.Actions.Trace("bx=");
                //				Flash.Native.Actions.Trace(b.m_hi);
                //Console.WriteLine("   cmp1 {0} {1}",a.m_hi,b.m_hi);
                //Console.WriteLine("   a.hi>b.hi");
                return 1;
            }
            //Console.WriteLine("a.m_lo={0} b.m_lo={1}",a.m_lo,b.m_lo);
            //			Flash.Native.Actions.Trace("a.m_lo=");
            //			Flash.Native.Actions.Trace(a.m_lo);
            //			Flash.Native.Actions.Trace("b.m_lo=");
            //			Flash.Native.Actions.Trace(b.m_lo);

            if (a.m_lo == b.m_lo)
            {
                //Console.WriteLine("   a.lo==b.lo");
                return 0;
            }
            if (a.m_lo < b.m_lo)
            {
                //Console.WriteLine("   a.lo<b.lo");
                return -1;
            }
            //Console.WriteLine("   cmp2 {0} {1}",a.m_lo,b.m_lo);
            //Console.WriteLine("   a.lo>b.lo");
            return 1;
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
            UInt32 sign = (ahi & SIGN_FLAG);
            UInt32 carry = (ahi & 1) << 31;
            ahi >>= 1;
            ahi |= sign;
            alo = (alo >> 1) | carry;
        }

        #endregion Internal Math Routines

        #region Operators
        // ============== Operators =================
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Addition(System.Int64,System.Int64)&quot;]/*"/>
        public static Int64 operator +(Int64 d1, Int64 d2)
        {
            Int64 res;
            //Console.WriteLine ("op +: {0} {1}   +    {2} {3}", d1, d1.ToStringX(), d2, d2.ToStringX());
            UInt64.Add64(d1.Lo, d1.Hi, d2.Lo, d2.Hi, out res.m_lo, out res.m_hi);
            return res;
        }

        // Same as operator + with overflow checking
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_AdditionOvf(System.Int64,System.Int64)&quot;]/*"/>
        public static Int64 op_AdditionOvf(Int64 d1, Int64 d2)
        {
            Int64 res;
            UInt64.Add64OvfSigned(d1.Lo, d1.Hi, d2.Lo, d2.Hi, out res.m_lo, out res.m_hi);
            return res;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Increment(System.Int64)&quot;]/*"/>
        public static Int64 operator ++(Int64 d1)
        {
            Int64 d2 = new Int64(0, 1);
            UInt64.Add64(d1.Lo, d1.Hi, d2.Lo, d2.Hi, out d1.m_lo, out d1.m_hi);
            return d1;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Addition(System.Int64)&quot;]/*"/>
        public static Int64 operator +(Int64 d)
        {
            return d;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Subtraction(System.Int64)&quot;]/*"/>
        public static Int64 operator -(Int64 d)
        {
            d.opNeg();
            return d;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Subtraction(System.Int64,System.Int64)&quot;]/*"/>
        public static Int64 operator -(Int64 d1, Int64 d2)
        {
            Int64 res;
            UInt64.Sub64(d1.Lo, d1.Hi, d2.Lo, d2.Hi, out res.m_lo, out res.m_hi);
            return res;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_SubtractionOvf(System.Int64,System.Int64)&quot;]/*"/>
        public static Int64 op_SubtractionOvf(Int64 a, Int64 b)
        {
            UInt32 dlo, dhi;
            dlo = a.m_lo - b.m_lo;
            bool isNeg1 = (a.m_hi & 0x80000000) != 0;
            bool isNeg2 = (b.m_hi & 0x80000000) != 0;
            if (a.m_lo < b.m_lo)
            {
                a.m_hi = a.m_hi - 1;// Borrow
            }
            dhi = a.m_hi - b.m_hi;
            if (isNeg1 != isNeg2)
            {
                if (isNeg1)
                {
                    if ((dhi & 0x80000000) == 0)
                        throw new OverflowException();
                }
                else if ((dhi & 0x80000000) != 0)
                    throw new OverflowException();
            }

            Int64 res;
            res.m_lo = dlo;
            res.m_hi = dhi;
            return res;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Decrement(System.Int64)&quot;]/*"/>
        public static Int64 operator --(Int64 d1)
        {
            Int64 d2 = new Int64(0, 1);
            UInt64.Sub64(d1.Lo, d1.Hi, d2.Lo, d2.Hi, out d1.m_lo, out d1.m_hi);
            return d1;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Multiply(System.Int64,System.Int64)&quot;]/*"/>
        public static Int64 operator *(Int64 d1, Int64 d2)
        {
            UInt32 c0, c1, c2, c3;

            bool sign1 = (d1.Hi & SIGN_FLAG) != 0;
            bool sign2 = (d2.Hi & SIGN_FLAG) != 0;

            d1 = new Int64(d1.m_hi, d1.m_lo);
            d2 = new Int64(d2.m_hi, d2.m_lo);

            //			Console.WriteLine("sign1={0} sign2={1}",sign1,sign2);
            //
            //			Console.WriteLine("d1.Lo={0} d1.Hi={1}",d1.m_lo,d1.m_hi);
            //			Console.WriteLine("d2.Lo={0} d2.Hi={1}",d2.m_lo,d2.m_hi);

            if (sign1)
                d1.opNeg();
            if (sign2)
                d2.opNeg();

            //			Console.WriteLine("d1.Lo={0} d1.Hi={1}",d1.m_lo,d1.m_hi);
            //			Console.WriteLine("d2.Lo={0} d2.Hi={1}",d2.m_lo,d2.m_hi);

            Int128 result128 = UInt64.Mult64by64to128(d1.Lo, d1.Hi, d2.Lo, d2.Hi);

            Int64 result = new Int64(result128.c1, result128.c0);
            if (sign1 != sign2)
                result.opNeg();
            return result;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Division(System.Int64,System.Int64)&quot;]/*"/>
        public static Int64 operator /(Int64 d1, Int64 d2)
        {
            //			if (d1 < d2) bad code, won't work for negative
            //			{
            //				Console.WriteLine("div lt");
            //				return 0;
            //			}
            if (d1 == d2)
            {
                return 1;
            }
            if (d2.Lo == 0 && d2.Hi == 0)
                throw new DivideByZeroException();

            if (FitIntoDouble(d1) && FitIntoDouble(d2))
            {
                //Console.WriteLine("FitsIntoDouble dividing {0}/{1}={2}={3}",d1,d2,((double)d1/(double)d2),(Int64)((double)d1/(double)d2));
                return (Int64)((double)d1 / (double)d2);
            }

            //Console.WriteLine("my_op_division {0}:{1} / {2}:{3}",d1.m_hi,d1.m_lo,d2.m_hi,d2.m_lo);
            long x1 = d1;
            long x2 = d2;
            return my_op_Division(x1, x2);
        }
        // For speed we do some Int64 math with doubles. But we must be
        // sure that we don't loose precision in the conversion. This tests
        // whether an Int64 can fit into a double by making sure the number
        // of bits of precision is 53 or less.
        internal static bool FitIntoDouble(Int64 testVal)
        {
            Int64 n = testVal;
            //const long MAXDOUBLEINT = (1L << 53) + 1;
            const long MAXDOUBLEINT = 9007199254740993L;
            if (n == 0) return true;
            if (n < 0) n = -n;

            if (n < MAXDOUBLEINT) return true;	// Quick check before any math

            while ((n & 1) == 0)	// Get rid of zeros on the right
                n >>= 1;

            if (n < MAXDOUBLEINT) return true;
            return false;
        }
        internal static Int64 my_op_Division(Int64 d1, Int64 d2)
        {
            Int64 result;
            UInt32 qlo, qhi, rlo, rhi, rest;
            bool sign1 = (d1.Hi & SIGN_FLAG) != 0;
            bool sign2 = (d2.Hi & SIGN_FLAG) != 0;

            if (d2.Lo == 0 && d2.Hi == 0)
            {
                throw new DivideByZeroException();
            }

            if (d1.Lo == 0 && d1.Hi == 0)
                return new Int64(0, 0);
            if (d1.Lo == d2.Lo && d1.Hi == d2.Hi)
                return new Int64(0, 1);
            if (d1.Hi == SIGN_FLAG && d1.Lo == 0 && d2.Lo == 0xFFFFffff && d2.Hi == 0xFFFFffff)
                throw new OverflowException();

            if (sign1)
                d1.opNeg();
            if (sign2)
                d2.opNeg();

            if (d1.Lo == d2.Lo && d1.Hi == d2.Hi)
                return new Int64(0xFFFFffff, 0xFFFFffff);

            if (d2.Hi == 0 && d2.Lo <= UInt16.MaxValue)
            {
                qlo = d1.Lo;
                qhi = d1.Hi;
                if (d2.Lo > 1)
                    UInt64.div64by16(d1.Lo, d1.Hi, d2.Lo, out qlo, out qhi, out rest);
            }

            UInt64.Div64by64(d1.Lo, d1.Hi, d2.Lo, d2.Hi, out qlo, out qhi, out rlo, out rhi);
            result = new Int64(qhi, qlo);
            if (sign1 ^ sign2)
                result.opNeg();

            //			Flash.Native.Actions.Trace("div1lo=");
            //			Flash.Native.Actions.Trace(d1.m_lo);
            //			Flash.Native.Actions.Trace("div2lo=");
            //			Flash.Native.Actions.Trace(d2.m_lo);
            //			Flash.Native.Actions.Trace("qlo=");
            //			Flash.Native.Actions.Trace(qlo);

            return result;
        }
#if false
		// An optimization for converting to string. Made in-line in FloatingPointFormatter
		public static void DivMod10 (Int64 d1, out Int64 quotient, out Int32 remainder) {
			Int64 q;
			Int32 r;

			if (d1.m_hi == 0) {
				q = d1.m_lo / 10;
				r = (Int32) (d1.m_lo % 10);
				// r = (Int32) (d1.m_lo - 10*q); -- this is slower
			} else if (FitIntoDouble(d1)) {
				double d1f = 4294967296.0 * d1.m_hi + d1.m_lo;
				double divRes = d1f / 10.0;
				q = (Int64) divRes;
				r = (Int32) (d1f - (10 * q));
			} else {
				q = d1 / 10;
				r = (Int32) (d1 - 10*q);
			}
			quotient = q;
			remainder = r;
		}
#endif
        // According to 7.7.3 of the C# Language Specification: 
        // "The result of x % y is the value produced by x - (x / y) * y"
        // (It also says it never overflows but it does for "IntXX.MinValue % -1")
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Modulus(System.Int64,System.Int64)&quot;]/*"/>
        public static Int64 operator %(Int64 d1, Int64 d2)
        {
            if (d2.Lo == 0 && d2.Hi == 0)
                throw new DivideByZeroException();

            if (FitIntoDouble(d1) && FitIntoDouble(d2))
            {
                // for flash runtime & numbers with < 53 bits of precision
                //Console.WriteLine(">>both fit into double");
                double d1f = 4294967296.0 * (int)d1.m_hi + d1.m_lo;
                double d2f = (4294967296.0 * (int)d2.m_hi + d2.m_lo);
                double divRes = d1f / d2f;
                //Console.WriteLine("d1f={0} d2f={1} divRes={2}",d1f,d2f,divRes);
                double truncatedValue;
                if (divRes < 0)
                    truncatedValue = -Math.Floor(-divRes);
                else
                    truncatedValue = Math.Floor(divRes);
                double val = d1f - (d2f * truncatedValue);

                UInt32 hi, lo;
                if (val < 0)
                {
                    val = -val;
                    hi = (UInt32)(val / 4294967296.0);
                    lo = (UInt32)(val - 4294967296.0 * hi);
                    Int64 res = new Int64(hi, lo);
                    res.opNeg();
                    return res;
                }
                else
                {
                    hi = (UInt32)(val / 4294967296.0);
                    lo = (UInt32)(val - 4294967296.0 * hi);
                    return new Int64(hi, lo);
                }
            }
            // for other platforms or numbers w/ > 53 bits of precision
            long x1 = d1; // non memberwise copy coming in, my_op_Modulus will modify input , this eliminates that
            long x2 = d2;
            return my_op_Modulus(x1, x2);
        }
        internal static Int64 my_op_Modulus(Int64 d1, Int64 d2)
        {
            Int64 result;
            UInt32 qlo, qhi, rlo, rhi;
            UInt32 c0, c1, c2, c3;
            bool sign1 = (d1.Hi & SIGN_FLAG) != 0;
            bool sign2 = (d2.Hi & SIGN_FLAG) != 0;

            if (d2.Lo == 0 && d2.Hi == 0)
                throw new DivideByZeroException();
            if (d1.Lo == 0 && d1.Hi == 0)
                return new Int64(0, 0);
            if (d1.Lo == d2.Lo && d1.Hi == d2.Hi)
                return new Int64(0, 0);
            if (d1.Hi == SIGN_FLAG && d1.Lo == 0 && d2.Lo == 0xFFFFffff && d2.Hi == 0xFFFFffff)
                throw new OverflowException();

            if (sign1)
                d1.opNeg();
            if (sign2)
                d2.opNeg();

            if (d2.Hi == 0 && d2.Lo <= UInt16.MaxValue)
            {
                rhi = 0;
                UInt64.div64by16(d1.Lo, d1.Hi, d2.Lo, out qlo, out qhi, out rlo);
            }
            else
                UInt64.Div64by64(d1.Lo, d1.Hi, d2.Lo, d2.Hi, out qlo, out qhi, out rlo, out rhi);

            if (sign1 || sign2)
            {
                if (sign1)
                    d1.opNeg();
                if (sign2)
                    d2.opNeg();

                // Compute d1 - (d1/d2)*d2
                if (sign1 ^ sign2)
                {
                    qlo = (~qlo) + 1;
                    qhi = ~qhi;
                    if (qlo == 0)
                        ++qhi;
                }
                Int128 result128 = UInt64.Mult64by64to128(qlo, qhi, d2.Lo, d2.Hi);
                UInt64.Sub64(d1.Lo, d1.Hi, result128.c0, result128.c1, out rlo, out rhi);
            }
            result = new Int64(rhi, rlo);
            return result;
        }

        public static Int64 operator ~(Int64 d1)
        {
            return new Int64(~d1.m_hi, ~d1.m_lo);
        }

        public static Int64 operator &(Int64 d1, Int64 d2)
        {
            return new Int64(d1.m_hi & d2.m_hi, d1.m_lo & d2.m_lo);
        }
        public static Int64 operator |(Int64 d1, Int64 d2)
        {
            return new Int64(d1.m_hi | d2.m_hi, d1.m_lo | d2.m_lo);
        }
        public static Int64 operator ^(Int64 d1, Int64 d2)
        {
            return new Int64(d1.m_hi ^ d2.m_hi, d1.m_lo ^ d2.m_lo);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Equality(System.Int64,System.Int64)&quot;]/*"/>
        public static bool operator ==(Int64 d1, Int64 d2)
        {
            //Console.WriteLine("Entered op_equality {0} {1}",d1.m_lo,d2.m_lo);
            return (d1.m_lo == d2.m_lo) && (d1.m_hi == d2.m_hi);
        }

        /// <summary>
        /// SwfILGenerator translates stack equals operator to this one since 
        //  operator== can't be used for sign/unsigned compares that the compiler tends to just ignore
        //  compiler will generate compare call for UInt64==0 as Compare(UInt64,Int
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        internal static bool opEq(Int64 d1, Int64 d2)
        {
            if (d1.m_lo != d2.m_lo)
                return false;
            if (d1.m_hi != d2.m_hi)
                return false;
            return true;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_Inequality(System.Int64,System.Int64)&quot;]/*"/>
        public static bool operator !=(Int64 d1, Int64 d2)
        {
            return Compare(d1, d2) != 0;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_GreaterThan(System.Int64,System.Int64)&quot;]/*"/>
        public static bool operator >(Int64 d1, Int64 d2)
        {
            //Console.WriteLine("    int64> {0}.{1} {2}.{3}",d1.m_hi,d1.m_lo,d2.m_hi,d2.m_lo);
            int res = Compare(d1, d2);
            //Console.WriteLine("    res={0}",res);
            return res > 0;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_GreaterThanOrEqual(System.Int64,System.Int64)&quot;]/*"/>
        public static bool operator >=(Int64 d1, Int64 d2)
        {
            //Console.WriteLine("    int64>= {0}.{1} {2}.{3}",d1.m_hi,d1.m_lo,d2.m_hi,d2.m_lo);
            int res = Compare(d1, d2);
            //Console.WriteLine("    res={0}",res);
            return res >= 0;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_LessThan(System.Int64,System.Int64)&quot;]/*"/>
        public static bool operator <(Int64 d1, Int64 d2)
        {
            return Compare(d1, d2) < 0;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_LessThanOrEqual(System.Int64,System.Int64)&quot;]/*"/>
        public static bool operator <=(Int64 d1, Int64 d2)
        {
            //Console.WriteLine("    int64<= {0}.{1} {2}.{3}",d1.m_hi,d1.m_lo,d2.m_hi,d2.m_lo);
            int res = Compare(d1, d2);
            //Console.WriteLine("    res={0}",res);
            return res <= 0;
        }

        public static Int64 operator <<(Int64 d1, Int32 n)
        {
            Int64 res = new Int64(d1.m_hi, d1.m_lo);
            n = n & 0x3f;
            while (n-- > 0)
                lshift(ref res.m_lo, ref res.m_hi);
            return res;
        }

        public static Int64 operator >>(Int64 d1, Int32 n)
        {
            Int64 res = new Int64(d1.m_hi, d1.m_lo);
            n = n & 0x3f;
            while (n-- > 0)
                rshift(ref res.m_lo, ref res.m_hi);
            return res;
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

        #endregion Operators

        #region Operators 32 bit unsigned
        // ============== Operators 32 bit unsigned =================
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_AdditionUInt32(System.Int64,System.UInt32)&quot;]/*"/>
        public static Int64 op_AdditionUInt32(Int64 d1, UInt32 d2)
        {
            long res;
            UInt64.Add64(d1.Lo, d1.Hi, d2, 0, out res.m_lo, out res.m_hi);
            return res;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_SubtractionUInt32(System.Int64,System.UInt32)&quot;]/*"/>
        public static Int64 op_SubtractionUInt32(Int64 d1, UInt32 d2)
        {
            long res;
            UInt64.Sub64(d1.Lo, d1.Hi, d2, 0, out res.m_lo, out res.m_hi);
            return res;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_MultiplyUInt32(System.Int64,System.UInt32)&quot;]/*"/>
        public static Int64 op_MultiplyUInt32(Int64 d1, UInt32 d2)
        {
            Int128 result;
            result = UInt64.Mult64by64to128(d1.Lo, d1.Hi, d2, 0);
            return new Int64(result.c1, result.c0);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_DivisionUInt32(System.Int64,System.UInt32)&quot;]/*"/>
        public static Int64 op_DivisionUInt32(Int64 d1, UInt32 d2)
        {
            return my_op_Division(d1, new Int64(0, d2));
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_ModulusUInt32(System.Int64,System.UInt32)&quot;]/*"/>
        public static Int64 op_ModulusUInt32(Int64 d1, UInt32 d2)
        {
            return my_op_Modulus(d1, new Int64(0, d2));
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_BitwiseAndUInt32(System.Int64,System.UInt32)&quot;]/*"/>
        public static Int64 op_BitwiseAndUInt32(Int64 d1, UInt32 d2)
        {
            d1.Lo &= d2;
            return d1;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_BitwiseOrUInt32(System.Int64,System.UInt32)&quot;]/*"/>
        public static Int64 op_BitwiseOrUInt32(Int64 d1, UInt32 d2)
        {
            return new Int64(d1.m_hi, d1.m_lo | d2);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_ExclusiveOrUInt32(System.Int64,System.UInt32)&quot;]/*"/>
        public static Int64 op_ExclusiveOrUInt32(Int64 d1, UInt32 d2)
        {
            return new Int64(d1.m_hi, d1.m_lo ^ d2);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_EqualityUInt32(System.Int64,System.UInt32)&quot;]/*"/>
        public static bool op_EqualityUInt32(Int64 d1, UInt32 d2)
        {
            return Compare(d1, new Int64(0, d2)) == 0;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_InequalityUInt32(System.Int64,System.UInt32)&quot;]/*"/>
        public static bool op_InequalityUInt32(Int64 d1, UInt32 d2)
        {
            return Compare(d1, new Int64(0, d2)) != 0;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_GreaterThanUInt32(System.Int64,System.UInt32)&quot;]/*"/>
        public static bool op_GreaterThanUInt32(Int64 d1, UInt32 d2)
        {
            return Compare(d1, new Int64(0, d2)) > 0;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_GreaterThanOrEqualUInt32(System.Int64,System.UInt32)&quot;]/*"/>
        public static bool op_GreaterThanOrEqualUInt32(Int64 d1, UInt32 d2)
        {
            return Compare(d1, new Int64(0, d2)) >= 0;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_LessThanUInt32(System.Int64,System.UInt32)&quot;]/*"/>
        public static bool op_LessThanUInt32(Int64 d1, UInt32 d2)
        {
            return Compare(d1, new Int64(0, d2)) < 0;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_LessThanOrEqualUInt32(System.Int64,System.UInt32)&quot;]/*"/>
        public static bool op_LessThanOrEqualUInt32(Int64 d1, UInt32 d2)
        {
            return Compare(d1, new Int64(0, d2)) <= 0;
        }
        #endregion Operators 32 bit unsigned

        #region Operators 32 bit signed
        // ============== Operators 32 bit signed =================
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_AdditionInt32(System.Int64,System.Int32)&quot;]/*"/>
        public static Int64 op_AdditionInt32(Int64 d1, Int32 d2)
        {
            uint alo = d1.Lo;
            uint blo = (uint)d2;
            uint ahi = d1.Hi;
            uint bhi = (d2 < 0) ? UInt32.MaxValue : 0;
            alo += blo;
            if (alo < blo) ahi++;
            ahi += bhi;
            return new Int64(ahi, alo);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_SubtractionInt32(System.Int64,System.Int32)&quot;]/*"/>
        public static Int64 op_SubtractionInt32(Int64 d1, Int32 d2)
        {
            uint alo = d1.Lo;
            uint blo = (uint)d2;
            uint ahi = d1.Hi;
            uint bhi = (d2 < 0) ? UInt32.MaxValue : 0;

            UInt32 dlo, dhi;
            dlo = alo - blo;
            if (alo < blo) --ahi;	// Borrow
            dhi = ahi - bhi;
            return new Int64(dhi, dlo);
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_MultiplyInt32(System.Int64,System.Int32)&quot;]/*"/>
        public static Int64 op_MultiplyInt32(Int64 d1, Int32 d2)
        {
            UInt32 hi = (d2 < 0) ? UInt32.MaxValue : 0;
            Int128 multResult = UInt64.Mult64by64to128(d1.Lo, d1.Hi, (UInt32)d2, hi);
            return new Int64(multResult.c1, multResult.c0);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_DivisionInt32(System.Int64,System.Int32)&quot;]/*"/>
        public static Int64 op_DivisionInt32(Int64 d1, Int32 d2)
        {
            return my_op_Division(d1, FromInt32(d2));
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_ModulusInt32(System.Int64,System.Int32)&quot;]/*"/>
        public static Int64 op_ModulusInt32(Int64 d1, Int32 d2)
        {
            return my_op_Modulus(d1, FromInt32(d2));
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_BitwiseAndInt32(System.Int64,System.Int32)&quot;]/*"/>
        public static Int64 op_BitwiseAndInt32(Int64 d1, Int32 d2)
        {
            return new Int64(d1.m_hi & ((d2 < 0) ? UInt32.MaxValue : 0), d1.m_lo & ((UInt32)d2));
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_BitwiseOrInt32(System.Int64,System.Int32)&quot;]/*"/>
        public static Int64 op_BitwiseOrInt32(Int64 d1, Int32 d2)
        {
            return new Int64(d1.m_hi | ((d2 < 0) ? UInt32.MaxValue : 0), d1.m_lo | ((UInt32)d2));
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_ExclusiveOrInt32(System.Int64,System.Int32)&quot;]/*"/>
        public static Int64 op_ExclusiveOrInt32(Int64 d1, Int32 d2)
        {
            return new Int64(d1.m_hi ^ ((d2 < 0) ? UInt32.MaxValue : 0), d1.m_lo ^ ((UInt32)d2));
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_EqualityInt32(System.Int64,System.Int32)&quot;]/*"/>
        public static bool op_EqualityInt32(Int64 d1, Int32 d2)
        {
            return Compare(d1, FromInt32(d2)) == 0;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_InequalityInt32(System.Int64,System.Int32)&quot;]/*"/>
        public static bool op_InequalityInt32(Int64 d1, Int32 d2)
        {
            return Compare(d1, FromInt32(d2)) != 0;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_GreaterThanInt32(System.Int64,System.Int32)&quot;]/*"/>
        public static bool op_GreaterThanInt32(Int64 d1, Int32 d2)
        {
            return Compare(d1, FromInt32(d2)) > 0;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_GreaterThanOrEqualInt32(System.Int64,System.Int32)&quot;]/*"/>
        public static bool op_GreaterThanOrEqualInt32(Int64 d1, Int32 d2)
        {
            return Compare(d1, FromInt32(d2)) >= 0;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_LessThanInt32(System.Int64,System.Int32)&quot;]/*"/>
        public static bool op_LessThanInt32(Int64 d1, Int32 d2)
        {
            return Compare(d1, FromInt32(d2)) < 0;
        }
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.Int64.op_LessThanOrEqualInt32(System.Int64,System.Int32)&quot;]/*"/>
        public static bool op_LessThanOrEqualInt32(Int64 d1, Int32 d2)
        {
            return Compare(d1, FromInt32(d2)) <= 0;
        }
        #endregion Operators 32 bit signed
    }

    internal class Int128
    {
        public uint c0;
        public uint c1;
        public uint c2;
        public uint c3;
    }
}
