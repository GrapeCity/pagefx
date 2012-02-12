using DataDynamics.PageFX.CLI.IL;

namespace DataDynamics.PageFX.CLI.CFG
{
    internal class TryNode : Node
    {
        public override NodeType NodeType
        {
            get { return NodeType.Try; }
        }

    	public ProtectedBlock Block { get; set; }

    	public NodeList Body
        {
            get { return _body; }
        }
        private readonly NodeList _body = new NodeList();

        public HandlerNodeList Handlers
        {
            get { return _handlers; }
        }
        private readonly HandlerNodeList _handlers = new HandlerNodeList();

        public void AddHandler(HandlerNode node)
        {
            node.Parent = this;
            _handlers.Add(node);
        }

        public override void DetachKids()
        {
            _body.Detach();
            foreach (var h in _handlers)
            {
                h.Detach();
            }
        }

        public override string GetSourceName(bool subgraph)
        {
            if (subgraph)
            {
                int n = _body.Count;
                if (n > 0)
                {
                    return _body[n - 1].GetSourceName(true);
                }
            }
            return base.GetSourceName(subgraph);
        }

        public override string GetTargetName(bool subgraph)
        {
            if (subgraph)
            {
                int n = _body.Count;
                if (n > 0)
                {
                    return _body[0].GetTargetName(true);
                }
            }
            return base.GetTargetName(subgraph);
        }
    }
}