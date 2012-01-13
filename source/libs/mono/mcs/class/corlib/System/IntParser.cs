using System.Globalization;

namespace System
{
    internal class IntParser
    {
        #region INumber
        public interface INumber
        {
            bool IsHexOverflow();

            void AddHexDigit(uint d);

            bool IsOverflow();

            void AddDigit(int d);

            bool CatchOverflowException { get; }

            void BeforeLoop(IntParser parser);

            void AfterLoop(IntParser parser);

            bool CheckNegative();
        }
        #endregion

        public bool AllowCurrencySymbol;
        public bool AllowHexSpecifier;
        public bool AllowThousands;
        public bool AllowDecimalPoint;
        public bool AllowParentheses;
        public bool AllowTrailingSign;
        public bool AllowLeadingSign;
        public bool AllowTrailingWhite;
        public bool AllowLeadingWhite;
        public bool AllowExponent;

        public bool tryParse;
        public Exception exc;
        public NumberFormatInfo nfi;
        public string s;
        public int pos;

        public bool foundOpenParentheses = false;
        public bool negative = false;
        public bool foundSign = false;
        public bool foundCurrency = false;
        private bool foundPoint;

        #region ctor
        public IntParser(string s, NumberStyles style, IFormatProvider fp, bool tryParse)
        {
            this.s = s;
            this.tryParse = tryParse;

            if (fp != null)
            {
                Type typeNFI = typeof(NumberFormatInfo);
                nfi = (NumberFormatInfo)fp.GetFormat(typeNFI);
            }
            else
            {
                nfi = CultureInfo.CurrentCulture.NumberFormat;
            }

            AllowCurrencySymbol = (style & NumberStyles.AllowCurrencySymbol) != 0;
            AllowHexSpecifier = (style & NumberStyles.AllowHexSpecifier) != 0;
            AllowThousands = (style & NumberStyles.AllowThousands) != 0;
            AllowDecimalPoint = (style & NumberStyles.AllowDecimalPoint) != 0;
            AllowParentheses = (style & NumberStyles.AllowParentheses) != 0;
            AllowTrailingSign = (style & NumberStyles.AllowTrailingSign) != 0;
            AllowLeadingSign = (style & NumberStyles.AllowLeadingSign) != 0;
            AllowTrailingWhite = (style & NumberStyles.AllowTrailingWhite) != 0;
            AllowLeadingWhite = (style & NumberStyles.AllowLeadingWhite) != 0;
            AllowExponent = (style & NumberStyles.AllowExponent) != 0;
        }
        #endregion

        #region Entry Point
        public bool Start(INumber number)
        {
            if (!BeforeLoop()) return false;

            int nDigits = 0;
            foundPoint = false;

            number.BeforeLoop(this);

            // Number stuff
            do
            {
                if (!IsValidDigit(s[pos], AllowHexSpecifier))
                {
                    if (FindGroupSeparator())
                    {
                        if (!AllowThousands)
                        {
                            if (!tryParse)
                                exc = GetFormatException();
                            return false;
                        }
                        continue;
                    }

                    if (FindDecimalSeparator())
                    {
                        if (!AllowDecimalPoint)
                        {
                            if (!tryParse)
                                exc = GetFormatException("Decimal point is not allowed");
                            return false;
                        }
                        foundPoint = true;
                        continue;
                    }

                    break;
                }
                else if (AllowHexSpecifier)
                {
                    nDigits++;
                    if (number.IsHexOverflow())
                    {
                        if (!tryParse)
                            exc = GetOverflowException(s);
                        return false;
                    }
                    uint d = (uint)Hex2Dec(s[pos++]);
                    number.AddHexDigit(d);
                }
                else if (foundPoint)
                {
                    nDigits++;
                    // Allows decimal point as long as it's only 
                    // followed by zeroes.
                    if (s[pos++] != '0')
                    {
                        if (!tryParse)
                            exc = GetOverflowException();
                        return false;
                    }
                }
                else
                {
                    nDigits++;
                    int d = s[pos++] - '0';

                    if (number.CatchOverflowException)
                    {
                        try
                        {
                            number.AddDigit(d);
                        }
                        catch (OverflowException)
                        {
                            if (!tryParse)
                                exc = GetOverflowException();
                            return false;
                        }
                    }
                    else
                    {
                        number.AddDigit(d);
                        if (number.IsOverflow())
                        {
                            if (!tryParse)
                                exc = GetOverflowException(s);
                            return false;
                        }
                    }
                }
            } while (pos < s.Length);

            if (!AfterLoop(number, nDigits)) return false;

            return true;
        }

