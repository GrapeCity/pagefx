using System;
using System.IO;
using System.Runtime.InteropServices;
using DataDynamics.PageFX.VisualStudio.Debugger;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace DataDynamics.PageFX
{
    /// <summary>
    /// Facade to VS Debugger
    /// </summary>
    internal class VSDebugger
    {
        public VSDebugger(DTE2 dte)
        {
            _dte = dte;
        }
        readonly DTE2 _dte;

        IVsDebugger GetVsDebugger()
        {
            var sp = new ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)_dte);
            return (IVsDebugger)sp.GetService(typeof(SVsShellDebugger));
        }

        public DBGMODE DebugMode
        {
            get
            {
                var debugger = GetVsDebugger();
                var mode = new DBGMODE[1];
                debugger.GetMode(mode);
                return mode[0];
            }
        }

        public void Start(string path)
        {
            string dir = Path.GetDirectoryName(path);
            string fname = Path.GetFileNameWithoutExtension(path);
            string swfPath = Path.Combine(dir, fname + ".swf");
            string htmlPath1 = Path.Combine(dir, fname + ".html");
            string htmlPath2 = Path.Combine(dir, fname + ".htm");
            //Prefer html to swf
            string realPath = Utils.SelectPath(htmlPath1, htmlPath2, swfPath);
            if (realPath == null)
            {
                Utils.ErrorBox("No swf/html file to debug");
                return;
            }

            var debugger = GetVsDebugger();

            var info = new VsDebugTargetInfo();
            info.cbSize = (uint)Marshal.SizeOf(info);
            info.dlo = DEBUG_LAUNCH_OPERATION.DLO_CreateProcess;

            info.bstrExe = realPath;
            info.bstrCurDir = Path.GetDirectoryName(info.bstrExe);
            info.bstrArg = null; // no command line parameters
            info.bstrRemoteMachine = null; // debug locally
            info.fSendStdoutToOutputWindow = 0; // Let stdout stay with the application.
            info.clsidCustom = Const.DebugEngineId; // Set the launching engine the sample engine guid
            info.grfLaunch = 0;

            IntPtr pInfo = Marshal.AllocCoTaskMem((int)info.cbSize);
            Marshal.StructureToPtr(info, pInfo, false);

            try
            {
                int hr = debugger.LaunchDebugTargets(1, pInfo);
                if (hr != Const.S_OK)
                {
                    Utils.ErrorBox("Error: {0:X8}", hr);
                }
            }
            catch (Exception exc)
            {
                Utils.ErrorBox("Unable to start debugger. Details:\n", exc.ToString());
            }
            finally
            {
                if (pInfo != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pInfo);
                }
            }
        }
    }
}