using System.IO;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.TestRunner.Tools
{
    public static class FlashShell
    {
        private static string GetToolPath()
        {
            string dir = GlobalSettings.BinDirectory;
            return Path.Combine(dir, "flashell.exe");
        }

        public static string Run(string path, out int exitCode)
        {
            string tool = GetToolPath();
            if (!File.Exists(tool))
            {
                exitCode = -1;
                return string.Format("Error: flashell.exe tool does not exist.");
            }

            string args = "";
            args += path;

            return CommandPromt.Run(tool, args, out exitCode);
        }
    }
}