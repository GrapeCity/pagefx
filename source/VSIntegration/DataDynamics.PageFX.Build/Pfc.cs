using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace DataDynamics.PageFX.Build
{
    public class Pfc : Task
    {
        #region Options
        /// <summary>
        /// Gets or sets assembly path to compile.
        /// </summary>
        [Required]
        public string AssemblyPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Debug { get; set; }

        /// <summary>
        /// Specifies whether to add reference to MX library.
        /// </summary>
        public bool MX { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FrameSize { get; set; }

        //TODO: Extend options
        #endregion

        #region Execute
        public override bool Execute()
        {
            string pfc = GlobalSettings.PfcPath;
            if (!File.Exists(pfc))
            {
                Log.LogError("Unable to find pfc.exe");
                return false;
            }

            using (var process = new Process())
            {
                string cl = GetCommandLine();

                //Log.LogMessage(MessageImportance.High, cl);

                process.StartInfo = new ProcessStartInfo
                                        {
                                            FileName = pfc,
                                            Arguments = cl,
                                            UseShellExecute = false,
                                            CreateNoWindow = true,
                                            RedirectStandardOutput = true,
                                            RedirectStandardError = true
                                        };
                process.Start();
                process.WaitForExit();

                string stdout = process.StandardOutput.ReadToEnd();

                var errors = CompilerConsole.ParseOutput(stdout);
                if (errors.HasErrors)
                {
                    Log.LogError(stdout);
                    return false;
                }

                if (errors.HasWarnings)
                {
                    Log.LogWarning(stdout);
                }

                return true;
            }
        }
        #endregion

        #region GetCommandLine
        private string GetCommandLine()
        {
            var sb = new StringBuilder();
            sb.Append("/nologo ");
            if (Debug != null)
                sb.Append(PFCOptions.Debug.ToString(Debug) + " ");
            if (MX)
                sb.Append(PFCOptions.MX + " ");
            if (!string.IsNullOrEmpty(FrameSize))
                sb.Append(PFCOptions.FrameSize.ToString(FrameSize) + " ");
            sb.Append(AssemblyPath);
            return sb.ToString();
        }
        #endregion
    }
}
