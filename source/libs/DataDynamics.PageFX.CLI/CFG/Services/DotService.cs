#if DEBUG
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using DataDynamics.PageFX.CodeModel;
using MethodBody=DataDynamics.PageFX.CLI.IL.MethodBody;

namespace DataDynamics.PageFX.CLI.CFG
{
    internal static class MethodBodyExtensions
    {
        private static string GetTypeName(IType type)
        {
            switch(type.TypeKind)
            {
                case TypeKind.Array:
                    {
                        var arr = (IArrayType)type;
                        return GetTypeName(arr.ElementType) + arr.Dimensions.Count;
                    }

                case TypeKind.Pointer:
                case TypeKind.Reference:
                    {
                        var ct = (ICompoundType)type;
                        return GetTypeName(ct.ElementType) + "&";
                    }

                default:
                    var st = type.SystemType;
                    if (st != null)
                    {
                        switch (st.Code)
                        {
                            case SystemTypeCode.Boolean: return "b";
                            case SystemTypeCode.Int8: return "i8";
                            case SystemTypeCode.UInt8: return "u8";
                            case SystemTypeCode.Int16: return "i16";
                            case SystemTypeCode.UInt16: return "u16";
                            case SystemTypeCode.Int32: return "i32";
                            case SystemTypeCode.UInt32: return "u32";
                            case SystemTypeCode.Int64: return "i64";
                            case SystemTypeCode.UInt64: return "u64";
                            case SystemTypeCode.Single: return "f32";
                            case SystemTypeCode.Double: return "f64";
                            case SystemTypeCode.Decimal: return "d";
                            case SystemTypeCode.Char: return "c";
                            case SystemTypeCode.String: return "s";
                            case SystemTypeCode.Object: return "o";
                        }
                    }
                    return type.Name;
            }
        }

        private static string GetFullName(IMethod m)
        {
            var s = new StringBuilder();
            s.Append(m.Name);
            int n = m.Parameters.Count;
            for (int i = 0; i < n; ++i)
            {
                var p = m.Parameters[i];
                s.Append("_");
                s.Append(GetTypeName(p.Type));
            }
            return s.ToString();
        }

        public static string GetTestDirectory(this MethodBody body)
        {
        	string dir = string.IsNullOrEmpty(CommonLanguageInfrastructure.TestCaseDirectory) ? "c:\\QA\\PageFX" : CommonLanguageInfrastructure.TestCaseDirectory;
            dir = Path.Combine(dir, body.Method.DeclaringType.FullName.ReplaceInvalidPathChars());
            dir = Path.Combine(dir, GetFullName(body.Method).ReplaceInvalidFileNameChars());
            Directory.CreateDirectory(dir);
            return dir;
        }
    }

    internal static class DotService
    {
        public static string GetDirectory(MethodBody body)
        {
            return body.GetTestDirectory();
        }

        public static string MakePath(MethodBody body, string name)
        {
            string dir = GetDirectory(body);
            return Path.Combine(dir, name + ".dot");
        }

        public static NameService NameService;

        public static void Write(IEnumerable<Node> graph, string path, bool subgraph)
        {
            if (NameService == null)
                NameService = new NameService();
            NameService.SetNames(graph);

            CLIDebug.LogInfo("WriteDotFile started");
            WriteDotFile(graph, path);
            CLIDebug.LogInfo("WriteDotFile succeeded");

            CLIDebug.LogInfo("dot.exe started");
            int start = Environment.TickCount;
            Dot.Render(path, null);
            int end = Environment.TickCount;
            CLIDebug.LogInfo("dot.exe succeeded. ellapsed time: {0}", (end - start) + "ms");
        }

        private static void WriteDotFile(IEnumerable<Node> graph, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine("digraph G");
                writer.WriteLine("{");

                string label = "CFG";
				var first = graph.FirstOrDefault();
                if (first != null && first.OwnerBlock != null)
                {
                    label = first.OwnerBlock.ToString();
                }
                writer.WriteLine("graph [label = \"{0}\"];", label);
                
                foreach (var node in graph)
                {
                    WriteNode(writer, node);
                    if (node.IsTwoWay)
                    {
                        var f = node.FirstOut;
                        var s = f.NextOut;
                        writer.WriteLine("{0} -> {1} [label=\"{2}\"];",
                                         node.Name,
                                         f.To.Name,
                                         f.Label);
                        writer.WriteLine("{0} -> {1} [label=\"{2}\"];",
                                         node.Name,
                                         s.To.Name,
                                         s.Label);
                    }
                    else
                    {
                        foreach (var suc in node.Successors)
                        {
                            WriteEdge(writer, node, suc, null);
                        }
                    }
                    if (node.Goto != null)
                    {
                        WriteEdge(writer, node, node.Goto, "red");
                    }
                }
                writer.WriteLine("}");
            }
        }

        private static void WriteEdge(TextWriter writer, Node from, Node to, string color)
        {
            string col = "";
            if (!string.IsNullOrEmpty(color))
                col = string.Format(" [color = {0}]", color);

            writer.WriteLine("{0} -> {1}{2};",
                             from.Name,
                             to.Name,
                             col);
        }

        private static void WriteNew(TextWriter writer, Node node, bool withName)
        {
            if (node.IsNew)
            {
                node.IsNew = false;
                if (withName)
                {
                    writer.WriteLine("{0} [style = filled, fillcolor = green];", node.Name);
                }
                else
                {
                    writer.WriteLine("style = filled;");
                    writer.WriteLine("fillcolor = green;");
                }
            }
        }

    	private static void WriteLabel(TextWriter writer, Node node)
        {
            string label = GetLabel(node);
            if (string.IsNullOrEmpty(label)) return;
            label = Escaper.EscapeUnquoted(label, true);
            //writer.WriteLine("{0} [label = \"{1}\", labeljust=left];", node.Name, label);
            writer.WriteLine("{0} [label = \"{1}\"];", node.Name, label);
        }

        private static void WriteNode(TextWriter writer, Node node)
        {
        	WriteNew(writer, node, true);
        	WriteLabel(writer, node);
        }

    	private static string GetLabel(Node node)
        {
            if (node == null) return null;
			var s = new StringBuilder();
			s.Append(node.Name);
			foreach (var instruction in node.Code)
			{
				if (s.Length > 0) s.Append("\n");
				s.Append(instruction.ToString());
			}
			return s.ToString();
        }
    }
}
#endif