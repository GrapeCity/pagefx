namespace IssueVision
{
    class TreeViewDataProvider
    {
        public TreeViewDataProvider()
        {
            children.push(_staffers);
            children.push(_conflicts);
        }

        public StaffersNode Staffers
        {
            get { return _staffers; }
        }
        private readonly StaffersNode _staffers = new StaffersNode();

        public ConflictsNode Conflicts
        {
            get { return _conflicts; }
        }
        private readonly ConflictsNode _conflicts = new ConflictsNode();

        public readonly Avm.Array children = new Avm.Array();

        public void Update()
        {
            _staffers.Update();
            _conflicts.Update();
        }

        public class StaffersNode
        {
            public Avm.String label = "All Staffers";
            public Avm.Array children = new Avm.Array();

            public void Update()
            {
                IVDataSet data = App.Instance.Data.Lookup;
                //Alert.show(data.Staffers.Rows.Count.ToString());

                Avm.Array arr = new Avm.Array();

                foreach (IVDataSet.StaffersRow row in data.Staffers.Rows)
                {
                    arr.push(new LeafTreeNode(row.DisplayName, row));
                }

                children = arr;
            }
        }

        public class ConflictsNode
        {
            public Avm.String label = "Conflicts";
            public Avm.Array children;

            public void Update()
            {
            }
        }
    }
}