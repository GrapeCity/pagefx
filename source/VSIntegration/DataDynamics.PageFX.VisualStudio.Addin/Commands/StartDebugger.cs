using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;

namespace DataDynamics.PageFX.VisualStudio.Commands
{
    [Command("StartDebugger", "Start Debugging"
        /*, Icon = "debug.png"*/)]
    internal class StartDebugger : IAddinCommand
    {
        public CommandAttribute Attribute { get; set; }

        public vsCommandStatus QueryStatus(VSAddin addin)
        {
            var m = addin.Debugger.DebugMode;
            var status = vsCommandStatus.vsCommandStatusSupported;
            if (!(m == DBGMODE.DBGMODE_Run || m == DBGMODE.DBGMODE_Break))
            {
                status |= vsCommandStatus.vsCommandStatusEnabled;
            }
            return status;
        }

        public Project GetStartupProject(DTE2 dte)
        {
            int n = dte.Solution.Projects.Count;
            if (n == 0) return null;

            var startup = dte.Solution.SolutionBuild.StartupProjects as object[];
            if (startup != null)
            {
                if (startup.Length == 1)
                {
                    try
                    {
                        return dte.Solution.Projects.Item(startup[0]);
                    }
                    catch
                    {
                    }
                }
            }

            if (n == 2)
                return dte.Solution.Projects.Item(1);

            return null;
        }

        public void Exec(VSAddin addin)
        {
            var dte = addin.DTE;
            
            var prj = GetStartupProject(dte);

            if (prj == null)
            {
                Utils.ErrorBox("No project to debug");
                return;
            }

            string path = Utils.GetProjectOutputPath(prj);
            addin.Debugger.Start(path);
        }
    }
}