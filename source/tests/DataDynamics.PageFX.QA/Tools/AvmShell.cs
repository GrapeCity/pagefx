using System.IO;
using System.Text;
using DataDynamics.PageFX;

namespace DataDynamics
{
    public static class AvmShell
    {
        #region class Options
        public class Options
        {
            /// <summary>
            /// [-d] enter debugger on start
            /// </summary>
            public bool StartDebugger { get; set; }

            /// <summary>
            /// [-Ddprofile]  dynamic instruction stats
            /// </summary>
            public bool DynamicProfile { get; set; }

            /// <summary>
            /// [-Dsprofile]  show static instruction stats
            /// </summary>
            public bool StaticProfile { get; set; }

            /// <summary>
            /// [-Dnogc] don't collect
            /// </summary>
            public bool NoGC { get; set; }

            /// <summary>
            /// [-Dgcstats]   generate statistics on gc
            /// </summary>
            public bool GCStats { get; set; }

            /// <summary>
            /// [-Dnoincgc] don't use incremental collection
            /// </summary>
            public bool NoIncGC { get; set; }

            /// <summary>
            /// [-Dastrace N] display AS execution information, where N is [1..4]
            /// </summary>
            public int ASTrace { get; set; }

            public static bool InterpretDefaultValue;

            /// <summary>
            /// [-Dinterp] do not generate machine code, interpret instead
            /// </summary>
            public bool Interpret { get; set; }

            /// <summary>
            /// [-Dverbose] trace every instruction (verbose!)
            /// </summary>
            public bool Verbose { get; set; }

            /// <summary>
            /// [-Dbbgraph] output MIR basic block graphs for use with Graphviz
            /// </summary>
            public bool BBGraph { get; set; }

            /// <summary>
            /// [-Dforcemir] use MIR always, never interp
            /// </summary>
            public bool ForceMIR { get; set; }

            /// <summary>
            /// [-Dnodce] disable DCE optimization
            /// </summary>
            public bool NoDCE { get; set; }

            /// <summary>
            /// [-Dnocse] disable CSE optimization
            /// </summary>
            public bool NoCSE
            {
                get { return _nocse; }
                set { _nocse = value; }
            }
            private bool _nocse;

            /// <summary>
            /// [-Dnosse] use FPU stack instead of SSE2 instructions
            /// </summary>
            public bool NoSSE { get; set; }

            //Note: it seems that for now this option is not working in AMV shell
            /// <summary>
            /// [-Dverifyall] verify greedily instead of lazily
            /// </summary>
            public bool VerifyAll { get; set; }

            /// <summary>
            /// [-Dtimeout] enforce maximum 15 seconds execution
            /// </summary>
            public bool Timeout { get; set; }

            /// <summary>
            /// [-error] crash opens debug dialog, instead of dumping
            /// </summary>
            public bool Error { get; set; }

            /// <summary>
            /// [-log]
            /// </summary>
            public bool Log { get; set; }

            /// <summary>
            /// [-- args]     args passed to AS3 program
            /// </summary>
            public string ApplicationArguments { get; set; }

            /// <summary>
            /// [-jargs ... ;] args passed to Java runtime
            /// </summary>
            public string JavaRuntimeArguments { get; set; }

            public Options()
            {
                Interpret = InterpretDefaultValue;
                Timeout = true;
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                if (StartDebugger) sb.Append("-d ");
                if (DynamicProfile) sb.Append("-Ddprofile ");
                if (StaticProfile) sb.Append("-Dsprofile ");
                if (NoGC) sb.Append("-Dnogc ");
                if (GCStats) sb.Append("-Dgcstats ");
                if (NoIncGC) sb.Append("-Dnoincgc ");
                if (ASTrace > 0) sb.AppendFormat("-Dastrace {0} ", ASTrace);
                if (Interpret) sb.Append("-Dinterp ");
                if (Verbose) sb.Append("-Dverbose ");
                if (BBGraph) sb.Append("-Dbbgraph ");
                if (ForceMIR) sb.Append("-Dforcemir ");
                if (NoDCE) sb.Append("-Dnodce ");
                if (_nocse) sb.Append("-Dnocse ");
                if (NoSSE) sb.Append("-Dnosse ");
                if (VerifyAll) sb.Append("-Dverifyall ");
                if (Timeout) sb.Append("-Dtimeout ");
                if (Error) sb.Append("-error ");
                if (Log) sb.Append("-log ");

                if (!string.IsNullOrEmpty(JavaRuntimeArguments))
                {
                    sb.AppendFormat("-jargs {0}", JavaRuntimeArguments);
                }

                if (!string.IsNullOrEmpty(ApplicationArguments))
                {
                    sb.AppendFormat("-- {0}", ApplicationArguments);
                }

                return sb.ToString();
            }
        }
        #endregion

        public static string GetPath()
        {
            if (_path == null)
            {
                string dir = GlobalSettings.ToolsDirectory;
                _path = Path.Combine(dir, "avmplus.exe");
                Directory.CreateDirectory(dir);
                var rs = typeof(AvmShell).GetResourceStream("avmplus.exe");
                rs.Save(_path);
            }
            return _path;
        }

        private static string _path;

        public static string Run(Options options, out int exitCode, params string[] inputs)
        {
            string path = GetPath();
            if (!File.Exists(path))
            {
                exitCode = -1;
                return string.Format("Error: File {0} does not exist", path);
            }

            if (options == null)
                options = new Options();

            var args = new StringBuilder();
            args.Append(options.ToString());
            int n = inputs.Length;
            for (int i = 0; i < n; ++i)
            {
                if (i > 0) args.Append(" ");
                args.Append(inputs[i]);
            }

            return CommandPromt.Run(path, args.ToString(), out exitCode);
        }
    }
}