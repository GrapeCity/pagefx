using System.Windows.Forms;
using DataDynamics.PageFX.TestRunner.Framework;

namespace DataDynamics.PageFX.TestRunner.UI
{
    public partial class GeneralOptionsPage : UserControl, ISupportApply
    {
        public GeneralOptionsPage()
        {
            InitializeComponent();
            LoadState();
        }

        private void LoadState()
        {
            if (DesignMode) return;
            tbQaBaseDir.Text = GlobalOptions.BaseDir;
            cbOptimizeCode.Checked = GlobalOptions.OptimizeCode;
            cbDebug.Checked = GlobalOptions.EmitDebugInfo;
        }

        #region ISupportApply Members
        public void Apply()
        {
            string path = tbQaBaseDir.Text;
            //TODO: Check path
            //if (Path.)
            GlobalOptions.BaseDir = path;
            GlobalOptions.OptimizeCode = cbOptimizeCode.Checked;
            GlobalOptions.EmitDebugInfo = cbDebug.Checked;

            GlobalOptions.Save();
        }
        #endregion
    }
}
