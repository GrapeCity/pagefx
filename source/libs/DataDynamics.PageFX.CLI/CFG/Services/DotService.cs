#if DEBUG
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DataDynamics.PageFX.CodeModel;
using MethodBody=DataDynamics.PageFX.CLI.IL.MethodBody;

namespace DataDynamics.PageFX.CLI.CFG
{
    internal static class DirHelper
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

        public static string GetFullName(IMethod m)
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

        public static string GetDirectory(MethodBody body)
        {
        	string dir = string.IsNullOrEmpty(Infrastructure.TestCaseDirectory) ? "c:\\QA\\PageFX" : Infrastructure.TestCaseDirectory;
            dir = Path.Combine(dir, PathHelper.ReplaceInvalidPathChars(body.Method.DeclaringType.FullName));
            dir = Path.Combine(dir, PathHelper.ReplaceInvalidFileNameChars(GetFullName(body.Method)));
            Directory.CreateDirectory(dir);
            return dir;
        }
    }

    internal static class DotService
    {
        public static string GetDirectory(MethodBody body)
        {
            return DirHelper.GetDirectory(body);
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
            WriteDotFile(graph, path, subgraph);
            CLIDebug.LogInfo("WriteDotFile succeeded");

            CLIDebug.LogInfo("dot.exe started");
            int start = Environment.TickCount;
            Dot.Render(path, null);
            int end = Environment.TickCount;
            CLIDebug.LogInfo("dot.exe succeeded. ellapsed time: {0}", (end - start) + "ms");
        }

        private static void WriteDotFile(IEnumerable<Node> graph, string path, bool subgraph)
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
                    WriteNode(writer, node, subgraph);
                    if (node.IsTwoWay)
                    {
                        var f = node.FirstOut;
                        var s = f.NextOut;
                        writer.WriteLine("{0} -> {1} [label=\"{2}\"];",
                                         node.GetSourceName(subgraph),
                                         f.To.GetTargetName(subgraph),
                                         f.Label);
                        writer.WriteLine("{0} -> {1} [label=\"{2}\"];",
                                         node.GetSourceName(subgraph),
                                         s.To.GetTargetName(subgraph),
                                         s.Label);
                    }
                    else
                    {
                        foreach (var suc in node.Successors)
                        {
                            WriteEdge(writer, node, suc, subgraph, null);
                        }
                    }
                    if (node.Goto != null)
                    {
                        WriteEdge(writer, node, node.Goto, subgraph, "red");
                    }
                }
                writer.WriteLine("}");
            }
        }

        private static void WriteEdge(TextWriter writer, Node from, Node to, bool subgraph, string color)
        {
            string col = "";
            if (!string.IsNullOrEmpty(color))
                col = string.Format(" [color = {0}]", color);

            writer.WriteLine("{0} -> {1}{2};",
                             from.GetSourceName(subgraph),
                             to.GetTargetName(subgraph),
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

        private static void Write<T>(TextWriter writer, IEnumerable<T> nodes, bool subgraph)
            where T : Node
        {
            foreach (var node in nodes)
            {
                WriteNode(writer, node, subgraph);
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

        private static void WriteNode(TextWriter writer, Node node, bool subgraph)
        {
            var type = node.NodeType;
            if (type == NodeType.BasicBlock)
            {
                WriteNew(writer, node, true);
                WriteLabel(writer, node);
            }
            else if (type == NodeType.Sequence)
            {
                var seq = (SequenceNode)node;
                int n = seq.Kids.Count;
                if (subgraph)
                {
                    writer.WriteLine("subgraph cluster{0}", node.Name);
                    writer.WriteLine("{");

                    Write(writer, seq.Kids, true);

                    for (int i = 1; i < n; ++i)
                    {
                        writer.WriteLine("{0} -> {1} [color=red];",
                                         seq.Kids[i - 1].GetSourceName(true),
                                         seq.Kids[i].GetTargetName(true));
                    }
                    writer.WriteLine("label = \"{0}({1})\";", node.Name, n);
                    writer.WriteLine("color = blue;");
                    WriteNew(writer, node, false);
                    writer.WriteLine("}");
                }
                else
                {
                    WriteLabel(writer, node);
                    WriteNew(writer, node, true);
                }
            }
            else if (type == NodeType.If)
            {
                var ifNode = (IfNode)node;
                if (subgraph)
                {
                    writer.WriteLine("subgraph cluster{0}", node.Name);
                    writer.WriteLine("{");

                    WriteNode(writer, ifNode.Condition, true);
                    WriteNode(writer, ifNode.True, true);
                    if (ifNode.False != null)
                        WriteNode(writer, ifNode.False, true);

                    writer.WriteLine("{0} -> {1} [color=red, label=\"1\"];",
                                     ifNode.Condition.GetSourceName(true),
                                     ifNode.True.GetTargetName(true));

                    if (ifNode.False != null)
                    {
                        writer.WriteLine("{0} -> {1} [color=red, label=\"0\"];",
                                         ifNode.Condition.GetSourceName(true),
                                         ifNode.False.GetTargetName(true));
                    }

                    writer.WriteLine("label = \"IF\";");
                    writer.WriteLine("color = blue;");
                    WriteNew(writer, node, false);
                    writer.WriteLine("}");
                }
                else
                {
                    WriteLabel(writer, node);
                    WriteNew(writer, node, true);
                }
            }
            else if (type == NodeType.Loop)
            {
                var loop = (LoopNode)node;
                if (subgraph)
                {
                    writer.WriteLine("subgraph cluster{0}", node.Name);
                    writer.WriteLine("{");

                    if (loop.Condition != null)
                    {
                        WriteNode(writer, loop.Condition, true);

                        if (loop.IsPreTested)
                        {
                            string srcName = loop.Condition.GetSourceName(true);
                            if (loop.FirstChild != null)
                            {
                                writer.WriteLine("{0} -> {1} [color=red];", srcName,
                                                 loop.FirstChild.GetTargetName(true));
                                writer.WriteLine("{0} -> {1} [color=red];",
                                                 loop.LastChild.GetSourceName(true),
                                                 loop.Condition.GetTargetName(true));
                            }
                            foreach (var suc in node.Successors)
                            {
                                writer.WriteLine("{0} -> {1} [color=red];",
                                                 srcName,
                                                 suc.GetTargetName(true));
                            }
                        }
                        else if (loop.FirstChild != null)
                        {
                            writer.WriteLine("{0} -> {1} [color=red];",
                                             loop.LastChild.GetSourceName(true),
                                             loop.Condition.GetTargetName(true));

                            writer.WriteLine("{0} -> {1} [color=red];",
                                             loop.Condition.GetSourceName(true),
                                             loop.FirstChild.GetTargetName(true));
                        }
                    }

                    int n = loop.Body.Count;
                    for (int i = 0; i < n; ++i)
                    {
                        WriteNode(writer, loop.Body[i], true);
                    }
                    for (int i = 1; i < n; ++i)
                    {
                        writer.WriteLine("{0} -> {1} [color=red];",
                                         loop.Body[i - 1].GetSourceName(true),
                                         loop.Body[i].GetTargetName(true));
                    }

                    writer.WriteLine("label = \"{0}\";", node.Name);
                    writer.WriteLine("color = blue;");
                    WriteNew(writer, node, false);
                    writer.WriteLine("}");
                }
                else
                {
                    WriteLabel(writer, node);
                    WriteNew(writer, node, true);
                }
            }
            else if (type == NodeType.Switch)
            {
                var sw = (SwitchNode)node;
                if (subgraph)
                {
                    writer.WriteLine("subgraph cluster{0}", node.Name);
                    writer.WriteLine("{");

                    WriteNode(writer, sw.Condition, true);
                    Write(writer, sw.CaseNodes, true);
                    
                    foreach (var c in sw.CaseNodes)
                    {
                        writer.WriteLine("{0} -> {1} [color=red];",
                                         sw.GetSourceName(true),
                                         c.GetTargetName(true));

                        if (c.Goto != null)
                        {
                            writer.WriteLine("{0} -> {1} [color=red];",
                                         c.GetSourceName(true),
                                         c.Goto.GetTargetName(true));
                        }
                    }

                    writer.WriteLine("label = \"{0}\";", node.Name);
                    writer.WriteLine("color = blue;");
                    WriteNew(writer, node, false);
                    writer.WriteLine("}");
                }
                else
                {
                    WriteLabel(writer, node);
                    WriteNew(writer, node, true);
                }
            }
            else if (type == NodeType.Try)
            {
                var tryNode = (TryNode)node;
                string label = tryNode.Name;
                if (subgraph)
                {
                    writer.WriteLine("subgraph cluster{0}", node.Name);
                    writer.WriteLine("{");

                    Write(writer, tryNode.Body, true);
                    Write(writer, tryNode.Handlers, true);

                    writer.WriteLine("label = \"{0}\";", label);
                    writer.WriteLine("color = blue;");
                    WriteNew(writer, node, false);
                    writer.WriteLine("}");
                }
                else
                {
                    WriteLabel(writer, node);
                    WriteNew(writer, node, true);
                }
            }
            else if (type == NodeType.Handler)
            {
                var h = (HandlerNode)node;
                string label = h.Name;
                if (subgraph)
                {
                    writer.WriteLine("subgraph cluster{0}", node.Name);
                    writer.WriteLine("{");

                    Write(writer, h.Body, true);

                    writer.WriteLine("label = \"{0}\";", label);
                    writer.WriteLine("color = blue;");
                    WriteNew(writer, node, false);
                    writer.WriteLine("}");
                }
                else
                {
                    WriteLabel(writer, node);
                    WriteNew(writer, node, true);
                }
            }
        }

        private static string GetLabel(Node node)
        {
            if (node == null) return null;
            switch (node.NodeType)
            {
                case NodeType.BasicBlock:
                    {
                        var s = new StringBuilder();
                        s.Append(node.Name);
                        foreach (var instruction in node.Code)
                        {
                            if (s.Length > 0) s.Append("\n");
                            s.Append(instruction.ToString());
                        }
                        return s.ToString();
                    }

                default:
                    return node.ToString(true);
            }
        }
    }
}
#endif