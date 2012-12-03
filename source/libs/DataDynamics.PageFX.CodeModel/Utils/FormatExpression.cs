using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DataDynamics
{
    public class FormatExpression
    {
        static readonly StringBuilder SB = new StringBuilder();

		public interface IEvaluator
		{
			string Evaluate(string expression);
		}

        public static string Eval(IEvaluator frame, string exp)
        {
            if (string.IsNullOrEmpty(exp)) return exp;
            var list = Parse(exp);
            if (list == null) return "";
            SB.Length = 0;
            foreach (var item in list)
            {
                string s = item.Eval(frame);
                if (s != null)
                    SB.Append(s);
            }
            return SB.ToString();
        }

        #region Parsing
        static IEnumerable<IItem> Parse(string exp)
        {
            return Parse(new StringReader(exp));
        }

        static IEnumerable<IItem> Parse(TextReader reader)
        {
            var list = new List<IItem>();
            SB.Length = 0;
            bool isexp = false;
            while (true)
            {
                int c = reader.Read();
                if (c < 0) break;

                if (isexp)
                {
                    if (c == '{')
                        throw Error("Complex expressions are not supported");

                    if (c == '}') //end of expression
                    {
                        Add(list, true);
                        isexp = false;
                        continue;
                    }

                    SB.Append((char)c);
                    continue;
                }

                if (c == '{')
                {
                    c = reader.Read();
                    if (c < 0)
                        throw Error("Unexpected end");

                    if (c == '{') // '{'
                    {
                        SB.Append('{');
                        continue;
                    }

                    if (c == '}') //empty expression
                        continue;

                    Add(list, false);
                    SB.Append((char)c);
                    isexp = true;
                    continue;
                }

                if (c == '}')
                {
                    c = reader.Read();
                    if (c < 0)
                        throw Error("Unexpected end");

                    if (c == '}') // '{'
                    {
                        SB.Append('}');
                        continue;
                    }

                    throw Error("Unexpected '}'");
                }
                SB.Append((char)c);
                continue;
            }

            if (isexp)
                throw Error("Expression is not completed");

            Add(list, false);

            return list;
        }

        static Exception Error(string format, params object[] args)
        {
            return new FormatException(string.Format(format, args));
        }

        static void Add(ICollection<IItem> list, bool isexp)
        {
            if (SB.Length != 0)
            {
                string s = SB.ToString();
                if (isexp)
                {
                    s = s.Trim();
                    if (!string.IsNullOrEmpty(s))
                        list.Add(new Exp(s));
                }
                else
                {
                    list.Add(new Str(s));
                }
                SB.Length = 0;
            }
        }
        #endregion

        #region Items
        interface IItem
        {
            string Eval(IEvaluator evaluator);
        }

        private sealed class Str : IItem
        {
            readonly string _value;

            public Str(string value)
            {
                _value = value;
            }

            public string Eval(IEvaluator evaluator)
            {
                return _value;
            }

            public override string ToString()
            {
                return _value;
            }
        }

        private sealed class Exp : IItem
        {
            public Exp(string exp)
            {
                _exp = exp;
            }

            readonly string _exp;

            public string Eval(IEvaluator evaluator)
            {
                try
                {
                    return evaluator.Evaluate(_exp);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                    return "Error";
                }
            }

            public override string ToString()
            {
                return "{" + _exp + "}";
            }
        }
        #endregion
    }
}