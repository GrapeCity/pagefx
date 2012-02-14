#if DEBUG
using System.Collections.Generic;

namespace DataDynamics.PageFX.CLI.CFG
{
    internal class NameService
    {
        private char letter = 'a';
        private int index;
        private int seqIndex;
        private int graphIndex;
        private int whileIndex;
        private int doWhileIndex;
        private int loopIndex;
        private int ifIndex;
        private int swIndex;
        private int tryIndex;
        private int handlerIndex;

        public void SetNames(IEnumerable<Node> list) 
        {
            foreach (var node in list)
                node.IsVisited = false;

            foreach (var node in list)
                SetNames(node);
        }

        private void SetNames(Node node)
        {
            if (node.IsVisited) return;
            node.IsVisited = true;
            SetName(node);
            foreach (var suc in node.Successors)
            {
                if (!suc.IsVisited)
                    SetNames(suc);
            }
        }

        private static void SetName(Node node, string prefix, ref int i)
        {
            node.Name = prefix + (i++);
        }

        public void SetName(Node node)
        {
            if (!string.IsNullOrEmpty(node.Name))
                return;

			SetName(node, "B", ref index);
        }
    }
}
#endif