using System;
using System.Collections.Generic;
using System.Text;
using DataDynamics.PageFX.UI;
using DataDynamics.UI;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;

namespace DataDynamics.PageFX.VisualStudio.Addin.Commands
{
    [Command("ShowOptions", "Options...", Position = 2)]
    internal class ShowOptions : IAddinCommand
    {
        public CommandAttribute Attribute
        {
            get; set; 
        }

        public vsCommandStatus QueryStatus(VSAddin addin)
        {
            return vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
        }

        public void Exec(VSAddin addin)
        {
            using (var dlg = new OptionsDialog())
            {
                dlg.Root = new OptionNode("Options", null,
                                          new OptionNode("General", new GeneralOptionsPage()));
                dlg.ShowDialog();
            }
        }
    }
}
