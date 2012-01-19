#if DEBUG
using System;
using System.IO;
using DataDynamics.PageFX.CLI;

namespace DataDynamics.PageFX
{
    partial class DebugInput
    {
        public static string SamplePath;

        public static CommandLine GetCommandLine()
        {
            if (string.IsNullOrEmpty(DebugSample))
                return null;

            try
            {
                const string rd1 = @"c:\dsamples";
                const string rd2 = @"e:\dsamples";

                string dir = Path.Combine(rd1, DebugSample);
                if (!Directory.Exists(dir))
                {
                    dir = Path.Combine(rd2, DebugSample);
                    if (!Directory.Exists(dir))
                        return null;
                }

                string path = Path.Combine(dir, "build.bat");
                if (!File.Exists(path))
                {
                    var files = Directory.GetFiles(dir, "*.bat");
                	if (files.Length <= 0) return null;
                    path = files[0];
                }

                SamplePath = path;

                var lines = File.ReadAllLines(path);
            	int n = lines.Length;
                if (n <= 0) return null;

                for (int i = 0; i < n; ++i)
                {
                    string line = lines[i];
					int index = line.IndexOf("pfc", StringComparison.OrdinalIgnoreCase);
                    if (index >= 0)
                    {
                        line = line.Substring(index + 3);
                        if (!string.IsNullOrEmpty(line) 
                            && line.StartsWith(".exe", StringComparison.OrdinalIgnoreCase))
                            line = line.Substring(4);
                        if (!string.IsNullOrEmpty(line))
                        {
                            var cl = CommandLine.Parse(line);
                            InitDebugHooks(cl);
                            return cl;
                        }
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        internal static void InitDebugHooks(CommandLine cl)
        {
            if (cl == null) return;

            Infrastructure.TestCaseDirectory = Path.Combine(Path.GetDirectoryName(SamplePath), "dump");

            if (cl.HasOption("dump-cil"))
                CLIDebug.DumpILCode = true;

            if (cl.HasOption("dump-ilmap"))
                CLIDebug.DumpILMap = true;
        }
    }
}
#endif