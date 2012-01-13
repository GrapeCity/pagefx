namespace IssueVision
{
    class LeafTreeNode
    {
        public LeafTreeNode(string label)
        {
            this.label = label;
        }

        public LeafTreeNode(string label, object tag)
        {
            this.label = label;
            _tag = tag;
        }

        public Avm.String label;

        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        private object _tag;
    }
}