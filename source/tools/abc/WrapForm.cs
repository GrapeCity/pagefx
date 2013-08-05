using System;
using System.IO;
using System.Windows.Forms;
using DataDynamics.PageFX.Flash.Core.Tools;

namespace DataDynamics.PageFX
{
    public partial class WrapForm : Form
    {
        public WrapForm()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            Utils.BrowseFile(tbInputPath, "Files|*.abc;*.swc");
        }

        private void WrapForm_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;
            Utils.LoadState(this);
        }

        private void WrapForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Idle -= Application_Idle;
            Utils.SaveState(this);
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            string path = tbInputPath.Text;
            btnGenerate.Enabled = !string.IsNullOrEmpty(path) && File.Exists(path);
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            Utils.SaveState(this);

            string curdir = Environment.CurrentDirectory;

            string path = tbInputPath.Text;
            string cls = "";
            
            string dir = tbDir.Text;
            if (!string.IsNullOrEmpty(dir))
            {
                cls += " /dir:" + dir;
                cls += " ";
            }

            if (!string.IsNullOrEmpty(tbOptions.Text))
            {
                cls += tbOptions.Text;
                cls += " ";
            }

            string outpath = tbOut.Text;
            if (string.IsNullOrEmpty(outpath))
                outpath = Path.GetFileNameWithoutExtension(path) + ".dll";

            try
            {
                dir = Path.GetDirectoryName(path);
                Environment.CurrentDirectory = dir;
                WrapperGenerator.Wrap(Path.GetFileName(path), outpath, cls);
            }
            finally
            {
                Environment.CurrentDirectory = curdir;
            }
        }
    }
}