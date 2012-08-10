using System;
using System.Diagnostics;
using System.Security.Permissions;

namespace DataDynamics
{
    [SecurityPermission(SecurityAction.LinkDemand, Unrestricted = true)]
    public static class CommandPromt
    {
        public static string Run(string command, string args, out int exitCode)
        {
            return Run(command, args, out exitCode, false, true);
        }

        private const int WaitTimeout = 60000;

        public static string Run(string command, string args, out int exitCode, bool useCommandShell, bool redirect)
        {
            using (var process = new Process())
            {
                var si = process.StartInfo;

                if (useCommandShell)
                {
                    si.FileName = "cmd.exe";
                    string s = "/k " + command;
                    if (!string.IsNullOrEmpty(args))
                        s += " " + args;
                    si.Arguments = s;
                }
                else
                {
                    si.FileName = command;
                    si.Arguments = args;
                }

                si.UseShellExecute = false;
                si.CreateNoWindow = true;
                si.RedirectStandardOutput = redirect;
                si.RedirectStandardError = redirect;
                si.WorkingDirectory = Environment.CurrentDirectory;
                
                if (process.Start())
                {
                    if (redirect)
                    {
                        string err = "";
                        string stdout = ReadProcessOutput(process, ref err, out exitCode);
						return stdout + err;
                    }

                    if (!process.WaitForExit(WaitTimeout))
                    {
                        exitCode = -1;
                        process.Kill();
                        return "error: Timeout";
                    }

                    //At this point the process should surely have exited,
                    //since both the error and output streams have been fully read.
                    //To be paranoid, let's check anyway...
                    if (!process.HasExited)
                    {
                        exitCode = -1;
                        process.Kill();
                        return string.Format("Error: Process deadlock");
                    }
                }

                exitCode = -1;
                return "Error: Unable to start process";
            }
        }

        private delegate string StringDelegate();

        private static string ReadProcessOutput(Process process, ref string err, out int exitCode)
        {
            StringDelegate outputStreamAsyncReader = process.StandardOutput.ReadToEnd;
            StringDelegate errorStreamAsyncReader = process.StandardError.ReadToEnd;

            var outAR = outputStreamAsyncReader.BeginInvoke(null, null);
            var errAR = errorStreamAsyncReader.BeginInvoke(null, null);

            if (!process.WaitForExit(WaitTimeout))
            {
                err = "error: Timeout";
                exitCode = -1;
                process.Kill();
                return err;
            }

            //if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            //{
            //    //WaitHandle.WaitAll fails on single-threaded  apartments. Poll for completion instead:
            //    while (!(outAR.IsCompleted && errAR.IsCompleted))
            //    {
            //        //Check again every 10 milliseconds:
            //        Thread.Sleep(10);
            //    }
            //}
            //else
            //{
            //    WaitHandle[] arWaitHandles = new WaitHandle[2];
            //    arWaitHandles[0] = outAR.AsyncWaitHandle;
            //    arWaitHandles[1] = errAR.AsyncWaitHandle;
            //    if (!WaitHandle.WaitAll(arWaitHandles))
            //    {
            //        throw new Exception("Command line aborted");
            //    }
            //}

            string result = outputStreamAsyncReader.EndInvoke(outAR);
            err = errorStreamAsyncReader.EndInvoke(errAR);

            //At this point the process should surely have exited,
            //since both the error and output streams have been fully read.
            //To be paranoid, let's check anyway...
            if (!process.HasExited)
            {
                exitCode = -1;
                process.Kill();
                return string.Format("Error: Process deadlock");
            }

            exitCode = process.ExitCode;

            return result;
        }
    }
}