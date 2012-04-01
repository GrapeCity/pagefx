using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;

namespace DataDynamics
{
    public static class ParseHelper
    {
        #region Is
        public static bool IsHexDigit(int c)
        {
            return (c >= '0' && c <= '9')
                   || (c >= 'a' && c <= 'z')
                   || (c >= 'A' && c <= 'Z');
        }

        public static bool IsHexDigit(char c)
        {
            return (c >= '0' && c <= '9')
                   || (c >= 'a' && c <= 'z')
                   || (c >= 'A' && c <= 'Z');
        }

        public static bool IsHex(string s)
        {
        	return s != null && s.All(IsHexDigit);
        }

    	public static int ParseHexInt32(this string s)
        {
            return int.Parse(s, NumberStyles.HexNumber, null);
        }

        public static bool IsNumberStart(int c)
        {
            return c == '-' || c == '+' || char.IsDigit((char)c);
        }

        public static bool IsStringStart(int c)
        {
            return c == '\"' || c == '\'';
        }

        public static bool IsSimpleIdStart(int c)
        {
            return char.IsLetter((char)c) || c == '_';
        }

        public static bool IsSimpleIdChar(int c)
        {
            return IsSimpleIdStart(c) || char.IsDigit((char)c);
        }

        public static bool IsSimpleId(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            if (!IsSimpleIdStart(s[0]))
                return false;
            int n = s.Length;
            for (int i = 1; i < n; ++i)
            {
                if (!IsSimpleIdChar(s[i]))
                    return false;
            }
            return true;
        }
        #endregion

        #region SkipWhiteSpace, Skip
        public static void SkipWhiteSpace(TextReader reader, ref int c)
        {
            while (char.IsWhiteSpace((char)c))
                c = reader.Read();
        }

        public static bool SkipOne(TextReader reader, ref int c, IEnumerable<char> set)
        {
        	if (set == null) return false;
        	if (set.Contains((char)c))
        	{
        		c = reader.Read();
        		return true;
        	}
        	return false;
        }

        public static int Skip(string s, int index, Predicate<char> p)
        {
            if (s == null)
                throw new ArgumentNullException("index");
            int n = s.Length;
            for (; index < n; ++index)
            {
                if (!p(s[index]))
                    return index;
            }
            return -1; //end of string
        }
        #endregion

        #region ReadSimpleId
        public static string ReadSimpleId(TextReader reader, ref int c)
        {
            if (IsSimpleIdStart(c))
            {
                string s = "";
                s += (char)c;
                c = reader.Read();
                while (IsSimpleIdChar(c))
                {
                    s += (char)c;
                    c = reader.Read();
                }
                return s;
            }
            return null;
        }
        #endregion

        #region ReadSimpleValue
        public static string ReadSimpleValue(TextReader reader, ref int c, IEnumerable<char> end)
        {
            var value = new StringBuilder();
            SkipWhiteSpace(reader, ref c);
            if (c == '\"' || c == '\'')
            {
                char q = (char)c;
                value.Append(q);
                c = reader.Read();
                while (true)
                {
                    if (c < 0)
                    {
                        //log.Error(ErrorCodes.CommaneLine_UnterminatedQuote, "Unterminated quote. Context: {0}",
                        //          value.ToString());
                        return null;
                    }
                    if (c == q)
                    {
                        value.Append((char)c);
                        c = reader.Read();
                        break;
                    }
                    value.Append((char)c);
                    c = reader.Read();
                }
                return Str.Unquote(value.ToString());
            }
            while (true)
            {
                if (c < 0) break;
                if (end != null && end.Contains((char)c)) break;
                if (char.IsWhiteSpace((char)c)) break;
                value.Append((char)c);
                c = reader.Read();
            }
            return value.ToString();
        }
        #endregion

