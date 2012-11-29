using System.Collections.Generic;
using System.Text;

namespace DataDynamics.PageFX.CLI.Translation.ControlFlow
{
    internal sealed class NodeList : List<Node>
    {
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
    }
}