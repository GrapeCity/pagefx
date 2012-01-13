using System.Collections.Generic;
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
            int n = nodes.Length;
            for (int i = 0; n > 0 && i < Count; ++i)
            {
                var p = this[i];
                for (int j = 0; j < nodes.Length; ++j)
                {
                    if (p == nodes[j])
                    {
                        RemoveAt(i);
                        --n;
                        break;
                    }
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