        #region ParseKeyValuePairs
        public static Dictionary<string, string> ParseKeyValuePairs(string inputString, IEnumerable<char> valueStart, IEnumerable<char> pairEnd)
        {
            if (valueStart == null)
                throw new ArgumentNullException();

            var dic = new Dictionary<string, string>();

            var reader = new StringReader(inputString);
            int c = reader.Read();
            while (c >= 0)
            {
                SkipWhiteSpace(reader, ref c);
                if (c < 0) break;

                string name = ReadSimpleId(reader, ref c);
                if (string.IsNullOrEmpty(name))
                    throw new FormatException();

                SkipWhiteSpace(reader, ref c);
                if (c < 0)
                {
                    dic.Add(name, "");
                    break;
                }

                if (SkipOne(reader, ref c, valueStart))
                {
                    SkipWhiteSpace(reader, ref c);
                    if (c < 0)
                    {
                        dic.Add(name, "");
                        break;
                    }

                    string value = ReadSimpleValue(reader, ref c, pairEnd);
                	dic.Add(name, string.IsNullOrEmpty(value) ? "" : value);
                }
                else //key has no value
                {
                    dic.Add(name, "");
                }

                SkipWhiteSpace(reader, ref c);
                if (c < 0) break;

                SkipOne(reader, ref c, pairEnd);
            }

            return dic;
        }
        #endregion

        #region ParseString
        public static string ParseString(TextReader reader, ref int c)
        {
            if (!IsStringStart(c))
                return null;
            int q = c;
            var sb = new StringBuilder();
            while (true)
            {
                c = reader.Read();
                if (c < 0) return null;
                if (c == q)
                {
                    c = reader.Read();
                    break;
                }

                if (c == '\\')
                {
                    c = reader.Read();
                    if (c == 'u')
                    {
                        int h = 0;
                        for (int i = 0; i < 4; ++i)
                        {
                            c = reader.Read();
                            if (c < 0)
                                throw new FormatException("Unexpected end of stream");
                            if (!IsHexDigit(c))
                                throw new FormatException(
                                    string.Format("Bad unicode symbol. '{0}' is not hex digit.", (char)c));
                            h += Hex.ToDecimal(c);
                        }
                        sb.Append((char)h);
                    }
                    else if (c == 'x')
                    {
                        int h = 0;
                        int n = 0;
                        while (true)
                        {
                            c = reader.Read();
                            if (c < 0)
                                throw new FormatException("Unexpected end of stream");
                            if (!IsHexDigit(c))
                                break;
                            ++n;
                            h += Hex.ToDecimal(c);
                        }
                        if (n == 0)
                        {
                            sb.Append("\\x");
                        }
                        else
                        {
                            sb.Append((char)h);
                        }
                    }
                    else if (Escaper.UnescapeChar(ref c))
                    {
                        sb.Append((char)c);
                    }
                    else
                    {
                        sb.Append('\\');
                        sb.Append((char)c);
                    }
                }
                else
                {
                    sb.Append((char)c);
                }
            }

            return sb.ToString();
        }
        #endregion

        #region ReadDigitSequence
        public static string ReadDigitSequence(TextReader reader, ref int c)
        {
            string s = "";
            while (true)
            {
                if (!char.IsDigit((char)c)) break;
                s += (char)c;
                c = reader.Read();
            }
            return s;
        }
        #endregion

        #region CheckFormatBraceBalance
        /// <summary>
        /// Checks balance of braces in format string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool CheckFormatBraceBalance(string s)
        {
            if (string.IsNullOrEmpty(s)) return true;
            int n = s.Length;
            for (int i = 0; i < n; ++i)
            {
                char c = s[i];
                if (c == '{')
                {
                    ++i;
                    if (i >= n) return false;
                    int start = i;
                    for (; i < n; ++i)
                    {
                        c = s[i];
                        if (c == '{')
                        {
                            if (i == start)
                                break;
                            return false;
                        }
                        if (c == '}')
                            break;
                    }
                }
                else if (c == '}')
                {
                    ++i;
                    if (i >= n) return false;
                    if (s[i] != '}') return false;
                }
            }
            return true;
        }
        #endregion
    }
}
