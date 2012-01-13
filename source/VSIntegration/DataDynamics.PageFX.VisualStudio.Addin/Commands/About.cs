using DataDynamics.PageFX.UI;
using EnvDTE;

namespace DataDynamics.PageFX.VisualStudio.Addin.Commands
{
    [Command("About", "About DataDynamics PageFX", Position = 3)]
    class AboutCommand : IAddinCommand
    {
        public CommandAttribute Attribute
        {
            get;
            set;
        }

        public vsCommandStatus QueryStatus(VSAddin addin)
        {
            return vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
        }

        public void Exec(VSAddin addin)
        {
            using (var dlg = new AboutForm())
                dlg.ShowDialog();
        }
    }
}
