using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DataDynamics
{
    public class CommandLine
    {
        #region Inner Types
        public enum ItemType
        {
            Option,
            InputFile,
            ResponseFile
        }

        public class Item
        {
            public ItemType Type;
            public string Name;
            public string Value;

            public Item()
            {   
            }

            public Item(ItemType type, string value)
            {
                Type = type;
                Value = value;
            }

            public Item(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public bool IsOption
            {
                get { return Type == ItemType.Option; }
            }
            
            public override string ToString()
            {
                switch (Type)
                {
                    case ItemType.Option:
                        {
                            if (string.IsNullOrEmpty(Value))
                                return "/" + Name;
                            return string.Format("/{0}:{1}", Name, Value);
                        }
                    case ItemType.InputFile:
                        return Value;
                    case ItemType.ResponseFile:
                        return "@" + Value;
                }
                return string.Empty;
            }
        }

        public class ItemList : List<Item>
        {
            
        }
        #endregion

        #region Items
        public ItemList Items
        {
            get { return _items; }
        }
        private readonly ItemList _items = new ItemList();

        public void AddItem(Item item)
        {
            _items.Add(item);
        }

        public void AddOption(string name, string value)
        {
            _items.Add(new Item(name, value));
        }

        public int InputFileCount
        {
            get
            {
                return GetInputFiles().Length;
            }
        }
        #endregion

        #region Public Members
        public static string Unquote(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            if (s[0] == '\"') return s.Trim('\"');
            if (s[0] == '\'') return s.Trim('\'');
            return s;
        }

        public static bool Match(string s, bool ignoreCase, params string[] list)
        {
        	return list.Any(s2 => string.Compare(s, s2, ignoreCase) == 0);
        }

    	private IEnumerable<Item> Options
        {
            get { return Items.Where(item => item.Type == ItemType.Option); }
        }

        public bool HasOption(params string[] names)
        {
        	return Options.Any(item => Match(item.Name, true, names));
        }

    	public bool HasOption(CLOption opt)
        {
            if (opt == null)
                throw new ArgumentNullException("opt");
            return HasOption(opt.Names);
        }

        public bool IsMinus(CLOption opt)
        {
            if (opt == null)
                throw new ArgumentNullException("opt");

            if (!opt.PlusMinus)
                throw new ArgumentException(
                    string.Format("Option {0} has no [+|-] format", opt.Name), "opt");

            foreach (var item in Options)
            {
                string name = item.Name;
                if (opt.CheckName(name, true))
                {
                    int nn = name.Length;
                    if (name[nn - 1] == '+')
                        return false;
                    if (name[nn - 1] == '-')
                        return true;
                    if (!ParseBool(item.Value, true))
                        return true;
                }
            }

            return false;
        }

        public string[] GetOptions(params string[] names)
        {
            var list = new List<string>();
            int n = Items.Count;
            for (int i = 0; i < n; ++i)
            {
                var item = Items[i];
                if (item.Type == ItemType.Option
                    && Match(item.Name, true, names))
                {
                    list.Add(item.Value);
                }
            }
            return list.ToArray();
        }

        public void RemoveOptions(params string[] names)
        {
            for (int i = 0; i < Items.Count; ++i)
            {
                var item = Items[i];
                if (item.Type == ItemType.Option
                    && Match(item.Name, true, names))
                {
                    Items.RemoveAt(i);
                    --i;
                }
            }
        }

        public string GetOption(string defval, params string[] names)
        {
            var opts = GetOptions(names);
            if (opts != null && opts.Length > 0)
                return opts[opts.Length - 1];
            return defval;
        }

        public string GetOption(CLOption o)
        {
            if (o == null)
                throw new ArgumentNullException("o");
            return GetOption(o.DefaultValue, o.Names);
        }

        public bool GetBoolOption(bool defval, params string[] names)
        {
            string o = GetOption(defval ? "+" : "-", names);
            return ParseBool(o, defval);
        }

        static bool ParseBool(string s, bool defval)
        {
            if (string.IsNullOrEmpty(s)) return defval;
            if (s == "+") return true;
            if (s == "-") return false;
            if (string.Compare(s, "true", true) == 0) return true;
            if (string.Compare(s, "false", true) == 0) return false;
            int v;
            if (int.TryParse(s, out v))
                return v != 0;
            return defval;
        }

        public string[] GetInputFiles()
        {
            var list = new List<string>();
            int n = Items.Count;
            for (int i = 0; i < n; ++i)
            {
                var item = Items[i];
                if (item.Type == ItemType.InputFile)
                {
                    list.Add(item.Value);
                }
            }
            return list.ToArray();
        }

        public string[] GetResponseFiles()
        {
            var list = new List<string>();
            int n = Items.Count;
            for (int i = 0; i < n; ++i)
            {
                var item = Items[i];
                if (item.Type == ItemType.ResponseFile)
                {
                    list.Add(item.Value);
                }
            }
            return list.ToArray();
        }

        public string Filter(Predicate<Item> p)
        {
            var sb = new StringBuilder();
            foreach (var item in _items)
            {
                if (p(item))
                {
                    if (sb.Length > 0)
                        sb.Append(' ');
                    sb.Append(item.ToString());
                }
            }
            return sb.ToString();
        }
        #endregion

        #region Parsing
        static void SkipWhiteSpace(TextReader reader, ref int c)
        {
            while (char.IsWhiteSpace((char)c))
                c = reader.Read();
        }

        static string ReadOptionName(TextReader reader, ref int c)
        {
            var name = new StringBuilder();
            while (true)
            {
                if (c < 0) break;
                if (c == ':' || c == '/') break;
                if (char.IsWhiteSpace((char)c)) break;
                name.Append((char)c);
                c = reader.Read();
            }
            return name.ToString();
        }

        static string ReadValue(TextReader reader, ref int c)
        {
            var value = new StringBuilder();
            SkipWhiteSpace(reader, ref c);
            if (c == '\"' || c == '\'')
            {
                var q = (char)c;
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
                        c = reader.Read();
                        break;
                    }
                    value.Append((char)c);
                    c = reader.Read();
                }
            }
            while (true)
            {
                if (c < 0) break;
                //if (c == '/') break;
                if (char.IsWhiteSpace((char)c)) break;
                value.Append((char)c);
                c = reader.Read();
            }
            return value.ToString();
        }

        public static CommandLine Parse(TextReader reader)
        {
            var cl = new CommandLine();
            int c = reader.Read();
            while (true)
            {
                SkipWhiteSpace(reader, ref c);
                if (c < 0) break;

                if (c == '/') //option
                {
                    c = reader.Read();
                    string name = ReadOptionName(reader, ref c);
                    if (string.IsNullOrEmpty(name))
                    {
                        //log.Warning(ErrorCodes.CommandLine_EmptyOptionName, "Empty option name is occured");
                        continue;
                    }

                    SkipWhiteSpace(reader, ref c);
                    if (c == ':' || c == '=') //option value
                    {
                        c = reader.Read();
                        string value = ReadValue(reader, ref c);
                        if (string.IsNullOrEmpty(value))
                            break;
                        cl.AddItem(new Item(name, value));
                    }
                    else
                    {
                        cl.AddItem(new Item(name, null));
                    }
                }
                else if (c == '@') //response file
                {
                    c = reader.Read();
                    string value = ReadValue(reader, ref c);
                    if (string.IsNullOrEmpty(value))
                    {
                        //log.Warning(ErrorCodes.CommandLine_NullOrEmpty, "Response file is null or empty");
                        continue;
                    }
                    cl.AddItem(new Item(ItemType.ResponseFile, value));
                }
                else //input file
                {
                    string value = ReadValue(reader, ref c);
                    if (string.IsNullOrEmpty(value))
                    {
                        //log.Warning(ErrorCodes.CommandLine_NullOrEmpty, "Input file is null or empty");
                        continue;
                    }
                    cl.AddItem(new Item(ItemType.InputFile, value));
                }
            }
            return cl;
        }

        public static CommandLine Parse(string line)
        {
            if (string.IsNullOrEmpty(line))
                return null;
            return Parse(new StringReader(line));
        }

        public static CommandLine Parse(string[] args)
        {
            var s = new StringBuilder();
            int n = args.Length;
            for (int i = 0; i < n; ++i)
            {
                if (i > 0) s.Append(" ");
                s.Append(args[i]);
            }
            return Parse(s.ToString());
        }
        #endregion

        #region Object Overrides
        public override string ToString()
        {
            var s = new StringBuilder();
            int n = Items.Count;
            for (int i = 0; i < n; ++i )
            {
                if (i > 0) s.Append(" ");
                s.Append(Items[i].ToString());
            }
            return s.ToString();
        }
        #endregion

        #region Dump
        public void Dump(TextWriter writer)
        {
            //writer.WriteLine(ToString());
            int n = Items.Count;
            for (int i = 0; i < n; ++i)
            {
                writer.WriteLine(Items[i].ToString());
            }
        }
        #endregion

        #region Usage
        public static void Usage(IEnumerable<CLOption> options, string tab, int maxWidth)
        {
            Usage(Console.Out, options, tab, maxWidth);
        }

        public static void Usage(IEnumerable<CLOption> options, string tab)
        {
            Usage(Console.Out, options, tab, Console.BufferWidth);
        }

        public static void Usage(TextWriter writer, IEnumerable<CLOption> options, string tab, int maxWidth)
        {
            if (options == null) return;

            bool descOnNewLine;
            var cats = GroupByCategory(options, out descOnNewLine, maxWidth);
            if (cats.Count == 0) return;
            if (cats.Count == 1)
            {
                string name = cats[0][0].Category;
                if (string.IsNullOrEmpty(name))
                {
                    UsageCore(writer, cats[0], tab, maxWidth, descOnNewLine);
                    return;
                }
            }

            foreach (var cat in cats)
            {
                string name = cat[0].Category;
                if (string.IsNullOrEmpty(name))
                    name = "GENERAL OPTIONS";
                writer.WriteLine(tab + "                        - " + name + " -");
                UsageCore(writer, cat, tab, maxWidth, descOnNewLine);
                writer.WriteLine();
            }
        }

        static List<List<CLOption>> GroupByCategory(IEnumerable<CLOption> options, out bool descOnNewLine, int maxWidth)
        {
            var cats = new List<List<CLOption>>();
            int emptyIndex = -1;
            var catcache = new Hashtable();
            descOnNewLine = false;
            int maxLeft = (int)(maxWidth * 0.3);
            foreach (var opt in options)
            {
                string l = LeftOf(opt);
                if (!descOnNewLine && l.Length > maxLeft)
                    descOnNewLine = true;

                string cat = opt.Category;
                if (string.IsNullOrEmpty(cat))
                {
                    if (emptyIndex < 0)
                    {
                        emptyIndex = 0;
                        cats.Add(new List<CLOption>());
                    }
                    cats[emptyIndex].Add(opt);
                }
                else
                {
                    var list = catcache[cat] as List<CLOption>;
                    if (list == null)
                    {
                        list = new List<CLOption>();
                        cats.Add(list);
                        catcache[cat] = list;
                    }
                    list.Add(opt);
                }
            }
            return cats;
        }

        static void UsageCore(TextWriter writer, IEnumerable<CLOption> options, string tab, int maxWidth, bool descOnNewLine)
        {
            var left = new List<string>();
            var right = new List<string>();
            var right2 = new List<string>();
            int maxLeft = (int)(maxWidth * 0.3);
            foreach (var opt in options)
            {
                string l = LeftOf(opt);
                if (!descOnNewLine && l.Length > maxLeft)
                    descOnNewLine = true;
                left.Add(l);

                string r = opt.Description;
                if (!string.IsNullOrEmpty(r))
                    r = r.Trim();
                right.Add(r);

                if (!string.IsNullOrEmpty(opt.Aliases))
                    right2.Add("Aliases: " + opt.Aliases);
                else
                    right2.Add("");
            }

            if (descOnNewLine)
            {
                UsageDescOnNewLine(writer, left, right, right2, tab, maxWidth);
            }
            else
            {
                Usage(writer, left, right, right2, tab, maxWidth);
            }
        }

        static string LeftOf(CLOption opt)
        {
            string l = string.Format("/{0}", opt.Name);
            if (!string.IsNullOrEmpty(opt.Format))
                l += ":" + opt.Format;
            return l;
        }

        static void UsageDescOnNewLine(TextWriter writer,
            IList<string> left,
            IList<string> right,
            IList<string> right2,
            string tab, int maxWidth)
        {
            int n = left.Count;
            if (n <= 0) return;
            if (right.Count != n) return;
            if (right2.Count != n) return;

            if (tab == null)
                tab = "";

            string tab2 = tab + "\t";

            for (int i = 0; i < n; ++i)
            {
                string l = left[i];
                writer.Write(tab);
                writer.WriteLine(l);

                string r = right[i];
                WriteLines(writer, r, tab2, maxWidth);

                r = right2[i];
                if (!string.IsNullOrEmpty(r))
                {
                    writer.Write(tab2);
                    writer.WriteLine(r);
                }
            }
        }

        static void WriteLines(TextWriter writer, string s, string tab, int maxWidth)
        {
            var lines = Str.Break(s, maxWidth - 1);
            int n = lines.Count;
            for (int i = 0; i < n; ++i)
            {
                writer.Write(tab);
                writer.WriteLine(lines[i]);
            }
        }

        static void Usage(TextWriter writer,
            IList<string> left,
            IList<string> right,
            IList<string> right2,
            string tab, int maxWidth)
        {
            int n = left.Count;
            if (n <= 0) return;
            if (right.Count != n) return;
            if (right2.Count != n) return;

            if (tab == null)
                tab = "";

            string maxLeft = MaxLength(left);

            int maxleftLen = maxLeft.Length;
            string leftPad = new string(' ', maxleftLen + 1);

            for (int i = 0; i < n; ++i)
            {
                string l = left[i];
                l = l.PadRight(maxleftLen);

                string r = right[i];
                var lines = Str.Break(r, maxWidth - maxleftLen - 1);

                writer.Write(tab);
                writer.Write(l);
                writer.Write(" ");
                writer.WriteLine(lines[0]);
                for (int j = 1; j < lines.Count; ++j)
                {
                    writer.Write(tab);
                    writer.Write(leftPad);
                    writer.WriteLine(lines[j]);
                }

                r = right2[i];
                if (!string.IsNullOrEmpty(r))
                {
                    writer.Write(tab);
                    writer.Write(leftPad);
                    writer.WriteLine(r);
                }
            }
        }

		private static string MaxLength(IEnumerable<string> collection)
		{
			string max = "";
			foreach (var s in collection)
			{
				if (s.Length > max.Length)
				{
					max = s;
				}
			}
			return max;
		}

        public static void Logo(Assembly asm)
        {
            var ver = asm.GetName().Version;
            var title = asm.GetAttribute<AssemblyTitleAttribute>(false);
            Console.WriteLine("{0} version {1}", title.Title, ver);
            var copyright = asm.GetAttribute<AssemblyCopyrightAttribute>(false);
            if (copyright != null)
                Console.WriteLine(copyright.Copyright);
            Console.WriteLine();
        }
        #endregion

        #region Utils
        public string GetPath(string defval, params string[] names)
        {
            string path = GetOption(defval, names);
            if (string.IsNullOrEmpty(path))
                return defval;
            if (!Path.IsPathRooted(path))
                path = Path.Combine(Environment.CurrentDirectory, path);
            return path;
        }

        public string GetOutputFile()
        {
            return GetPath(null, "out", "output");
        }
        #endregion
    }
}
