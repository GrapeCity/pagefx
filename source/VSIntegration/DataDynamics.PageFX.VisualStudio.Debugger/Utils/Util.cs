using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualStudio.Debugger.Interop;
using System.Diagnostics;
using System.Globalization;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    static class Util
    {
        public static SourceLocation GetSourceLocation(BP_REQUEST_INFO info)
        {
            var docPosition = (IDebugDocumentPosition2)(Marshal.GetObjectForIUnknown(info.bpLocation.unionmember2));
            string documentName;
            CheckOk(docPosition.GetFileName(out documentName));

            // Get the location in the document that the breakpoint is in.
            var startPosition = new TEXT_POSITION[1];
            var endPosition = new TEXT_POSITION[1];
            CheckOk(docPosition.GetRange(startPosition, endPosition));

            return new SourceLocation
                       {
                           File = documentName,
                           Line = (int)startPosition[0].dwLine + 1,
                           Column = (int)startPosition[0].dwColumn
                       };
        }

        public static BP_REQUEST_INFO GetRequestInfo(IDebugBreakpointRequest2 r)
        {
            var info = new BP_REQUEST_INFO[1];
            CheckOk(r.GetRequestInfo((uint)enum_BPREQI_FIELDS.BPREQI_ALLFIELDS, info));
            return info[0];
        }

        public static string QName(string prefix, string name)
        {
            if (string.IsNullOrEmpty(prefix))
                return name;
            if (string.IsNullOrEmpty(name))
                return prefix;
            return prefix + "." + name;
        }

        public static int IndexOf<T>(IEnumerable<T> set, Predicate<T> p)
        {
            if (set != null)
            {
                int i = 0;
                foreach (var item in set)
                {
                    if (p(item))
                        return i;
                    ++i;
                }
            }
            return -1;
        }

        public static bool Contains<T>(IEnumerable<T> set, Predicate<T> p)
        {
            return IndexOf(set, p) >= 0;
        }

        public static T Find<T>(IEnumerable<T> set, Predicate<T> p)
        {
            if (set != null)
            {
                foreach (var item in set)
                {
                    if (p(item))
                        return item;
                }
            }
            return default(T);
        }

        public delegate bool WaitEvent();

        public static int Wait(WaitEvent w)
        {
            return Wait(30000, w);
        }

        public static int Wait(int timeout, WaitEvent w)
        {
            int start = Environment.TickCount;
            while (true)
            {
                if (w()) return 0;
                Thread.Sleep(100);
                if (timeout > 0 && (Environment.TickCount - start) > timeout)
                    return Const.TIMEOUT;
                //Application.DoEvents();
            }
        }

        public static string GetFdbPath()
        {
            string home = GetEnv("FlexSdkHome");
            if (home != null && Directory.Exists(home))
            {
                return Path.Combine(home, "bin\\fdb.exe");
            }
            return null;
        }

        public static string GetEnv(string name)
        {
            var targets = new[] { EnvironmentVariableTarget.Process, EnvironmentVariableTarget.User, EnvironmentVariableTarget.Machine};
            foreach (var target in targets)
            {
                var v = Environment.GetEnvironmentVariable(name, target);
                if (!string.IsNullOrEmpty(v))
                    return v;
            }
            return null;
        }

        public static void CheckOk(int hr)
        {
            if (hr != 0)
            {
                throw new InvalidOperationException();
            }
        }

        public static void RequireOk(int hr)
        {
            if (hr != 0)
            {
                throw new InvalidOperationException();
            }
        }

        public static int GetProcessId(IDebugProcess2 process)
        {
            var pid = new AD_PROCESS_ID[1];
            RequireOk(process.GetPhysicalProcessId(pid));
            if (pid[0].ProcessIdType != (uint)enum_AD_PROCESS_ID.AD_PROCESS_ID_SYSTEM)
                return 0;
            return (int) pid[0].dwProcessId;            
        }

        public static int GetProcessId(IDebugProgram2 program)
        {
            IDebugProcess2 process;
            RequireOk(program.GetProcess(out process));

            return GetProcessId(process);
        }

        public static int UnexpectedException(Exception e)
        {
            Debug.Fail("Unexpected exception during Attach");
            return Const.RPC_E_SERVERFAULT;
        }

        internal static bool IsFlagSet(uint value, int flagValue)
        {
            return (value & flagValue) != 0;
        }

        public static string GetAddressDescription(string module, uint ip)
        {
            string location = ip.ToString("x8", CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(module))
            {
                location = string.Concat(module, "!", location);
            }
            return location;
        }

        public static void CloseWindow(string ProcessName, bool CheckStop)
        {
            DialogResult res;
            Process[] allProcesses = Process.GetProcessesByName(ProcessName);
            foreach (Process oneProcess in allProcesses)
            {
                if (!CheckStop) res = DialogResult.OK;
                else res = MessageBox.Show(string.Format("Close {0} ?", ProcessName), "", MessageBoxButtons.OKCancel);
                if (res == DialogResult.OK)
                {
                    if (oneProcess.Responding) oneProcess.CloseMainWindow();
                    else oneProcess.Kill();
                }
            }
        }
    }
}
