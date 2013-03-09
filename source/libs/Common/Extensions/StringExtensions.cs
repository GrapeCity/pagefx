using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DataDynamics.PageFX.Common.Extensions
{
    public static class StringExtensions
    {
		public static int IndexOf(this string s, Func<char,int,bool> predicate)
		{
			if (string.IsNullOrEmpty(s)) return -1;

			for (int i = 0; i < s.Length; i++)
			{
				if (predicate(s[i], i))
					return i;
			}

			return -1;
		}

    	public static string Unquote(this string s)
        {
            return s.Unquote(true);
        }

        public static string Unquote(this string s, bool apos)
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

		public static string[] ReadLines(this TextReader reader, bool trim, Func<string, bool> lineFilter)
		{
			var list = new List<string>();
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				if (trim) line = line.Trim();
				if (!lineFilter(line)) continue;
				list.Add(line);
			}
			return list.ToArray();
		}

		public static string[] ReadLines(this Stream stream, bool trim, Func<string, bool> lineFilter)
		{
			using (var reader = new StreamReader(stream))
				return reader.ReadLines(trim, lineFilter);
		}

        public static string[] ReadLines(this TextReader reader, bool allowEmptyLines, bool trim)
        {
        	return reader.ReadLines(trim, line => line.Length != 0 || allowEmptyLines);
        }

        public static string[] ReadLines(this TextReader reader)
        {
            return reader.ReadLines(true, false);
        }

        public static string[] ReadLines(this string text, bool allowEmptyLines, bool trim)
        {
            if (String.IsNullOrEmpty(text))
                return new string[0];
            using (var reader = new StringReader(text))
            {
                return reader.ReadLines(allowEmptyLines, trim);
            }
        }

        public static string[] ReadLines(this string text)
        {
            return text.ReadLines(true, false);
        }

    	public static string IndentLines(this string text, string tab)
        {
            if (String.IsNullOrEmpty(tab))
                return text;
            var sb = new StringBuilder();
            foreach (var line in text.ReadLines())
            {
                sb.AppendLine(tab + line);
            }
            return sb.ToString();
		}

		#region Join

		public static string Join<T>(this IEnumerable<T> seq, string prefix, string suffix, string separator, Func<T, string> tostr)
        {
			if (seq == null) return "";
            var sb = new StringBuilder();
            bool f = false;
			if (!string.IsNullOrEmpty(prefix))
			{
				sb.Append(prefix);
			}
    		foreach (var item in seq)
            {
                if (f) sb.Append(separator);
                sb.Append(tostr(item));
                f = true;
            }
			if (!string.IsNullOrEmpty(suffix))
			{
				sb.Append(suffix);
			}
    		return sb.ToString();
        }

        public static string Join<T>(this IEnumerable<T> seq, string prefix, string suffix, string separator)
        {
            return seq.Join(prefix, suffix, separator, x => x.ToString());
        }

        public static string Join<T>(this IEnumerable<T> seq, string separator)
        {
            return seq.Join("", "", separator);
        }

        public static string Join<T>(this IEnumerable<T> seq)
        {
            return seq.Join(",");
		}

		#endregion

		public static void Append<T>(this StringBuilder sb, IEnumerable<T> seq, Func<T,string> convert, string separator)
		{
			bool sep = false;
			foreach (var item in seq)
			{
				if (sep && !string.IsNullOrEmpty(separator))
					sb.Append(separator);
				if (convert == null)
					sb.Append(item);
				else
					sb.Append(convert(item));
				sep = true;
			}
		}

		public static List<string> Break(this string line, int maxWidth)
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

        private static int LeftWhiteSpace(string s, int i)
        {
            while (i >= 0)
            {
                if (Char.IsWhiteSpace(s[i]))
                    break;
                --i;
            }
            if (i < 0)
                return 0;
            return i;
        }

    	#region ReplaceVars
        private static readonly Regex[] Schemes = new Regex[3];
        
        public static string ReplaceVars(this string template, RVScheme scheme, params string[] vars)
        {
            if (String.IsNullOrEmpty(template))
                throw new ArgumentException("template");
            if (vars == null)
                throw new ArgumentNullException("vars");
            int n = vars.Length;
            if (n <= 1)
                throw new ArgumentException("vars");
            if ((n & 1) != 0)
                throw new ArgumentException("vars.Length is not even", "vars");

            var rx = Schemes[(int)scheme];
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

					default:
						throw new NotSupportedException();
                }
                Schemes[(int)scheme] = rx;
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
                m =>
	                {
		                string var = m.Groups["var"].Value;
		                string v = vh[var] as string;
		                if (v != null)
		                {
			                if (String.Compare(v, "newguid()", true) == 0)
				                v = Guid.NewGuid().ToString("D");
			                return v;
		                }
		                return var;
	                });
        }

        public static string ReplaceVars(this string template, params string[] vars)
        {
            return template.ReplaceVars(RVScheme.DollarID, vars);
        }
        #endregion

    	public static string ToXmlString(this string s)
    	{
    		if (String.IsNullOrEmpty(s)) return s;
    		var sb = new StringBuilder();
    		foreach (var c in s)
    		{
    			if (c == '<') sb.Append("&lt;");
    			else if (c == '>') sb.Append("&gt;");
    			else if (c == '&') sb.Append("&amp;");
    			else if (c == '\'') sb.Append("&apos;");
    			else if (c == '\"') sb.Append("&quot;");
    			else if (c == 0x1B || c == 0x0C) //TODO: find/include other invalid chars
    			{
    				sb.AppendFormat("&#x{0:X};", (int)c);
    			}
    			else
    			{
    				sb.Append(c);
    			}
    		}
    		return sb.ToString();
    	}
    }

    public enum RVScheme
    {
        DollarID,
        ID,
        Ant
    }
}