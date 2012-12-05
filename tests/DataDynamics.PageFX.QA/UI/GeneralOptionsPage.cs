using System.Windows.Forms;
using DataDynamics.UI;

namespace DataDynamics.PageFX.UI
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
            tbQaBaseDir.Text = QA.BaseDir;
            cbOptimizeCode.Checked = QA.OptimizeCode;
            cbDebug.Checked = QA.EmitDebugInfo;
        }

        #region ISupportApply Members
        public void Apply()
        {
            string path = tbQaBaseDir.Text;
            //TODO: Check path
            //if (Path.)
            QA.BaseDir = path;
            QA.OptimizeCode = cbOptimizeCode.Checked;
            QA.EmitDebugInfo = cbDebug.Checked;

            QA.SaveGlobalOptions();
        }
        #endregion
    }
}