        private bool AfterLoop(INumber number, int nDigits)
        {
            if (nDigits == 0)
            {
                if (!tryParse)
                    exc = GetFormatException("nDigits == 0.");
                return false;
            }

            number.AfterLoop(this);
            
            if (AllowExponent)
                FindExponent(ref pos, s);

            if (AllowTrailingSign && !foundSign)
            {
                // Sign + Currency
                FindSign(ref pos, s, nfi, ref foundSign, ref negative);
                if (foundSign)
                {
                    if (AllowTrailingWhite && !JumpOverWhite(ref pos, s, true, tryParse, ref exc))
                        return false;
                    if (AllowCurrencySymbol)
                        FindCurrency(ref pos, s, nfi,
                                     ref foundCurrency);
                }
            }

            if (AllowCurrencySymbol && !foundCurrency)
            {
                // Currency + sign
                if (nfi.CurrencyPositivePattern == 3 && s[pos++] != ' ')
                {
                    if (!tryParse)
                        exc = GetFormatException("No space between number and currency symbol");
                    return false;
                }

                FindCurrency(ref pos, s, nfi, ref foundCurrency);
                if (foundCurrency)
                {
                    if (AllowTrailingWhite && !JumpOverWhite(ref pos, s, true, tryParse, ref exc))
                        return false;
                    if (!foundSign && AllowTrailingSign)
                        FindSign(ref pos, s, nfi, ref foundSign,
                                 ref negative);
                }
            }

            if (AllowTrailingWhite && pos < s.Length
                && !JumpOverWhite(ref pos, s, false, tryParse, ref exc))
                return false;

            if (foundOpenParentheses)
            {
                if (pos >= s.Length || s[pos++] != ')')
                {
                    if (!tryParse)
                        exc = GetFormatException("No room for close parens.");
                    return false;
                }
                if (AllowTrailingWhite && pos < s.Length &&
                    !JumpOverWhite(ref pos, s, false, tryParse, ref exc))
                    return false;
            }

            if (pos < s.Length && s[pos] != '\u0000')
            {
                if (!tryParse)
                    exc = GetFormatException("Did not parse entire string. pos = "
                                             + pos + " s.Length = " + s.Length);
                return false;
            }

            if (negative && !number.CheckNegative())
            {
                if (!tryParse)
                    exc = GetOverflowException("Negative number");
                return false;
            }
            return true;
        }

        private bool FindGroupSeparator()
        {
            if (FindOther(ref pos, s, nfi.NumberGroupSeparator))
                return true;

            if (AllowCurrencySymbol && FindOther(ref pos, s, nfi.CurrencyGroupSeparator))
                return true;

            return false;
        }

        private bool FindDecimalSeparator()
        {
            if (foundPoint) return false;

            if (FindOther(ref pos, s, nfi.NumberDecimalSeparator))
                return true;

            if (AllowCurrencySymbol && FindOther(ref pos, s, nfi.CurrencyDecimalSeparator))
                return true;
            return false;
        }

        private bool BeforeLoop()
        {
            pos = 0;

            if (AllowLeadingWhite && !JumpOverWhite(ref pos, s, true, tryParse, ref exc))
                return false;

            foundOpenParentheses = false;
            negative = false;
            foundSign = false;
            foundCurrency = false;

            // Pre-number stuff
            if (AllowParentheses && s[pos] == '(')
            {
                foundOpenParentheses = true;
                foundSign = true;
                negative = true; // MS always make the number negative when there parentheses
                // even when NumberFormatInfo.NumberNegativePattern != 0!!!
                pos++;
                if (AllowLeadingWhite && !JumpOverWhite(ref pos, s, true, tryParse, ref exc))
                    return false;

                if (s.Substring(pos, nfi.NegativeSign.Length) == nfi.NegativeSign)
                {
                    if (!tryParse)
                        exc = GetFormatException("Has Negative Sign.");
                    return false;
                }
                if (s.Substring(pos, nfi.PositiveSign.Length) == nfi.PositiveSign)
                {
                    if (!tryParse)
                        exc = GetFormatException("Has Positive Sign.");
                    return false;
                }
            }

            if (AllowLeadingSign && !foundSign)
            {
                // Sign + Currency
                FindSign(ref pos, s, nfi, ref foundSign, ref negative);
                if (foundSign)
                {
                    if (AllowLeadingWhite && !JumpOverWhite(ref pos, s, true, tryParse, ref exc))
                        return false;
                    if (AllowCurrencySymbol)
                    {
                        FindCurrency(ref pos, s, nfi,
                                     ref foundCurrency);
                        if (foundCurrency && AllowLeadingWhite &&
                            !JumpOverWhite(ref pos, s, true, tryParse, ref exc))
                            return false;
                    }
                }
            }

            if (AllowCurrencySymbol && !foundCurrency)
            {
                // Currency + sign
                FindCurrency(ref pos, s, nfi, ref foundCurrency);
                if (foundCurrency)
                {
                    if (AllowLeadingWhite && !JumpOverWhite(ref pos, s, true, tryParse, ref exc))
                        return false;
                    if (foundCurrency)
                    {
                        if (!foundSign && AllowLeadingSign)
                        {
                            FindSign(ref pos, s, nfi, ref foundSign, ref negative);
                            if (foundSign && AllowLeadingWhite &&
                                !JumpOverWhite(ref pos, s, true, tryParse, ref exc))
                                return false;
                        }
                    }
                }
            }
            return true;
        }
        #endregion

