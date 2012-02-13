using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataDynamics.PageFX.CLI.CFG
{
    internal class SwitchNode : Node
    {
        public SwitchNode(Node condition)
        {
            _condition = condition;
            _condition.Parent = this;
        }

        public override NodeType NodeType
        {
            get { return NodeType.Switch; }
        }

        public override string GetSourceName(bool subgraph)
        {
            if (subgraph)
            {
                if (_condition != null)
                    return _condition.GetSourceName(true);
            }
            return Name;
        }

        public override string GetTargetName(bool subgraph)
        {
            if (subgraph)
            {
                if (_condition != null)
                    return _condition.GetTargetName(true);
            }
            return Name;
        }

        public Node Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }
        private Node _condition;

        public class Case
        {
            public int From;
            public int To;
            public Node Node;
            public bool Detach;
        }

        public List<Case> Cases
        {
            get { return _cases; }
        }
        private readonly List<Case> _cases = new List<Case>();

        public IEnumerable<Node> CaseNodes
        {
			get { return _cases.Cast<Node>(); }
        }

        public Case AddCase(Node caseNode, int from, int to, bool detach)
        {
            caseNode.Parent = this;
            var c = new Case();
            c.Node = caseNode;
            c.From = from;
            c.To = to;
            c.Detach = detach;
            _cases.Add(c);
            return c;
        }

        public override void DetachKids()
        {
            if (_condition != null)
                _condition.Detach();
            int n = _cases.Count;
            for (int i = 0; i < n; ++i)
            {
                var c = _cases[i];
                if (c.Detach)
                {
                    c.Node.DetachKids();
                    c.Node.Detach();
                }
            }
        }

        public override string ToString(bool full)
        {
            var sb = new StringBuilder();
            if (string.IsNullOrEmpty(Name))
                sb.AppendFormat("SW{0}", Index);
            else
                sb.Append(Name);
            if (full)
            {
                sb.Append("(");
                bool comma = false;
                if (_condition != null)
                {
                    sb.Append(_condition.ToString(false));
                    comma = true;
                }
                int n = _cases.Count;
                for (int i = 0; i < n; ++i)
                {
                    if (comma) sb.Append(", ");
                    sb.Append(_cases[i].Node.ToString(false));
                    comma = true;
                }
                sb.Append(")");
            }
            return sb.ToString();
        }
    }
}