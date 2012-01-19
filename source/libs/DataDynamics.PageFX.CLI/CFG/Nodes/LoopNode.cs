using System.Collections.Generic;
using System.Text;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.CFG
{
    internal class LoopNode : Node
    {
        public LoopNode()
        {
        }

        public LoopNode(LoopType type, Node condition)
        {
            _loopType = type;
            _condition = condition;
            _condition.Parent = this;
        }

        public override string GetSourceName(bool subgraph)
        {
            if (subgraph)
            {
                if (_condition != null && IsPostTested)
                    return _condition.GetTargetName(true);
                int n = _body.Count;
                if (n > 0)
                    return _body[n - 1].GetSourceName(true);
            }
            return Name;
        }

        public override string GetTargetName(bool subgraph)
        {
            if (subgraph)
            {
                if (_condition != null && IsPreTested)
                    return _condition.GetSourceName(true);
                int n = _body.Count;
                if (n > 0)
                    return _body[0].GetSourceName(true);
            }
            return Name;
        }

        public override NodeType NodeType
        {
            get { return NodeType.Loop; }
        }

        public LoopType LoopType
        {
            get { return _loopType; }
            set { _loopType = value; }
        }
        private LoopType _loopType;

        public bool IsPreTested
        {
            get { return _loopType == LoopType.PreTested; }
        }

        public bool IsPostTested
        {
            get { return _loopType == LoopType.PostTested; }
        }

        public Node Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }
        private Node _condition;

        public NodeList Body
        {
            get { return _body; }
        }
        private readonly NodeList _body = new NodeList();

        public Node FirstChild
        {
            get
            {
                if (_body.Count > 0)
                    return _body[0];
                return null;
            }
        }

        public Node LastChild
        {
            get
            {
                int n = _body.Count;
                if (n > 0)
                    return _body[n - 1];
                return null;
            }
        }

        public void Add(Node kid)
        {
            _body.Add(kid);
            kid.Parent = this;
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
            if (_condition != null)
            {
                _condition.DetachKids();
                _condition.Detach();
            }
            foreach (var kid in _body)
            {
                kid.DetachKids();
            }
            _body.Detach();
        }

        public override string ToString(bool full)
        {
            var sb = new StringBuilder();
        	sb.Append(!string.IsNullOrEmpty(Name) ? Name : LoopType.ToString());
        	if (full)
            {
                sb.Append("(");
                bool comma = false;
                if (_condition != null)
                {
                    sb.Append(_condition.ToString(false));
                    comma = true;
                }
                foreach (var kid in _body)
                {
                    if (comma) sb.Append(", ");
                    sb.Append(kid.ToString(false));
                    //sb.Append(kid.Name);
                    comma = true;
                }
                sb.Append(")");
            }
            return sb.ToString();
        }
    }
}