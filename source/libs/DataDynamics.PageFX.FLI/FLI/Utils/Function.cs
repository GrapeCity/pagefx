using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DataDynamics.PageFX
{
    class Function
    {
        public string Name { get; set; }

        #region class Argument
        public class Argument
        {
            public string Name { get; set; }

            public object Value { get; set; }

            public override string ToString()
            {
                if (!string.IsNullOrEmpty(Name))
                    return Name + "=" + FormatValue(Value);
                return FormatValue(Value);
            }

            private static string FormatValue(object v)
            {
                if (v == null) return "null";
                if (v is bool) return (bool)v ? "true" : "false";
                var s = v as string;
                if (s != null)
                    return Escaper.Escape(s, true);
                var c = v as IConvertible;
                if (c != null)
                {
                    double d = c.ToDouble(null);
                    return d.ToString(CultureInfo.InvariantCulture);
                }
                return v.ToString();
            }
        }
        #endregion

        public Argument this[int index]
        {
            get { return _args[index]; }
        }

        public List<Argument> Arguments
        {
            get { return _args; }
        }

        readonly List<Argument> _args = new List<Argument>();

        public Argument Find(string name, bool ignoreCase)
        {
        	return _args.FirstOrDefault(a => a.Name != null
        	                                 && string.Compare(a.Name, name,
        	                                                   ignoreCase
        	                                                   	? StringComparison.CurrentCultureIgnoreCase
        	                                                   	: StringComparison.CurrentCulture) == 0);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Name);
            sb.Append('(');
            int n = _args.Count;
            for (int i = 0; i < n; ++i)
            {
                if (i > 0) sb.Append(", ");
                sb.Append(_args[i].ToString());
            }
            sb.Append(')');
            return sb.ToString();
        }

        #region Parsing
        public static Function Parse(string s)
        {
            return Parse(new StringReader(s));
        }

        public static Function Parse(TextReader reader)
        {
            int c = reader.Read();

            ParseHelper.SkipWhiteSpace(reader, ref c);
            if (c < 0) return null; //unexpected eof

            string name = ParseHelper.ReadSimpleId(reader, ref c);
            if (c < 0) return null; //unexpected eof
            if (string.IsNullOrEmpty(name)) //bad name
                return null;

            ParseHelper.SkipWhiteSpace(reader, ref c);
            if (c < 0) return null; //unexpected eof

            if (c != '(') //no function start
                return null;

            return ParseArgs(reader, name, ref c);
        }

        static Function ParseArgs(TextReader reader, string name, ref int c)
        {
            var func = new Function { Name = name };

            c = reader.Read(); //eat '('
            if (c < 0) return null; //unexpected eof

            //args
            bool comma = false;
            while (true)
            {
                ParseHelper.SkipWhiteSpace(reader, ref c);
                if (c < 0) return null; //unexpected eof

                if (c == ')')
                {
                    c = reader.Read();
                    break;
                }

                if (comma && c == ',')
                {
                    c = reader.Read();
                    if (c < 0) return null; //unexpected eof

                    ParseHelper.SkipWhiteSpace(reader, ref c);
                    if (c < 0) return null; //unexpected eof
                }

                string id;
                var val = ParseValue(reader, ref c, out id);

                if (id != null)
                {
                    ParseHelper.SkipWhiteSpace(reader, ref c);
                    if (c == '=') //name = value
                    {
                        c = reader.Read();
                        string argname = id;
                        val = ParseValue(reader, ref c, out id);
                        if (id != null) val = IdToValue(id);
                        func.Arguments.Add(new Argument { Name = argname, Value = val });
                    }
                    else
                    {
                        val = IdToValue(id);
                        func.Arguments.Add(new Argument { Value = val });
                    }
                }
                else
                {
                    func.Arguments.Add(new Argument { Value = val });
                }

                comma = true;
            }

            return func;
        }

        static object IdToValue(string id)
        {
            if (string.Compare(id, "true", true) == 0)
                return true;
            if (string.Compare(id, "false", true) == 0)
                return false;
            if (string.Compare(id, "null", true) == 0)
                return null;
            return id;
        }

        static object ParseValue(TextReader reader, ref int c, out string id)
        {
            id = null;
            ParseHelper.SkipWhiteSpace(reader, ref c);

            if (ParseHelper.IsSimpleIdStart(c))
            {
                id = ParseHelper.ReadSimpleId(reader, ref c);

                ParseHelper.SkipWhiteSpace(reader, ref c);
                if (c == '(')
                {
                    string name = id;
                    id = null;
                    return ParseArgs(reader, name, ref c);
                }

                return null;
            }

            if (ParseHelper.IsStringStart(c))
                return ParseHelper.ParseString(reader, ref c);

            double num;
            if (ParseNumberOrID(reader, ref c, out num, out id))
            {
                if (id != null)
                {
                    ParseHelper.SkipWhiteSpace(reader, ref c);
                    if (c == '(')
                    {
                        string name = id;
                        id = null;
                        return ParseArgs(reader, name, ref c);
                    }
                    return id;
                }
                return num;
            }

            throw UnexpectedChar(c);
        }

        static Exception UnexpectedChar(int c)
        {
            return new FormatException(string.Format("Unexpected character '{0}'", (char)c));
        }

        static bool ParseNumberOrID(TextReader reader, ref int c, out double num, out string id)
        {
            num = 0;
            id = null;
            string s = "";
            if (c == '-')
            {
                c = reader.Read();
                id = ParseHelper.ReadSimpleId(reader, ref c);
                if (id != null)
                {
                    id = "-" + id;
                    return true;
                }

                if (char.IsDigit((char)c))
                {
                    s = "-";
                    FinishNumberString(reader, ref c, ref s);
                    num = ParseDouble(s);
                    return true;
                }

                throw UnexpectedChar(c);
            }

            if (c == '+')
            {
                s = "+";
                c = reader.Read();
            }

            if (char.IsDigit((char)c))
            {
                FinishNumberString(reader, ref c, ref s);
                num = ParseDouble(s);
                return true;
            }

            return false;
        }

        static void FinishNumberString(TextReader reader, ref int c, ref string s)
        {
            s += ParseHelper.ReadDigitSequence(reader, ref c);
            if (c == '.')
            {
                c = reader.Read();
                s += '.';
                s += ParseHelper.ReadDigitSequence(reader, ref c);
            }
            if (c == 'e' || c == 'E')
            {
                s += (char)c;
                c = reader.Read();
                if (c == '+' || c == '-')
                {
                    s += (char)c;
                    c = reader.Read();
                    s += ParseHelper.ReadDigitSequence(reader, ref c);
                }
                else
                {
                    throw new FormatException("Invalid number exponent");
                }
            }
        }

        static double ParseDouble(string s)
        {
            return double.Parse(s, NumberStyles.Float, CultureInfo.InvariantCulture);
        }

    	#endregion
    }
}