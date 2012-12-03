using System;
using System.Collections;
using System.IO;
using System.Text;

namespace DataDynamics.PageFX.Common.Utilities
{
    public interface ITextTemplateContext
    {
        string Eval(string statement, string var);
    }

    public static class TextTemplateEngine
    {
        private const string StatementFor = "for";
        private const string StatementEndFor = "endfor";

        private const string StatementIf = "if";
        private const string StatementElseIf = "elseif";
        private const string StatementElse = "else";
        private const string StatementEndIf = "endif";

        public static string Replace(string text, ITextTemplateContext context)
        {
            if (text == null) return null;
            using (var writer = new StringWriter())
            {
                var stack = new Stack();
                int n = text.Length;
                string ws = "";
                string s;
                int i = 0;
                while (i < n)
                {
                    char c = text[i];
                    if (c == '$')
                    {
                        if (i + 1 >= n)
                        {
                            WriteReset(writer, ref ws);
                            writer.Write(c);
                            break;
                        }
                        s = ReadStatement(text, ref i);
                        if (s.Length == 0)
                        {
                            WriteReset(writer, ref ws);
                            writer.Write('$');
                        }
                        if (s.StartsWith(StatementFor))
                        {
                            ws = "";
                            var names = s.Split(' ', '\t');
                            SkipEol(text, ref i);
                            string var = names[1];
                            if (string.IsNullOrEmpty(context.Eval(StatementFor, var)))
                            {
                                SkipBlock(text, ref i, StatementEndFor);
                            }
                            else
                            {
                                var f = new For();
                                f.var = var;
                                f.index = i;
                                f.useeol = names.Length >= 3 && names[2] == "eol";
                                stack.Push(f);
                            }
                        }
                        else if (s == StatementEndFor)
                        {
                            ws = "";
                            SkipEol(text, ref i);
                            var f = stack.Peek() as For;
                            if (f == null)
                                throw new NullReferenceException();
                            if (string.IsNullOrEmpty(context.Eval(StatementFor, f.var)))
                            {
                                //remove 'for' from stack
                                stack.Pop();
                            }
                            else
                            {
                                i = f.index;
                                if (f.useeol)
                                {
                                    if (f.eol)
                                    {
                                        writer.WriteLine();
                                        f.eol = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            WriteReset(writer, ref ws);
                            s = context.Eval(null, s);
                            SafeWrite(writer, s);

                            if (stack.Count > 0)
                            {
                                var f = stack.Peek() as For;
                                if (f != null)
                                {
                                    if (f.useeol)
                                    {
                                        f.eol = true;
                                    }
                                }
                            }
                        }
                    }
                    else if (c == ' ' || c == '\t')
                    {
                        ws += c;
                        ++i;
                    }
                    else
                    {
                        WriteReset(writer, ref ws);
                        writer.Write(c);
                        ++i;
                    }
                }
                WriteReset(writer, ref ws);
                return writer.ToString();
            }
        }

        private abstract class Statement
        {
        }

        private class For : Statement
        {
            public string var;
            public int index;
            public bool useeol;
            public bool eol;
        }

        //TODO:
        private class If : Statement
        {
            public string var;
        }

        private static void SafeWrite(TextWriter writer, string s)
        {
            if (!string.IsNullOrEmpty(s))
                writer.Write(s);
        }

        private static void WriteReset(TextWriter writer, ref string s)
        {
            if (s.Length > 0)
            {
                writer.Write(s);
                s = "";
            }
        }

        private static void SkipEol(string text, ref int i)
        {
            int n = text.Length;
            if (i < n)
            {
                if (text[i] == '\r')
                {
                    ++i;
                    if (i < n && text[i] == '\n')
                        ++i;
                }
                else if (text[i] == '\n')
                {
                    ++i;
                }
            }
        }

        private static string ReadStatement(string text, ref int i)
        {
            var s = new StringBuilder(10);
            int n = text.Length;
            for (++i; i < n; ++i)
            {
                if (text[i] == '$')
                {
                    ++i;
                    break;
                }
                s.Append(text[i]);
            }
            return s.ToString();
        }

        private static void SkipBlock(string text, ref int i, string until)
        {
            int n = text.Length;
            for (; i < n; ++i)
            {
                if (text[i] == '$')
                {
                    string s = ReadStatement(text, ref i);
                    if (s == until)
                    {
                        SkipEol(text, ref i);
                        break;
                    }
                }
            }
        }
    }
}