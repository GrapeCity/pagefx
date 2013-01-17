using System;
using System.IO;
using System.Text;
using DataDynamics.PageFX.Common.CompilerServices;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.TestRunner.Tools
{
	internal static class JsRunner
	{
		public sealed class Options
		{
			public override string ToString()
			{
				return "";
			}
		}

		public static string Run(string input, Options options, out int exitCode)
		{
			var exepath = GetPath();
			if (string.IsNullOrEmpty(exepath) || !File.Exists(exepath))
			{
				exitCode = -1;
				return String.Format("Error: File {0} does not exist", exepath);
			}

			if (options == null)
				options = new Options();

			var args = new StringBuilder();
			args.Append(options);
			if (args.Length > 0) args.Append(" ");
			args.Append(input);

			return CommandPromt.Run(exepath, args.ToString(), out exitCode);
		}

		private static string GetPath()
		{
			return Path.Combine(GlobalSettings.GetVar("NODEJS_HOME"), "node.exe");
		}
	}
}
