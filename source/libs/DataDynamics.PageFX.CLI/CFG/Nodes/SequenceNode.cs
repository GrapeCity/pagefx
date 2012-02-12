using System.Collections.Generic;
using System.Text;

namespace DataDynamics.PageFX.CLI.CFG
{
    internal class SequenceNode : Node
    {
        public SequenceNode()
        {
        }

        public SequenceNode(params Node[] kids)
        {
            Add(kids);
        }

        public override NodeType NodeType
        {
            get { return NodeType.Sequence; }
        }

        public NodeList Kids
        {
            get { return _kids; }
        }
        private readonly NodeList _kids = new NodeList();

        public override string GetSourceName(bool subgraph)
        {
            if (subgraph)
            {
                int n = Kids.Count;
                if (n > 0)
                    return Kids[n - 1].GetSourceName(true);
            }
            return Name;
        }

        public override string GetTargetName(bool subgraph)
        {
            if (subgraph)
            {
                if (Kids.Count > 0)
                    return Kids[0].GetSourceName(true);
            }
            return Name;
        }

        public string GetKidLabel(int i)
        {
            int n = Kids.Count;
            if (n > 3)
            {
                if (i == 0) return "1";
                if (i == 1) return "...";
                return n.ToString();
            }
            return (i + 1).ToString();
        }

        public Node Begin
        {
            get
            {
                if (_kids.Count > 0)
                    return _kids[0];
                return null;
            }
        }

        public Node End
        {
            get
            {
                int i = _kids.Count - 1;
                if (i >= 0)
                    return _kids[i];
                return null;
            }
        }

        public void Add(Node kid)
        {
            kid.Parent = this;
            _kids.Add(kid);
        }

        public void Add(IEnumerable<Node> kids)
        {
            foreach (var kid in kids)
            {
                Add(kid);
            }
        }

        public override void DetachKids()
        {
            _kids.Detach();
        }

        public override string ToString(bool full)
        {
            var sb = new StringBuilder();
        	sb.Append(string.IsNullOrEmpty(Name) ? "SEQ" : Name);
        	if (full)
            {
                sb.Append("(");
                int n = _kids.Count;
                for (int i = 0; i < n; ++i)
                {
                    if (i > 0) sb.Append(", ");
                    sb.Append(_kids[i].ToString(false));
                }
                sb.Append(")");
            }
            return sb.ToString();
        }
    }
}