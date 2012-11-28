using System;
using System.Text;

namespace DataDynamics.PageFX.CLI.ControlFlow.Services
{
    [Flags]
    internal enum FormatOptions
    {
        None,
        CodeSpan = 1,
        Predecessors = 2,
        Successors = 4,
        Default = None,
        //Default = Predecessors | Successors,
    }

    internal static class FormatService
    {
        public static string FormatLabel(Node node)
        {
            if (!string.IsNullOrEmpty(node.Name))
                return node.Name;
            return string.Format("{0}", node.Index);
        }

        public static string ToString(Node node, FormatOptions options)
        {
            var s = new StringBuilder();
            if ((options & FormatOptions.CodeSpan) != 0)
            {
                if (node.CodeLength > 0)
                {
                    s.AppendFormat("[{0}, {1}] ", node.EntryIndex, node.ExitIndex);
                }
            }

            if ((options & FormatOptions.Predecessors) != 0)
            {
                if (node.FirstIn != null)
                {
                    bool sep = false;
                    foreach (var p in node.Predecessors)
                    {
                        if (sep) s.Append(", ");
                        s.Append(FormatLabel(p));
                        sep = true;
                    }
                    s.Append(" -> ");
                }
            }

            s.Append(FormatLabel(node));

            if ((options & FormatOptions.Successors) != 0)
            {
                if (node.FirstOut != null)
                {
                    s.Append(" -> ");
                    bool sep = false;
                    foreach (var suc in node.Successors)
                    {
                        if (sep) s.Append(", ");
                        s.Append(FormatLabel(suc));
                        sep = true;
                    }
                }
            }
            return s.ToString();
        }
    }
}