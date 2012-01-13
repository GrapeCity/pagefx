using System;
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
            tbHTMLWDir.Text = PfxConfig.HTML.Template;
            cbGenHTMLWrapper.Checked = (PfxConfig.HTML.Template != "") ? true : false;
            
            if (PfxConfig.Compiler.ExceptionBreak)
                cbBreakException.Checked = true;

            nuFPVersion.Text = PfxConfig.Runtime.FlashVersion.ToString();
            tbLocale.Text = PfxConfig.Flex.Locale;
        }

        #region ISupportApply Members
        public void Apply()
        {
            PfxConfig.HTML.Template             = cbGenHTMLWrapper.Checked ? tbHTMLWDir.Text : "";
            PfxConfig.Compiler.ExceptionBreak   = cbBreakException.Checked ? true : false;
            PfxConfig.Runtime.FlashVersion = Convert.ToInt32(nuFPVersion.Text);
            PfxConfig.Flex.Locale = tbLocale.Text;
            PfxConfig.Save();
        }

        #endregion
    }
}
