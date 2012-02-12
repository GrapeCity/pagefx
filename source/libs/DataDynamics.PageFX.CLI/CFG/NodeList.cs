using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataDynamics.PageFX.CLI.CFG
{
    internal class NodeList : List<Node>
    {
        #region Public Members
        public void Add(IEnumerable<Node> list)
        {
            foreach (var node in list)
            {
                Add(node);
            }
        }

        public void Remove(params Node[] nodes)
        {
            for (int i = 0; i < Count; ++i)
            {
            	var p = this[i];
            	if (nodes.Contains(p))
            	{
            		RemoveAt(i);
            	}
            }
        }
        #endregion

        #region Object Override Methods
        public override string ToString()
        {
            var s = new StringBuilder();
            int n = Count;
            for (int i = 0; i < n; ++i)
            {
                if (i > 0) s.Append(", ");
                s.Append(this[i].Index);
            }
            return s.ToString();
        }
        #endregion

        public void Detach()
        {
            foreach (var node in this)
            {
                node.Detach();
            }
        }
    }
}