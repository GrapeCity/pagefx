using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.CLI.CFG
{
    internal class FlowGraphHelper
    {
        //   C
        //  / \
        // T   F
        //  \ /
        //   V
        public static bool IsDiamond(Node C, Node F, out Node T)
        {
            T = null;
            if (C == null) return false;
            if (F == null) return false;

            if (!F.HasOneOut) return false;

            var e1 = C.FirstOut;
            if (e1 == null) return false;

            var e2 = e1.NextOut;
            if (e2 == null) return false;

            if (e2.NextOut != null) return false;

            T = e1.To == F ? e2.To : e1.To;
            if (!T.HasOneOut) return false;

            return F.FirstSuccessor == T.FirstSuccessor;
        }

        public static bool IsBranchOfTernaryExpression(Node F, out Node T, out Node C)
        {
            T = null;
            C = null;

            if (F == null) return false;
            if (!F.HasOneOut) return false;

            foreach (var CE in F.InEdges)
            {
                C = CE.From;
                if (IsDiamond(C, F, out T))
                    return true;
            }

            C = null;
            T = null;

            return false;
        }

        public static Node GetCommonSuccessor(Node a, Node b)
        {
            Node result = null;
            foreach (var suc in a.Successors)
            {
                if (b.Successors.Contains(suc))
                {
                    if (result != null)
                        return null;
                    result = suc;
                }
            }
            return result;
        }

        //Entry points to a SCC are those nodes whose predecessor does not belong to the SCC
        public static NodeList GetEntryPoints(IEnumerable<Node> nodes)
        {
            var hash = new Hashtable();
            foreach (var node in nodes)
                hash[node] = node;

            var entryPoints = new NodeList();
        	entryPoints.AddRange(nodes.Where(node => node.Predecessors.Any(p => hash[p] == null)));
        	return entryPoints;
        }
    }
}