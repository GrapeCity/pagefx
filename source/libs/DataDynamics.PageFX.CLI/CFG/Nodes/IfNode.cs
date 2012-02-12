using System.Text;

namespace DataDynamics.PageFX.CLI.CFG
{
    internal class IfNode : Node
    {
        public IfNode(Node condition, Node t, Node f)
        {
            _condition = condition;
            _true = t;
            _false = f;
            _condition.Parent = this;
            _true.Parent = this;
            if (_false != null)
                _false.Parent = this;
        }

        public IfNode(Node condition, Node t) 
            : this(condition, t, null)
        {
        }

        /// <summary>
        /// Flag specifying whether to negate branch operator during decompiling.
        /// </summary>
        public bool Negate = true;

        public bool And = true;
        
        public bool DetachTrue = true;
        
        public override NodeType NodeType
        {
            get { return NodeType.If; }
        }

        public bool IsTernary
        {
            get
            {
                return _condition != null && _true != null && _false != null;
            }
        }

        public override string GetSourceName(bool subgraph)
        {
            if (subgraph)
            {
                if (_condition != null)
                    return _condition.GetTargetName(true);
                if (_true != null)
                    return _true.GetTargetName(true);
            }
            return Name;
        }

        public override string GetTargetName(bool subgraph)
        {
            if (subgraph)
            {
                if (_condition != null)
                    return _condition.GetSourceName(true);
            }
            return Name;
        }

        public Node Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }
        private Node _condition;

        public Node True
        {
            get { return _true; }
        }
        private readonly Node _true;

        public Node False
        {
            get { return _false; }
        }
        private readonly Node _false;

        public override void DetachKids()
        {
            if (_condition != null)
            {
                _condition.DetachKids();
                _condition.Detach();
            }
            if (DetachTrue && _true != null)
            {
                _true.DetachKids();
                _true.Detach();
            }
            if (_false != null)
            {
                _false.DetachKids();
                _false.Detach();
            }
        }

        public override string ToString(bool full)
        {
            var sb = new StringBuilder();
        	sb.Append(string.IsNullOrEmpty(Name) ? "IF" : Name);
        	if (full)
            {
                sb.Append("(");
                bool comma = false;
                if (_condition != null)
                {
                    sb.AppendFormat("COND = {0}", _condition.ToString(false));
                    comma = true;
                }
                if (_true != null)
                {
                    if (comma) sb.Append(", ");
                    sb.AppendFormat("TRUE = {0}", _true.ToString(false));
                    comma = true;
                }
                if (_false != null)
                {
                    if (comma) sb.Append(", ");
                    sb.AppendFormat("FALSE = {0}", _false.ToString(false));
                }
                sb.Append(")");
            }
            return sb.ToString();
        }
    }
}