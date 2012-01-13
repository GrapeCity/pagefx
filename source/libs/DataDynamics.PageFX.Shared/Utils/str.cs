using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DataDynamics
{
    public static class Str
    {
        public static string ReverseString(string s)
        {
            var c = s.ToCharArray();
            Array.Reverse(c);
            return new string(c);
        }

        public static string Unquote(string s)
        {
            return Unquote(s, true);
        }

        public static string Unquote(string s, bool apos)
        {
            if (s == null) return null;
            int l = s.Length;
            if (l < 2) return s;
            if (s[0] == '\"' && s[l - 1] == '\"')
                return s.Trim('\"');
            if (apos && s[0] == '\'' && s[l - 1] == '\'')
                return s.Trim('\'');
            return s;
        }

        public static string[] GetLines(TextReader reader, bool allowEmptyLines, bool trim)
        {
            var list = new List<string>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (trim) line = line.Trim();
                if (!allowEmptyLines && line.Length == 0) continue;
                list.Add(line);
            }
            return list.ToArray();
        }

        public static string[] GetLines(TextReader reader)
        {
            return GetLines(reader, true, false);
        }

        public static string[] GetLines(string text, bool allowEmptyLines, bool trim)
        {
            if (string.IsNullOrEmpty(text))
                return new string[0];
            using (var reader = new StringReader(text))
            {
                return GetLines(reader, allowEmptyLines, trim);
            }
        }

        public static string[] GetLines(string text)
        {
            return GetLines(text, true, false);
        }

        public static string Multiply(string s, int n)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < n; ++i)
                sb.Append(s);
            return sb.ToString();
        }

        public static string Tabulate(string text, string tab)
        {
            if (string.IsNullOrEmpty(tab))
                return text;
            var sb = new StringBuilder();
            foreach (var line in GetLines(text))
            {
                sb.AppendLine(tab + line);
            }
            return sb.ToString();
        }

        public static string[] GetSentences(string s)
        {
            //usually sentence ends with . and space.
            var list = new List<string>();
            if (string.IsNullOrEmpty(s))
                return list.ToArray();
            string line = "";
            int n = s.Length;
            for (int i = 0; i < n; ++i)
            {
                if (s[i] == '.' && i + 1 < n && char.IsWhiteSpace(s[i + 1]))
                {
                    ++i;
                    line = Normalize(line);
                    if (line.Length > 0)
                    {
                        line += ".";
                        list.Add(line);
                    }
                    line = "";
                }
                else
                {
                    line += s[i];
                }
            }
            line = Normalize(line);
            if (line.Length > 0)
            {
                line += ".";
                list.Add(line);
            }
            return list.ToArray();
        }

        public static string Normalize(string s)
        {
            var lines = GetLines(s, false, false);
            var res = new StringBuilder();
            foreach (var line in lines)
            {
                string t = line.Trim();
                if (res.Length > 0)
                    res.Append(" ");
                res.Append(t);
            }
            return res.ToString().Trim();
        }

        public static string ToString<T>(IEnumerable<T> set,
            string prefix, string suffix, string sep,
            Converter<T, string> tostr)
        {
            var sb = new StringBuilder();
            bool f = false;
            if (prefix != null)
                sb.Append(prefix);
            foreach (var item in set)
            {
                if (f) sb.Append(sep);
                sb.Append(tostr(item));
                f = true;
            }
            if (suffix != null)
                sb.Append(suffix);
            return sb.ToString();
        }

        public static string ToString<T>(IEnumerable<T> set,
            string prefix, string suffix, string sep)
        {
            return ToString(set, prefix, suffix, sep, x => x.ToString());
        }

        public static string ToString<T>(IEnumerable<T> set, string sep)
        {
            return ToString(set, "", "", sep);
        }

        public static string ToString<T>(IEnumerable<T> set)
        {
            return ToString(set, ",");
        }

        #region Break
        public static List<string> Break(string line, int maxWidth)
        {
            var lines = new List<string>();
            if (line == null)
            {
                lines.Add("");
                return lines;
            }

            if (line.Length <= maxWidth)
            {
                lines.Add(line);
                return lines;
            }

            while (line.Length > maxWidth)
            {
                int i = LeftWhiteSpace(line, maxWidth);
                string s = line.Substring(0, i);
                lines.Add(s);
                line = line.Substring(i + 1);
            }

            if (line.Length > 0)
                lines.Add(line);

            return lines;
        }

        static int LeftWhiteSpace(string s, int i)
        {
            while (i >= 0)
            {
                if (char.IsWhiteSpace(s[i]))
                    break;
                --i;
            }
            if (i < 0)
                return 0;
            return i;
        }
        #endregion

        #region ReplaceVars
        static readonly Regex[] _rxrv = new Regex[3];
        
        public static string ReplaceVars(string template, RVScheme scheme, params string[] vars)
        {
            if (string.IsNullOrEmpty(template))
                throw new ArgumentException("template");
            if (vars == null)
                throw new ArgumentNullException("vars");
            int n = vars.Length;
            if (n <= 1)
                throw new ArgumentException("vars");
            if ((n & 1) != 0)
                throw new ArgumentException("vars.Length is not even", "vars");

            Regex rx = _rxrv[(int)scheme];
            if (rx == null)
            {
                switch (scheme)
                {
                    case RVScheme.DollarID:
                        rx = new Regex(@"\$(?<var>(\w|-)+)", RegexOptions.Compiled);
                        break;

                    case RVScheme.ID:
                        rx = new Regex(@"(?<var>(\w|-)+)", RegexOptions.Compiled);
                        break;

                    case RVScheme.Ant:
                        rx = new Regex(@"\$\{(?<var>(\w|-)+)\}", RegexOptions.Compiled);
                        break;
                }
                _rxrv[(int)scheme] = rx;
            }
            
            var vh = new Hashtable();
            for (int i = 0; i < n;)
            {
                string name = vars[i++];
                string value = vars[i++];
                vh[name] = value;
            }

            return rx.Replace(
                template,
                delegate(Match m)
                    {
                        string var = m.Groups["var"].Value;
                        string v = vh[var] as string;
                        if (v != null)
                        {
                            if (string.Compare(v, "newguid()", true) == 0)
                                v = Guid.NewGuid().ToString("D");
                            return v;
                        }
                        return var;
                    });
        }

        public static string ReplaceVars(string template, params string[] vars)
        {
            return ReplaceVars(template, RVScheme.DollarID, vars);
        }
        #endregion
    }

    public enum RVScheme
    {
        DollarID,
        ID,
        Ant
    }
}