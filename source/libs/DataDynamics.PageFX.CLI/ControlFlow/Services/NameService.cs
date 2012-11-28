#if DEBUG
using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.CLI.ControlFlow.Services
{
    internal sealed class NameService
    {
    	private int _index;

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
            foreach (var suc in node.Successors.Where(suc => !suc.IsVisited))
            {
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

			SetName(node, "B", ref _index);
        }
    }
}
#endif