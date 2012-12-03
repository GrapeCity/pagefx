using System;
using System.IO;
using System.Windows.Forms;
using DataDynamics.PageFX.FlashLand.Abc;

namespace abc
{
    public partial class DumpForm : Form
    {
        public DumpForm()
        {
            InitializeComponent();
        }

        private void btnBrowseInputFile_Click(object sender, EventArgs e)
        {
            Utils.BrowseFile(tbInputFile, "Files|*.abc;*.swf;*.swc");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cbExcludeStandardLibs.Checked)
                AbcDumpService.ClassFilter = Program.LoadStandardExcludes();
            Program.Dump(tbInputFile.Text, null);
            MessageBox.Show("Done!!!");
        }

        private void DumpForm_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;
            Utils.LoadState(this);
        }

        private void DumpForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Idle -= Application_Idle;
            Utils.SaveState(this);
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            string path = tbInputFile.Text;
            btnStart.Enabled = !string.IsNullOrEmpty(path) && File.Exists(path);
        }
    }
}