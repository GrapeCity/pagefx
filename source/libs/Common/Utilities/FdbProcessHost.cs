using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;

namespace DataDynamics.PageFX.Common.Utilities
{
	[SecurityPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public static class FdbProcessHost
	{
		private static string GetFdbExe()
		{
			string flexdir = GlobalSettings.GetVar("FlexSdkHome");
			if (!string.IsNullOrEmpty(flexdir))
				return Path.Combine(flexdir, "bin\\fdb.exe");
			return "fdb.exe";
		}

		private const int waitTimeForProcess = 30000;

		public static string Run(string args, string[] input, string workdir, out int exitCode)
		{
			string fileName = GetFdbExe();

			using (var process = new Process())
			{
				var si = process.StartInfo;

				{
					si.FileName = fileName;
					si.Arguments = args;
				}

				si.UseShellExecute = false;
				si.RedirectStandardInput = true;
				si.RedirectStandardOutput = true;
				si.RedirectStandardError = false;
				si.WorkingDirectory = workdir;
				si.CreateNoWindow = false;

				if (process.Start())
				{
					WriteToProccessInput(process, input);

					if (!process.WaitForExit(waitTimeForProcess))
					{
						exitCode = -1;
						process.Kill();
						return string.Format("Error: Timeout");
					}

					StreamReader sr = process.StandardOutput;
					for (int skipline = 0; skipline < 8; skipline++)
						sr.ReadLine();
					string retVal = sr.ReadToEnd();
					exitCode = 0;

					//At this point the process should surely have exited,
					//since both the error and output streams have been fully read.
					//To be paranoid, let's check anyway...
					if (!process.HasExited)
					{
						exitCode = -1;
						process.Kill();
						return string.Format("Error: Process deadlock");
					}

					return retVal;
				}

				exitCode = -1;
				return "Error: Unable to start process";
			}
		}

		private static void WriteToProccessInput(Process process, IEnumerable<string> input)
		{
			foreach (string cmd in input)
			{
				process.StandardInput.WriteLine(cmd);
			}
		}
	}
}