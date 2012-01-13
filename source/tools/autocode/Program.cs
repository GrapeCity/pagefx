using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Xml;
using DataDynamics.PageFX;
using DataDynamics.PageFX.FLI.IL;
using OperandType=DataDynamics.PageFX.FLI.IL.OperandType;

namespace DataDynamics
{
    public class TemplateAttribute : Attribute
    {
        public TemplateAttribute(string resource, string path)
        {
            _resource = resource;
            _path = path;
        }

        public string Resource
        {
            get { return _resource; }
        }
        private readonly string _resource;

        public string Path
        {
            get { return _path; }
        }
        private readonly string _path;
    }

    class Program
    {
        #region DumpCILOpCodes
        static void DumpCILOpCodes()
        {
            using (var writer = new StreamWriter("c:\\cli.opcodes.txt"))
            {
                var list = typeof(OpCodes).GetFields();
                writer.WriteLine(list.Length);
                bool longCodes = false;
                foreach (var fi in list)
                {
                    if (!fi.IsStatic) continue;
                    var c = (OpCode)fi.GetValue(null);
                    if (!longCodes && c.Size == 2)
                    {
                        longCodes = true;
                        writer.WriteLine();
                    }
                    writer.WriteLine(
                        "{0}, Value = 0x{1:X}, Size = {2}, OpCodeType = {3}, OperandType = {4}, FlowControl = {5}, Pop = {6}, Push = {7}",
                        c.Name, (ushort)c.Value, c.Size, c.OpCodeType, c.OperandType, c.FlowControl, c.StackBehaviourPop,
                        c.StackBehaviourPush);
                }
            }
        }
        #endregion

        #region DumpAvmInstructions
        static void DumpAvmInstructions()
        {
            var ins = Instructions.GetInstructions();
            var xws = new XmlWriterSettings();
            xws.Indent = true;
            xws.IndentChars = "  ";
            using (var writer = XmlWriter.Create("c:\\FLI.IL.xml", xws))
            {
                writer.WriteStartElement("il");
                foreach (var i in ins)
                {
                    i.DumpXml(writer);
                }
                writer.WriteEndElement();
            }
        }
        #endregion

        #region RunTemplates
        static void RunTemplates()
        {
            var asm = typeof(Program).Assembly;
            string dir = Path.GetDirectoryName(asm.Location);
            foreach (var type in asm.GetTypes())
            {
                var attr = ReflectionHelper.GetAttribute<TemplateAttribute>(type, false);
                if (attr != null)
                {
                    var ci = type.GetConstructor(Type.EmptyTypes);
                    var context = ci.Invoke(null) as ITextTemplateContext;
                    string text = ResourceHelper.GetText(asm, attr.Resource);
                    text = TextTemplateEngine.Replace(text, context);
                    string fullpath = Path.Combine(dir, attr.Path);
                    File.WriteAllText(fullpath, text);
                }
            }
        }
        #endregion

        #region GenerateAvmInstructionsWikiTable
        static string ToString(OperandType type)
        {
            switch (type)
            {
                case OperandType.ConstInt:
                    return "int";
                case OperandType.ConstUInt:
                    return "uint";
                case OperandType.ConstDouble:
                    return "double";
                case OperandType.ConstString:
                    return "string";
                case OperandType.ConstMultiname:
                    return "multiname";
                case OperandType.ConstNamespace:
                    return "namespace";
                case OperandType.MethodIndex:
                    return "method";
                case OperandType.ClassIndex:
                    return "class";
                case OperandType.ExceptionIndex:
                    return "exception";
                case OperandType.BranchTarget:
                    return "brtarget";
                case OperandType.BranchTargets:
                    return "brtargets";
                default:
                    return type.ToString().ToLower();
            }
        }

        static string GetOperands(Instruction i)
        {
            if (i.Operands == null) return string.Empty;
            int n = i.Operands.Length;
            if (n == 0) return string.Empty;
            var s = new StringBuilder();
            for (int k = 0; k < n; ++k)
            {
                if (k > 0) s.Append(", ");
                var op = i.Operands[k];
                s.Append(ToString(op.Type));
                s.Append(" ");
                s.Append(op.Name);
            }
            return s.ToString();
        }

