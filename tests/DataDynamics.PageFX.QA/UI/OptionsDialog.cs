using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DataDynamics.UI
{
    public partial class OptionsDialog : Form
    {
        public OptionsDialog()
        {
            InitializeComponent();
        }

        public OptionsDialog(OptionNode root) : this()
        {
            Root = root;
        }

        [Browsable(false)]
        public OptionNode Root
        {
            get { return _root; }
            set 
            {
                if (value != _root)
                {
                    _root = value;
                    treeOpts.Nodes.Clear();
                    LoadKids(value, null);
                    //LoadOptionNode(value, null);
                    if (treeOpts.Nodes.Count > 0)
                        treeOpts.SelectedNode = treeOpts.Nodes[0];
                    treeOpts.ExpandAll();
                }
            }
        }
        private OptionNode _root;

        private void LoadKids(OptionNode opt, TreeNode node)
        {
            if (opt == null) return;
            foreach (var kid in opt.Kids)
                LoadOptionNode(kid, node);
        }

        private void LoadOptionNode(OptionNode opt, TreeNode parent)
        {
            if (opt == null) return;

        	var node = new TreeNode(opt.Name) {Tag = opt};

        	if (parent != null)
                parent.Nodes.Add(node);
            else
                treeOpts.Nodes.Add(node);

            LoadKids(opt, node);
        }

        private void treeOpts_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var opt = e.Node.Tag as OptionNode;
            if (opt != null && opt.Control != null)
            {
                opt.Control.Dock = DockStyle.Fill;
                splitMain.Panel2.Controls.Clear();
                splitMain.Panel2.Controls.Add(opt.Control);
            }
            else
            {
                splitMain.Panel2.Controls.Clear();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (_root != null)
                _root.Apply();
        }
    }

    public interface ISupportApply
    {
        void Apply();
    }

    public class OptionNode : ISupportApply
    {
        public OptionNode(string name, Control ctrl, params OptionNode[] kids)
        {
            Name = name;
            Control = ctrl;
            if (kids != null)
            {
                _kids.AddRange(kids);
            }
        }

    	public string Name { get; set; }

    	public Control Control { get; set; }

    	public List<OptionNode> Kids
        {
            get { return _kids; }
        }
        private readonly List<OptionNode> _kids = new List<OptionNode>();

        #region ISupportApply Members
        public void Apply()
        {
            var a = Control as ISupportApply;
            if (a != null)
                a.Apply();
            foreach (var kid in _kids)
                kid.Apply();
        }
        #endregion
    }
}