using System;
using dd.events;
using mx.controls;
using mx.events;

namespace IssueVision
{
    class RootController
    {
        private readonly Tree _tree;
        private readonly IssueList _issues;
        private readonly OneIssuePane _issueView;
        private readonly TreeViewDataProvider _dataProvider;

        public RootController()
        {
            App app = App.Instance;
            app.DataLoaded += OnDataLoaded;

            _tree = app.MainPane.PaneLeft.StaffList;
            _tree.showRoot = false;
            _tree.mouseFocusEnabled = true;
            _tree.focusEnabled = true;
            _tree.itemClick += OnNodeClick;

            _issues = app.MainPane.PaneMiddle.Issues;
            _issues.AfterSelect += OnIssueSelect;

            _issueView = app.MainPane.PaneIssue;

            _dataProvider = new TreeViewDataProvider();
            _tree.dataProvider = _dataProvider;
        }

        private void OnDataLoaded(object sender, EventArgs e)
        {
            _dataProvider.Update();
            _tree.invalidateDisplayList();
        }

        private void OnNodeClick(ListEvent e)
        {
            object item = _tree.selectedItem;
            if (item == null) return;

            if (item is TreeViewDataProvider.StaffersNode)
            {
                SelectIssues(-1);
                return;
            }

            LeafTreeNode node = item as LeafTreeNode;
            if (node != null)
            {
                IVDataSet.StaffersRow row = node.Tag as IVDataSet.StaffersRow;
                if (row != null)
                {
                    SelectIssues(row.StafferID);
                }
                return;
            }
        }

        private void SelectIssues(int stafferID)
        {
            _issues.Clear();
            IVDataSet data = App.Instance.Data.Issues;
            foreach (IVDataSet.IssuesRow row in data.Issues.Rows)
            {
                if (stafferID < 0 || row.StafferID == stafferID)
                {
                    IssueBox box = new IssueBox();
                    box.Issue = row;
                    box.percentWidth = 100;
                    box.Setup(row.Title, row.Description,
                              row.DateOpened.ToString(DefaultDateShortFormat), row.IsOpen);
                    _issues.addChild(box);
                }
            }
        }

        private void OnIssueSelect(SelectionEvent e)
        {
            IssueBox box = e.Selection as IssueBox;
            if (box != null)
            {
                UpdateIssuePane(box.Issue as IVDataSet.IssuesRow);
                return;
            }

            UpdateIssuePane(e.Selection as IVDataSet.IssuesRow);
        }

        private const string DefaultDateLongFormat = "dd.MM.yyyy hh:mm";
        private const string DefaultDateShortFormat = "dd.MM.yyyy";

        private static string GetOpenDate(IVDataSet.IssuesRow row)
        {
            return row.DateOpened.ToString(DefaultDateLongFormat);
        }

        private static string GetStafferName(int id)
        {
            IVDataSet data = App.Instance.Data.Lookup;
            if (data != null)
            {
                foreach (IVDataSet.StaffersRow row in data.Staffers.Rows)
                {
                    if (row.StafferID == id)
                        return row.UserName;
                }
            }
            return "Unknown";
        }

        private void UpdateIssuePane(IVDataSet.IssuesRow row)
        {
            if (row == null) return;

            int issueID = row.IssueID;
            _issueView.IssueTitle = row.Title;
            _issueView.IssueDescription = row.Description;
            _issueView.IssueID = issueID;
            _issueView.IssueOpened = GetOpenDate(row);
            _issueView.IssueCategory = row.IssueType;
            _issueView.AssignedTo = row.UserName;
            _issueView.IssueStatus = row.IsOpen ? "Opened" : "Closed";

            HistoryList history = _issueView.History;
            history.Clear();

            IVDataSet data = App.Instance.Data.Issues;
            foreach (IVDataSet.IssueHistoryRow hr in data.IssueHistory.Rows)
            {
                if (hr.IssueID == issueID)
                {
                    string user = GetStafferName(hr.StafferID);
                    string date = hr.DateCreated.ToString(DefaultDateLongFormat);
                    history.AddItem(user, hr.Comment, date);
                }
            }
        }
    }
}