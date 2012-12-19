using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace DataDynamics.PageFX.Common.Tools
{
    /// <summary>
    /// Provides running of FlashPlayer
    /// </summary>
    public class FlashPlayer
    {
        private const string Prefix = "<%";
		private const string Suffix = "%>";

        public const string MarkerEnd = Prefix + "END" + Suffix;
        public const string MarkerSuccess = Prefix + "SUCCESS" + Suffix;
        public const string MarkerFail = Prefix + "FAIL" + Suffix;

        public class Results
        {
            public bool Success { get; internal set; }

            public string Output { get; internal set; }
            
            public int Time { get; internal set; }

            public int ExitCode { get; internal set; }

            public bool NotRun { get; internal set; }

            public bool Timeout { get; internal set; }
        }

        public static string Path
        {
            get 
            {
                if (string.IsNullOrEmpty(_path)
                    || !File.Exists(_path))
                    return FindFP();
                return _path;
            }
            set 
            {
                _path = value;
            }
        }
        private static string _path;

        static string FindFP()
        {
            return PfxConfig.FlashPlayer.Path;
        }

        public static Results Run(string path)
        {
            BackupMMCfg();
            var results = new Results();
            RunCore(path, results);
            RestoreMMCfg();
            return results;
        }

        static void RunCore(string path, Results results)
        {
            using (var process = new Process())
            {
                var si = process.StartInfo;

                si.FileName = Path;
                si.Arguments = path;
                si.UseShellExecute = false;
                si.CreateNoWindow = true;

                int start = Environment.TickCount;
                if (process.Start())
                {
                    results.Output = Wait(results);
                    results.Time = Environment.TickCount - start;
                    try
                    {
                        process.Kill();
                    }
                    catch (Exception e)
                    {
                        results.ExitCode = -1;
                        results.Output = "Unexpected Error: " + e;
                    }
                    return;
                }
                results.NotRun = true;
                results.ExitCode = -1;
                results.Output = "Unable to start Flash Player";
            }
        }

        private static string flashlog; //path to flash log file
		private static StreamReader reader;
		private static bool read;

		private static void CreateReader()
        {
            if (reader != null) return;
            if (!File.Exists(flashlog)) return;
            var fs = File.Open(flashlog, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            reader = new StreamReader(fs);
        }

		private static string Wait(Results results)
        {
            Thread.Sleep(500);

            try
            {
                flashlog = GetFlashLogPath();
                string dir = System.IO.Path.GetDirectoryName(flashlog);
                Directory.CreateDirectory(dir);

                reader = null;
                CreateReader();

                using (var watcher = new FileSystemWatcher(dir, "*.txt"))
                {
                    watcher.NotifyFilter = NotifyFilters.FileName |
                                           NotifyFilters.LastAccess |
                                           NotifyFilters.LastWrite |
                                           NotifyFilters.Size;

                    watcher.Changed += watcher_Changed;
                    watcher.Created += watcher_Created;
                    watcher.Deleted += watcher_Deleted;
                    watcher.Renamed += watcher_Renamed;

                    //Begin watching
                    watcher.EnableRaisingEvents = true;

                    return Read(results);
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

		private static string Read(Results results)
        {
            read = true;
            bool timeout = false;
            var output = new StringBuilder();
            int start = Environment.TickCount;
            while (true)
            {
                if (Environment.TickCount - start > 60000)
                {
                    timeout = true;
                    break;
                }

                if (read && reader != null)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        read = false;
                        continue;
                    }

                    output.Append(line);
                    output.Append('\n');

                    if (line == MarkerSuccess)
                    {
                        results.Success = true;
                        continue;
                    }

                    if (line == MarkerFail)
                    {
                        results.Success = false;
                        continue;
                    }

                    if (line == MarkerEnd)
                        break;
                }
                else
                {
                    Thread.Sleep(100);
                }
            }

            if (reader != null)
            {
                reader.Close();
                reader = null;
            }

            if (timeout)
            {
                results.Timeout = true;
                return output.ToString();
            }

            return output.ToString();
        }

		private static void watcher_Created(object sender, FileSystemEventArgs e)
        {
            CreateReader();
        }

		private static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            read = true;
        }

		private static void watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            //This should not happen
        }

		private static void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            //This should not happen
        }

        #region MM.CFG
        static string old_mm_cfg;

        static readonly string[] mm_cfg = 
            {
                "ErrorReportingEnable=1",
                "TraceOutputFileEnable=1"
            };

        static void BackupMMCfg()
        {
            string path = GetMMCfgPath();
            if (File.Exists(path))
                old_mm_cfg = File.ReadAllText(path);

            File.WriteAllLines(path, mm_cfg);
        }

        static void RestoreMMCfg()
        {
            string path = GetMMCfgPath();
            if (old_mm_cfg != null)
                File.WriteAllText(path, old_mm_cfg);
        }
        #endregion

        #region Path Utils
        static string GetMMCfgPath()
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            dir = System.IO.Path.GetDirectoryName(dir);
            return System.IO.Path.Combine(dir, "mm.cfg");
        }

        static string GetFlashLogPath()
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            dir = System.IO.Path.Combine(dir, "Macromedia\\Flash Player\\Logs");
            return System.IO.Path.Combine(dir, "flashlog.txt");
        }
        #endregion
    }
}