using System;
using System.Windows.Forms;
using DataDynamics.PageFX.Common.CompilerServices;
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
            tbHTMLWDir.Text = Config.Html.Template;
            cbGenHTMLWrapper.Checked = (Config.Html.Template != "") ? true : false;
            
            if (Config.Compiler.ExceptionBreak)
                cbBreakException.Checked = true;

            nuFPVersion.Text = Config.Runtime.FlashVersion.ToString();
            tbLocale.Text = Config.Flex.Locale;
        }

	    private static PfxConfig Config
	    {
			get { return PfxConfig.Default; }
	    }

	    public void Apply()
        {
            Config.Html.Template             = cbGenHTMLWrapper.Checked ? tbHTMLWDir.Text : "";
            Config.Compiler.ExceptionBreak   = cbBreakException.Checked ? true : false;
            Config.Runtime.FlashVersion = Convert.ToInt32(nuFPVersion.Text);
            Config.Flex.Locale = tbLocale.Text;
            Config.Save();
        }
    }
}