        static void GenerateAvmInstructionsWikiTable()
        {
            using (var writer = new StreamWriter("c:\\wiki.avm.txt"))
            {
                writer.WriteLine("||'''Name'''||'''Code'''||'''Hex'''||'''Description'''||'''Operands'''||'''Pop'''||'''Push'''||'''Throws'''||'''Frame Use'''||'''Frame Set'''||");
                var arr = Instructions.GetUsedInstructions();
                Array.Sort(arr, delegate (Instruction x, Instruction y)
                                    {
                                        int n = x.Category.CompareTo(y.Category);
                                        if (n == 0)
                                        {
                                            return (int)x.Code - (int)y.Code;
                                        }
                                        return n;
                                    });

                Instruction lastCat = null;
                foreach (var i in arr)
                {
                    string cat = i.Category;
                    if (lastCat == null || cat != lastCat.Category)
                    {
                        lastCat = i;
                        writer.WriteLine("||'''{0}'''|| || || || || || || || || ||", cat);
                    }
                    writer.WriteLine("||{0}||{1}||0x{1:X2}||{2}||{3}||{4}||{5}||{6}||{7}||{8}||",
                                     i.Name, (int)i.Code, i.Description, GetOperands(i),
                                     i.StackPop, i.StackPush, i.CanThrow ? "1" : "0", i.FrameUse, i.FrameSet);
                }
            }
        }
        #endregion

        #region GenerateMDB
        static void WriteWarning(TextWriter writer)
        {
            writer.WriteLine("//");
            writer.WriteLine("// WARNING: Automatically generated file. DO NOT EDIT");
            //writer.WriteLine("// Generated at {0}", DateTime.Now.ToString("F", CultureInfo.InvariantCulture));
            writer.WriteLine("//");
            writer.WriteLine();
        }

        private static readonly Hashtable TableNames = new Hashtable();

        private static string ToCSharpType(string type)
        {
            if (type == "2") return "MdbColumnType.Int16";
            if (type == "4") return "MdbColumnType.Int32";
            if (string.Compare(type, "str", true) == 0) return "MdbColumnType.StringIndex";
            if (string.Compare(type, "blob", true) == 0) return "MdbColumnType.BlobIndex";
            if (string.Compare(type, "guid", true) == 0) return "MdbColumnType.GuidIndex";
            if (TableNames.Contains(type)) return string.Format("MdbTableId.{0}", type);
            return string.Format("MdbCodedIndex.{0}", type);
        }

        private static string ToShortType(string type)
        {
            if (type == "2") return "Int16";
            if (type == "4") return "Int32";
            if (string.Compare(type, "str", true) == 0) return "StringIndex";
            if (string.Compare(type, "blob", true) == 0) return "BlobIndex";
            if (string.Compare(type, "guid", true) == 0) return "GuidIndex";
            return type;
        }