        #region Utils
        public static bool CheckInput(string s, NumberStyles style, bool tryParse, out Exception exc)
        {
            exc = null;

            if (s == null)
            {
                if (!tryParse)
                    exc = new ArgumentNullException("s");
                return false;
            }

            if (s.Length == 0)
            {
                if (!tryParse)
                    exc = GetFormatException();
                return false;
            }

            if (!CheckStyle(style, tryParse, ref exc))
                return false;

            return true;
        }

        private static bool CheckStyle(NumberStyles style, bool tryParse, ref Exception exc)
        {
            if ((style & NumberStyles.AllowHexSpecifier) != 0)
            {
                NumberStyles ne = style ^ NumberStyles.AllowHexSpecifier;
                if ((ne & NumberStyles.AllowLeadingWhite) != 0)
                    ne ^= NumberStyles.AllowLeadingWhite;
                if ((ne & NumberStyles.AllowTrailingWhite) != 0)
                    ne ^= NumberStyles.AllowTrailingWhite;
                if (ne != 0)
                {
                    if (!tryParse)
                        exc = new ArgumentException(
                            "With AllowHexSpecifier only " +
                            "AllowLeadingWhite and AllowTrailingWhite " +
                            "are permitted.");
                    return false;
                }
            }

            return true;
        }

        internal static bool JumpOverWhite(ref int pos, string s, bool reportError, bool tryParse, ref Exception exc)
        {
            while (pos < s.Length && Char.IsWhiteSpace(s[pos]))
                pos++;

            if (reportError && pos >= s.Length)
            {
                if (!tryParse)
                    exc = GetFormatException();
                return false;
            }

            return true;
        }

        internal static Exception GetFormatException()
        {
            return new FormatException("Input string was not in the correct format");
        }

        internal static Exception GetFormatException(string reason)
        {
            return new FormatException("Input string was not in the correct format: " + reason);
        }

        internal static Exception GetOverflowException()
        {
            return new OverflowException("Value too large or too small.");
        }

        internal static Exception GetOverflowException(string value)
        {
            return new OverflowException("Value " + value + " too large or too small.");
        }

        internal static void FindSign(ref int pos, string s, NumberFormatInfo nfi,
                      ref bool foundSign, ref bool negative)
        {
            if ((pos + nfi.NegativeSign.Length) <= s.Length &&
                s.IndexOf(nfi.NegativeSign, pos, nfi.NegativeSign.Length) == pos)
            {
                negative = true;
                foundSign = true;
                pos += nfi.NegativeSign.Length;
            }
            else if ((pos + nfi.PositiveSign.Length) < s.Length &&
                s.IndexOf(nfi.PositiveSign, pos, nfi.PositiveSign.Length) == pos)
            {
                negative = false;
                pos += nfi.PositiveSign.Length;
                foundSign = true;
            }
        }

        internal static void FindCurrency(ref int pos, string s, NumberFormatInfo nfi, ref bool foundCurrency)
        {
            if ((pos + nfi.CurrencySymbol.Length) <= s.Length &&
                 s.Substring(pos, nfi.CurrencySymbol.Length) == nfi.CurrencySymbol)
            {
                foundCurrency = true;
                pos += nfi.CurrencySymbol.Length;
            }
        }

        internal static bool FindOther(ref int pos, string s, string other)
        {
            if ((pos + other.Length) <= s.Length &&
                 s.Substring(pos, other.Length) == other)
            {
                pos += other.Length;
                return true;
            }

            return false;
        }

        internal static bool IsHexDigit(char c)
        {
            return Char.IsDigit(c) || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');
        }

        internal static bool IsValidDigit(char c, bool allowHex)
        {
            if (allowHex)
                return IsHexDigit(c);
            return Char.IsDigit(c);
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

        internal static bool IsOverflow(int val, int sign)
        {
            if (sign == 1)
            {
                if (val < 0)
                {
                    return true;
                }
            }
            else if (val > 0)
            {
                return true;
            }
            return false;
        }

        internal static bool IsHexOverflow(uint v)
        {
            return (v & 0xf0000000u) != 0;
        }

        internal static int Hex2Dec(char hexDigit)
        {
            if (Char.IsDigit(hexDigit))
                return hexDigit - '0';
            if (Char.IsLower(hexDigit))
                return (hexDigit - 'a') + 10;
            return (hexDigit - 'A') + 10;
        }
        #endregion
    }
}