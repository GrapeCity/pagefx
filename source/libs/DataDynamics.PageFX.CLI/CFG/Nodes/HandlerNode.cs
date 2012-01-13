using System.Collections.Generic;
using DataDynamics.PageFX.CLI.IL;

namespace DataDynamics.PageFX.CLI.CFG
{
    internal class HandlerNode : Node
    {
        public override NodeType NodeType
        {
            get { return NodeType.Handler; }
        }

        public HandlerBlock Block
        {
            get { return _block; }
            set { _block = value; }
        }
        private HandlerBlock _block;

        public NodeList Body
        {
            get { return _body; }
        }
        private readonly NodeList _body = new NodeList();

        public override void DetachKids()
        {
            _body.Detach();
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

    internal class HandlerNodeList : List<HandlerNode>
    {
    }
}