        private static void GenerateMDB()
        {
            var asm = Assembly.GetEntryAssembly();
            var rs = ResourceHelper.GetStream(asm, "cli.mdb.xml");
            var doc = new XmlDocument();
            doc.Load(rs);

            string dir = Path.Combine(Path.GetDirectoryName(asm.Location), "..\\..\\..\\..\\libs\\DataDynamics.PageFX.CLI\\Metadata");
            string path = Path.Combine(dir, "MdbTables.cs");
            using (var writer = new StreamWriter(path))
            {
                var tables = doc.SelectNodes("//table");

                WriteWarning(writer);

                writer.WriteLine("using System;");
                writer.WriteLine("using DataDynamics.PageFX.CodeModel;");
                writer.WriteLine();

                writer.WriteLine("namespace DataDynamics.PageFX.CLI.Metadata");
                writer.WriteLine("{");

                #region enum MdbTableId
                writer.WriteLine("\t#region enum MdbTableId");
                writer.WriteLine("\t/// <summary>");
                writer.WriteLine("\t/// MDB Table Identifiers");
                writer.WriteLine("\t/// </summary>");
                writer.WriteLine("\tpublic enum MdbTableId");
                writer.WriteLine("\t{");
                for (int i = 0; i < tables.Count; ++i)
                {
                    var tableElem = (XmlElement)tables[i];
                    string name = tableElem.GetAttribute("name");
                    string id = tableElem.GetAttribute("id");
                    TableNames[name] = id;
                    writer.WriteLine("\t\t/// <summary>");
                    writer.WriteLine("\t\t/// Identifies {0} MDB Table ({1}, 0x{1:X2})", name, i);
                    writer.WriteLine("\t\t/// </summary>");
                    writer.WriteLine("\t\t{0} = {1},", name, id);
                }
                writer.WriteLine("\t}");
                writer.WriteLine("\t#endregion");
                writer.WriteLine();
                #endregion

                #region MdbColumnId
                writer.WriteLine("\t#region enum MdbColumnId");
                writer.WriteLine("\t/// <summary>");
                writer.WriteLine("\t/// MDB Columns");
                writer.WriteLine("\t/// </summary>");
                writer.WriteLine("\tpublic enum MdbColumnId");
                writer.WriteLine("\t{");
                foreach (XmlElement tableElem in tables)
                {
                    string name = tableElem.GetAttribute("name");
                    var cols = tableElem.GetElementsByTagName("col");
                    for (int i = 0; i < cols.Count; ++i)
                    {
                        var colElem = (XmlElement)cols[i];
                        string colName = colElem.GetAttribute("name");
                        writer.WriteLine("\t\t/// <summary>");
                        writer.WriteLine("\t\t/// Index of {0} column in {1} table", colName, name);
                        writer.WriteLine("\t\t/// </summary>");
                        writer.WriteLine("\t\t{0}_{1} = {2},", name, colName, i);
                    }
                }
                writer.WriteLine("\t}");
                writer.WriteLine("\t#endregion");
                writer.WriteLine();
                #endregion

                writer.WriteLine("\t/// <summary>");
                writer.WriteLine("\t/// Contains MDB Table Schemas");
                writer.WriteLine("\t/// </summary>");
                writer.WriteLine("\tpublic static class MDB");
                writer.WriteLine("\t{");

                #region CreateTable
                writer.WriteLine("\t\t#region CreateTable");
                writer.WriteLine("\t\tinternal static MdbTable CreateTable(MdbTableId id)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tswitch (id)");
                writer.WriteLine("\t\t\t{");
                foreach (XmlElement tableElem in tables)
                {
                    string name = tableElem.GetAttribute("name");
                    writer.WriteLine("\t\t\t\tcase MdbTableId.{0}: return new MdbTable(id, {0}.Columns);", name);
                }
                writer.WriteLine("\t\t\t\tdefault: throw new ArgumentOutOfRangeException(\"id\");");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine("\t\t#endregion");
                writer.WriteLine();
                #endregion

                #region Columns
                writer.WriteLine("\t\t#region Columns");
                foreach (XmlElement tableElem in tables)
                {
                    var cols = tableElem.GetElementsByTagName("col");

                    string tableName = tableElem.GetAttribute("name");
                    string id = tableElem.GetAttribute("id");
                    string desc = tableElem.GetAttribute("desc");
                    string rf = tableElem.GetAttribute("ref");
                    if (string.IsNullOrEmpty(desc))
                    {
                        desc = string.Format("{0} : {1}", tableName, id);
                    }
                    if (!string.IsNullOrEmpty(rf))
                    {
                        desc = "22." + rf + " " + desc;
                    }

                    writer.WriteLine("\t\t/// <summary>");
                    writer.WriteLine("\t\t/// {0}", desc);
                    writer.WriteLine("\t\t/// </summary>");
                    writer.WriteLine("\t\tpublic static class {0}", tableName);
                    writer.WriteLine("\t\t{");

                    var colNames = new List<string>();
                    for (int i = 0; i < cols.Count; ++i)
                    {
                        var colElem = (XmlElement)cols[i];
                        string colName = colElem.GetAttribute("name");
                        colNames.Add(colName);
                        string type = colElem.GetAttribute("type");
                        string cstype = ToCSharpType(type);
                        string enumType = colElem.GetAttribute("enum");
                        desc = colElem.GetAttribute("desc");

                        string s = " " + colName + " : ";
                        if (string.IsNullOrEmpty(enumType))
                            s += ToShortType(type);
                        else
                            s += enumType;

                        writer.WriteLine("\t\t\t/// <summary>");
                        writer.WriteLine("\t\t\t/// {0}", i + "." + desc + s);
                        writer.WriteLine("\t\t\t/// </summary>");
                        if (string.IsNullOrEmpty(enumType))
                            writer.WriteLine("\t\t\tpublic static readonly MdbColumn {0} = new MdbColumn({1}, \"{0}\", {2}, \"{3}\");", colName, i, cstype, desc);
                        else
                            writer.WriteLine("\t\t\tpublic static readonly MdbColumn {0} = new MdbColumn({1}, \"{0}\", {2}, typeof({3}), \"{4}\");", colName, i, cstype, enumType, desc);
                        writer.WriteLine();
                    }

                    writer.WriteLine("\t\t\t/// <summary>");
                    writer.WriteLine("\t\t\t/// {0} Columns", tableName);
                    writer.WriteLine("\t\t\t/// </summary>");
                    writer.Write("\t\t\tinternal static readonly MdbColumn[] Columns = {");
                    for (int i = 0; i < colNames.Count; ++i)
                    {
                        if (i > 0) writer.Write(", ");
                        writer.Write(colNames[i]);
                    }
                    writer.WriteLine("};");

                    writer.WriteLine("\t\t}");
                }
                writer.WriteLine("\t\t#endregion");
                #endregion

                writer.WriteLine("\t}");

                writer.WriteLine("}");
            }
        }
        #endregion

        [STAThread]
        static void Main(string[] args)
        {
            //DumpCILOpCodes();
            //DumpAvmInstructions();
            //RunTemplates();
            //GenerateMDB();
            //GenerateAvmInstructionsWikiTable();

            var cl = CommandLine.Parse(args);
            if (cl == null)
                cl = new CommandLine();

            string srcdir = cl.GetOption(null, "src", "srcdir");

            try
            {
                QA.GenerateNUnitTestFixtures(srcdir);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}