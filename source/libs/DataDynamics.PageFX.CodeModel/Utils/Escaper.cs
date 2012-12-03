using System.Text;

namespace DataDynamics
{
    public static class Escaper
    {
        #region Escape
        public const char EscapePrefix = '\\';
        public const string EscapeChars = "\'\"\t\n\r\\\v\f\a\b\0";
        public const string EscapeSuffix = "\'\"tnr\\vfab0";

        public static string EscapeChar(char c, char prefix, string chars, string suffix)
        {
            int n = chars.Length;
            for (int i = 0; i < n; ++i)
            {
                if (c == chars[i])
                    return new string(new[] { prefix, suffix[i] });
            }
            return null;
        }

        public static string EscapeChar(char c)
        {
            return EscapeChar(c, EscapePrefix, EscapeChars, EscapeSuffix);
        }

        public static string Escape(string value, bool dq)
        {
            var sb = new StringBuilder();
            sb.Append(dq ? "\"" : "\'");
            sb.Append(EscapeUnquoted(value, dq));
            sb.Append(dq ? "\"" : "\'");
            return sb.ToString();
        }

        public static string Escape(string value)
        {
            return Escape(value, true);
        }

        public static string EscapeUnquoted(string value, bool dq)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            var sb = new StringBuilder();
            int n = value.Length;
            for (int i = 0; i < n; ++i)
            {
                char c = value[i];
                if (c == '\'')
                {
                    if (dq) sb.Append('\'');
                    else sb.Append("\\\'");
                }
                else if (c == '\"')
                {
                    if (dq) sb.Append("\\\"");
                    else sb.Append('\"');
                }
                else if (c == '\\')
                {
                    if (i + 1 < n)
                    {
                        char nc = value[i + 1];
                        if (nc == '\r')
                        {
                            ++i;
                            if (i + 1 < n)
                            {
                                nc = value[i + 1];
                                if (nc == '\n')
                                {
                                    sb.Append("\\\r\n");
                                    ++i;
                                }
                                else sb.Append("\\\r");
                            }
                            else sb.Append("\\\r");
                        }
                        else if (nc == '\n')
                        {
                            sb.Append("\\\n");
                            ++i;
                        }
                        else if (nc == '\f')
                        {
                            sb.Append("\\\f");
                            ++i;
                        }
                        else
                        {
                            sb.Append("\\\\");
                        }
                    }
                    else
                    {
                        sb.Append("\\\\");
                    }
                }
                else
                {
                    string ec = EscapeChar(c);
                    if (ec != null)
                    {
                        sb.Append(ec);
                    }
                    else if (c > 255) // Unicode
                    {
                        sb.Append("\\u");
                        sb.Append(((int)c).ToString("X4"));
                    }
                    else if (!(c >= 0 && c <= 127))
                    {
                        sb.Append("\\x");
                        sb.Append(((int)c).ToString("x2"));
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
            }
            return sb.ToString();
        }
        #endregion

        #region Unescape
        public static bool IsDigit(int c)
        {
            return (c >= '0' && c <= '9');
        }

        public static bool IsHexDigit(int c)
        {
            return IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
        }

        public static bool UnescapeChar(ref char c, string chars, string suffix)
        {
            int i = suffix.IndexOf(c);
            if (i >= 0)
            {
                c = chars[i];
                return true;
            }
            return false;
        }

        public static bool UnescapeChar(ref char c)
        {
            return UnescapeChar(ref c, EscapeChars, EscapeSuffix);
        }

        public static bool UnescapeChar(ref int c)
        {
            char cc = (char)c;
            if (UnescapeChar(ref cc, EscapeChars, EscapeSuffix))
            {
                c = cc;
                return true;
            }
            return false;
        }

        public static string UnescapeString(string s, char sep, string chars, string suffix)
        {
            var res = new StringBuilder();
            int n = s.Length;
            for (int i = 0; i < n; ++i)
            {
                char c = s[i];
                if (c == sep)
                {
                    ++i;
                    if (i >= n) return null;
                    c = s[i];
                    int j = suffix.IndexOf(c);
                    if (j >= 0)
                    {
                        res.Append(chars[j]);
                    }
                    else if (c == 'u')
                    {
                        ++i;
                        if (i + 3 >= n) return null;
                        int b = 0x1000;
                        int v = 0;
                        for (j = 0; j < 4; ++j, ++i)
                        {
                            c = s[i];
                            if (!IsHexDigit(c))
                                return null;
                            v += Hex.ToDecimal(c) * b;
                            b >>= 4;
                        }
                        res.Append((char)v);
                    }
                    else if (c == 'x')
                    {
                        ++i;
                        if (i >= n) return null;
                        string h = "";
                        for (; i < n; ++i)
                        {
                            c = s[i];
                            if (!IsHexDigit(c))
                            {
                                --i;
                                break;
                            }
                            h += c;
                            if (h.Length == 4) break;
                        }
                        if (h.Length == 0) return null;
                        int v = Hex.ToDecimal(h);
                        res.Append((char)v);
                    }
                }
                else
                {
                    res.Append(c);
                }
            }
            return res.ToString();
        }

        public static string UnescapeString(string s)
        {
            return UnescapeString(s, EscapePrefix, EscapeChars, EscapeSuffix);
        }
        #endregion
    }
}