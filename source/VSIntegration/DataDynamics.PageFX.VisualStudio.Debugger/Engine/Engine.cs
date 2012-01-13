using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Debugger.Interop;
using System.Diagnostics;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    // AD7Engine is the primary entrypoint object for the sample engine. 
    //
    // It implements:
    //
    // IDebugEngine2: This interface represents a debug engine (DE). It is used to manage various aspects of a debugging session, 
    // from creating breakpoints to setting and clearing exceptions.
    //
    // IDebugEngineLaunch2: Used by a debug engine (DE) to launch and terminate programs.
    //
    // IDebugProgram3: This interface represents a program that is running in a process.
    // Since this engine only debugs one process at a time and each 
    // process only contains one program, it is implemented on the engine.
    //
    
    [ComVisible(true)]
    [Guid(Const.DebugEngineCLSID)]
    public class Engine : IDebugEngine2, IDebugEngineLaunch2, IDebugProgram3
    {
        // This object manages breakpoints in the sample engine.
        BreakpointManager m_breakpointManager;

        // A unique identifier for the program being debugged.
        Guid m_ad7ProgramId;
        IDebugEventCallback2 m_sdm;
        int m_processId;
        DebugThread m_thread;
        IDebugger m_debugger;
        string m_apppath;

        public Engine()
        {
        }

        internal IDebugger Debugger
        {
            get { return m_debugger; }
        }

        internal DebugThread Thread
        {
            get { return m_thread; }
        }

        internal BreakpointManager BreakpointManager
        {
            get { return m_breakpointManager; }
        }

        internal PendingBreakpoint FindBreakpoint(string id)
        {
            foreach (var bp in m_breakpointManager)
            {
                if (bp.ID == id)
                    return bp;
            }
            return null;
        }

        internal void CompleteBreakpoint(PendingBreakpoint bp)
        {
            Send(new BreakpointEvent(bp.BoundBreakpoint));
        }

        internal string SwfPath
        {
            get { return m_apppath; }
        }

        internal string ModuleName
        {
            get 
            {
                if (m_modname == null)
                    m_modname = Path.GetFileNameWithoutExtension(m_apppath);
                return m_modname;
            }
        }
        string m_modname;

        void OpenSession()
        {
            m_breakpointManager = new BreakpointManager(this);
            m_debugger = new FdbHost(this);
            m_thread = new DebugThread(this, null);
        }

        void CloseSession()
        {
            try
            {
                if (m_debugger != null)
                {
                    m_debugger.Stop();
                    m_debugger = null;
                }
            }
            catch (Exception exc)
            {
            }

            m_thread = null;
            m_breakpointManager = null;
            m_processId = 0;
        }

        #region IDebugEngine2 Members

        // Attach the debug engine to a program. 
        int IDebugEngine2.Attach(IDebugProgram2[] rgpPrograms, IDebugProgramNode2[] rgpProgramNodes, uint celtPrograms, IDebugEventCallback2 ad7Callback, uint dwReason)
        {
            if (celtPrograms != 1)
            {
                Debug.Fail("SampleEngine only expects to see one program in a process");
                throw new ArgumentException();
            }

            try
            {
                int processId = Util.GetProcessId(rgpPrograms[0]);
                if (processId == 0)
                    return Const.E_NOTIMPL; // sample engine only supports system processes

                Util.RequireOk(rgpPrograms[0].GetProgramId(out m_ad7ProgramId));

                Send(new EngineCreateEvent(this));
                Send(new ProgramCreateEvent());
                Send(new ThreadCreateEvent());
                Send(new LoadCompleteEvent());
                
                return Const.S_OK;
            }
            catch (Exception e)
            {
                return Util.UnexpectedException(e);
            }
        }

        // Requests that all programs being debugged by this DE stop execution the next time one of their threads attempts to run.
        // This is normally called in response to the user clicking on the pause button in the debugger.
        // When the break is complete, an AsyncBreakComplete event will be sent back to the debugger.
        int IDebugEngine2.CauseBreak()
        {
            return ((IDebugProgram2)this).CauseBreak();
        }

        // Called by the SDM to indicate that a synchronous debug event, previously sent by the DE to the SDM,
        // was received and processed. The only event the engine sends in this fashion is Program Destroy.
        // It responds to that event by shutting down the engine.
        int IDebugEngine2.ContinueFromSynchronousEvent(IDebugEvent2 eventObject)
        {
            try
            {
                if (eventObject is ProgramDestroyEvent)
                {
                    CloseSession();
                }
                else
                {
                    Debug.Fail("Unknown syncronious event");
                }
            }
            catch (Exception e)
            {
                return Util.UnexpectedException(e);
            }

            return Const.S_OK;
        }

        // Creates a pending breakpoint in the engine. A pending breakpoint is contains all the information needed to bind a breakpoint to 
        // a location in the debuggee.
        int IDebugEngine2.CreatePendingBreakpoint(IDebugBreakpointRequest2 pBPRequest, out IDebugPendingBreakpoint2 ppPendingBP)
        {
            Debug.Assert(m_breakpointManager != null);
            ppPendingBP = null;

            try
            {
                var pb = m_breakpointManager.CreatePendingBreakpoint(pBPRequest);
                ppPendingBP = pb;
            }
            catch (Exception e)
            {
                return Util.UnexpectedException(e);
            }

            return Const.S_OK;
        }

        // Informs a DE that the program specified has been atypically terminated and that the DE should 
        // clean up all references to the program and send a program destroy event.
        int IDebugEngine2.DestroyProgram(IDebugProgram2 pProgram)
        {
            // Tell the SDM that the engine knows that the program is exiting, and that the
            // engine will send a program destroy. We do this because the Win32 debug api will always
            // tell us that the process exited, and otherwise we have a race condition.

            return (HRESULT.E_PROGRAM_DESTROY_PENDING);
        }

        // Gets the GUID of the DE.
        int IDebugEngine2.GetEngineId(out Guid guidEngine)
        {
            guidEngine = Const.DebugEngineId;
            return Const.S_OK;
        }

        // Removes the list of exceptions the IDE has set for a particular run-time architecture or language.
        // The sample engine does not support exceptions in the debuggee so this method is not actually implemented.
        int IDebugEngine2.RemoveAllSetExceptions(ref Guid guidType)
        {
            return Const.S_OK;
        }

        // Removes the specified exception so it is no longer handled by the debug engine.
        // The sample engine does not support exceptions in the debuggee so this method is not actually implemented.       
        int IDebugEngine2.RemoveSetException(EXCEPTION_INFO[] pException)
        {
            // The sample engine will always stop on all exceptions.
            return Const.S_OK;
        }

        // Specifies how the DE should handle a given exception.
        // The sample engine does not support exceptions in the debuggee so this method is not actually implemented.
        int IDebugEngine2.SetException(EXCEPTION_INFO[] pException)
        {           
            return Const.S_OK;
        }

        // Sets the locale of the DE.
        // This method is called by the session debug manager (SDM) to propagate the locale settings of the IDE so that
        // strings returned by the DE are properly localized. The sample engine is not localized so this is not implemented.
        int IDebugEngine2.SetLocale(ushort wLangID)
        {
            return Const.S_OK;
        }

        // A metric is a registry value used to change a debug engine's behavior or to advertise supported functionality. 
        // This method can forward the call to the appropriate form of the Debugging SDK Helpers function, SetMetric.
        int IDebugEngine2.SetMetric(string pszMetric, object varValue)
        {
            // The sample engine does not need to understand any metric settings.
            return Const.S_OK;
        }

        // Sets the registry root currently in use by the DE. Different installations of Visual Studio can change where their registry information is stored
        // This allows the debugger to tell the engine where that location is.
        int IDebugEngine2.SetRegistryRoot(string pszRegistryRoot)
        {
            // The sample engine does not read settings from the registry.
            return Const.S_OK;
        }

        #endregion

        #region IDebugEngineLaunch2 Members
        // Determines if a process can be terminated.
        int IDebugEngineLaunch2.CanTerminateProcess(IDebugProcess2 process)
        {
            try
            {
                int processId = Util.GetProcessId(process);
                return processId == m_processId ? Const.S_OK : Const.S_FALSE;
            }
            catch (Exception e)
            {
                return Util.UnexpectedException(e);
            }
        }

        // Launches a process by means of the debug engine.
        // Normally, Visual Studio launches a program using the IDebugPortEx2::LaunchSuspended method and then attaches the debugger 
        // to the suspended program. However, there are circumstances in which the debug engine may need to launch a program 
        // (for example, if the debug engine is part of an interpreter and the program being debugged is an interpreted language), 
        // in which case Visual Studio uses the IDebugEngineLaunch2::LaunchSuspended method
        // The IDebugEngineLaunch2::ResumeProcess method is called to start the process after the process has been successfully launched in a suspended state.
        int IDebugEngineLaunch2.LaunchSuspended(string pszServer, IDebugPort2 port,
            string exe, string args, string dir, string env, string options,
            uint launchFlags, uint hStdInput, uint hStdOutput, uint hStdError,
            IDebugEventCallback2 ad7Callback, out IDebugProcess2 process)
        {
            process = null;

            if (string.IsNullOrEmpty(exe))
                return Const.E_FAIL;

            if (!File.Exists(exe))
                return Const.E_FAIL;

            //TODO: Is it really good check?
            if (!Util.Contains(AppExtensions,
                ext => exe.EndsWith(ext,
                    StringComparison.InvariantCultureIgnoreCase)))
            {
                return Const.E_FAIL;
            }

            exe = Path.GetFullPath(exe);

            m_apppath = exe;
            m_sdm = ad7Callback;

            OpenSession();

            try
            {
                m_processId = m_debugger.Start(exe);

                var adProcessId = new AD_PROCESS_ID
                                      {
                                          ProcessIdType = ((uint)enum_AD_PROCESS_ID.AD_PROCESS_ID_SYSTEM),
                                          dwProcessId = ((uint)m_processId)
                                      };

                Util.RequireOk(port.GetProcess(adProcessId, out process));

                return Const.S_OK;
            }
            catch (Exception e)
            {
                return Util.UnexpectedException(e);
            }
        }

        readonly static string[] AppExtensions = { ".html", ".htm", ".swf" };

        // Resume a process launched by IDebugEngineLaunch2.LaunchSuspended
        int IDebugEngineLaunch2.ResumeProcess(IDebugProcess2 process)
        {
            try
            {
                int processId = Util.GetProcessId(process);

                if (processId != m_processId)
                {
                    return Const.S_FALSE;
                }

                // Send a program node to the SDM. This will cause the SDM to turn around and call IDebugEngine2.Attach
                // which will complete the hookup with AD7
                IDebugPort2 port;
                Util.RequireOk(process.GetPort(out port));
                
                var defaultPort = (IDebugDefaultPort2)port;
                
                IDebugPortNotify2 portNotify;
                Util.RequireOk(defaultPort.GetPortNotify(out portNotify));

                Util.RequireOk(portNotify.AddProgramNode(new ProgramNode(m_processId)));

                if (m_ad7ProgramId == Guid.Empty)
                {
                    Debug.Fail("Unexpected problem -- IDebugEngine2.Attach wasn't called");
                    return Const.E_FAIL;
                }

                return Const.S_OK;
            }
            catch (Exception e)
            {
                return Util.UnexpectedException(e);
            }
        }

        // This function is used to terminate a process that the SampleEngine launched
        // The debugger will call IDebugEngineLaunch2::CanTerminateProcess before calling this method.
        int IDebugEngineLaunch2.TerminateProcess(IDebugProcess2 process)
        {
            try
            {
                int processId = Util.GetProcessId(process);
                if (processId != m_processId)
                    return Const.S_FALSE;

                CloseSession();

                return Const.S_OK;
            }
            catch (Exception e)
            {
                return Util.UnexpectedException(e);
            }
        }

        #endregion

        #region IDebugProgram2 Members
        // Determines if a debug engine (DE) can detach from the program.
        public int CanDetach()
        {
            // The sample engine always supports detach
            return Const.S_OK;
        }

        // The debugger calls CauseBreak when the user clicks on the pause button in VS. The debugger should respond by entering
        // breakmode. 
        public int CauseBreak()
        {
            m_debugger.Pause();
            return Const.S_OK;
        }

        // Continue is called from the SDM when it wants execution to continue in the debugee
        // but have stepping state remain. An example is when a tracepoint is executed, 
        // and the debugger does not want to actually enter break mode.
        public int Continue(IDebugThread2 pThread)
        {
            m_debugger.Continue();
            return Const.S_OK;
        }

        // Detach is called when debugging is stopped and the process was attached to (as opposed to launched)
        // or when one of the Detach commands are executed in the UI.
        public int Detach()
        {
            CloseSession();
            return Const.S_OK;
        }

        // Enumerates the code contexts for a given position in a source file.
        public int EnumCodeContexts(IDebugDocumentPosition2 pDocPos, out IEnumDebugCodeContexts2 ppEnum)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        // EnumCodePaths is used for the step-into specific feature -- right click on the current statment and decide which
        // function to step into. This is not something that the SampleEngine supports.
        public int EnumCodePaths(string hint, IDebugCodeContext2 start, IDebugStackFrame2 frame, int fSource, out IEnumCodePaths2 pathEnum, out IDebugCodeContext2 safetyContext)
        {
            pathEnum = null;
            safetyContext = null;
            return Const.E_NOTIMPL;
        }

        // EnumModules is called by the debugger when it needs to enumerate the modules in the program.
        public int EnumModules(out IEnumDebugModules2 ppEnum)
        {
            ppEnum = new ModuleEnum(new Module[0]);
            return Const.S_OK;
        }

        // EnumThreads is called by the debugger when it needs to enumerate the threads in the program.
        public int EnumThreads(out IEnumDebugThreads2 ppEnum)
        {
            ppEnum = new ThreadEnum(new[] {m_thread});
            return Const.S_OK;
        }

        // The properties returned by this method are specific to the program. If the program needs to return more than one property, 
        // then the IDebugProperty2 object returned by this method is a container of additional properties and calling the 
        // IDebugProperty2::EnumChildren method returns a list of all properties.
        // A program may expose any number and type of additional properties that can be described through the IDebugProperty2 interface. 
        // An IDE might display the additional program properties through a generic property browser user interface.
        // The sample engine does not support this
        public int GetDebugProperty(out IDebugProperty2 ppProperty)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        // The debugger calls this when it needs to obtain the IDebugDisassemblyStream2 for a particular code-context.
        // The sample engine does not support dissassembly so it returns E_NOTIMPL
        public int GetDisassemblyStream(uint dwScope, IDebugCodeContext2 codeContext, out IDebugDisassemblyStream2 disassemblyStream)
        {
            disassemblyStream = null;
            return Const.E_NOTIMPL;
        }

        // This method gets the Edit and Continue (ENC) update for this program. A custom debug engine always returns E_NOTIMPL
        public int GetENCUpdate(out object update)
        {
            // The sample engine does not participate in managed edit & continue.
            update = null;
            return Const.S_OK;            
        }

        // Gets the name and identifier of the debug engine (DE) running this program.
        public int GetEngineInfo(out string engineName, out Guid engineGuid)
        {
            engineName = Const.DebugEngineName;
            engineGuid = Const.DebugEngineId;
            return Const.S_OK;
        }

        // The memory bytes as represented by the IDebugMemoryBytes2 object is for the program's image in memory and not any memory 
        // that was allocated when the program was executed.
        public int GetMemoryBytes(out IDebugMemoryBytes2 ppMemoryBytes)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        // Gets the name of the program.
        // The name returned by this method is always a friendly, user-displayable name that describes the program.
        public int GetName(out string programName)
        {
            // The Sample engine uses default transport and doesn't need to customize the name of the program,
            // so return NULL.
            programName = null;
            return Const.S_OK;
        }

        // Gets a GUID for this program. A debug engine (DE) must return the program identifier originally passed to the IDebugProgramNodeAttach2::OnAttach
        // or IDebugEngine2::Attach methods. This allows identification of the program across debugger components.
        public int GetProgramId(out Guid guidProgramId)
        {
            guidProgramId = m_ad7ProgramId;
            return Const.S_OK;
        }

        // This method is deprecated. Use the IDebugProcess3::Step method instead.
        public int Step(IDebugThread2 pThread, uint sk, uint Step)
        {
            return m_debugger.Step((enum_STEPKIND)sk, (enum_STEPUNIT)Step);
        }

        // Terminates the program.
        public int Terminate()
        {
            // Because the sample engine is a native debugger, it implements IDebugEngineLaunch2, and will terminate
            // the process in IDebugEngineLaunch2.TerminateProcess
            return Const.S_OK;
        }

        // Writes a dump to a file.
        public int WriteDump(uint DUMPTYPE, string pszDumpUrl)
        {
            // The sample debugger does not support creating or reading mini-dumps.
            return Const.E_NOTIMPL;
        }
        #endregion

        #region IDebugProgram3 Members
        // ExecuteOnThread is called when the SDM wants execution to continue and have 
        // stepping state cleared.
        public int ExecuteOnThread(IDebugThread2 pThread)
        {
            m_debugger.Continue();
            return Const.S_OK;
        }
        #endregion

        #region Deprecated interface methods
        // These methods are not called by the Visual Studio debugger, so they don't need to be implemented

        int IDebugEngine2.EnumPrograms(out IEnumDebugPrograms2 programs)
        {
            Debug.Fail("This function is not called by the debugger");

            programs = null;
            return Const.E_NOTIMPL;
        }

        public int Attach(IDebugEventCallback2 pCallback)
        {
            Debug.Fail("This function is not called by the debugger");

            return Const.E_NOTIMPL;
        }

        public int GetProcess(out IDebugProcess2 process)
        {
            Debug.Fail("This function is not called by the debugger");

            process = null;
            return Const.E_NOTIMPL;
        }

        public int Execute()
        {
            Debug.Fail("This function is not called by the debugger.");
            return Const.E_NOTIMPL;
        }

        #endregion

        #region Communication with SDM
        internal void Send(IDebugEvent eventObject)
        {
            uint attributes;
            var riidEvent = eventObject.IID;
            Util.RequireOk(eventObject.GetAttributes(out attributes));
            Util.RequireOk(m_sdm.Event(this, null, this, m_thread, eventObject, ref riidEvent, attributes));
        }

        internal static IDebugEvent CreateEvent(CompleteEvent e)
        {
            switch (e)
            {
                case CompleteEvent.Step:
                    return new StepCompleteEvent();
                case CompleteEvent.Break:
                    return new BreakCompleteEvent();
            }
            return null;
        }

        internal void SendCompleteEvent(CompleteEvent e)
        {
            Send(CreateEvent(e));
        }

        internal void StopComplete()
        {
            //Send(new StopCompleteEvent());
            m_ad7ProgramId = Guid.Empty;
            //Send(new ThreadDestroyEvent(0));
            Send(new ProgramDestroyEvent(0));
        }
        #endregion

        public void Trace(string msg)
        {
            Send(new OutputDebugStringEvent(ModuleName + ": " + msg + "\n"));
        }

        internal void EvalChildProperties(Property prop)
        {
            m_debugger.EvalChildProperties(prop);
        }

        internal Property EvalExpression(StackFrame frame, string code)
        {
            return m_debugger.EvalExpression(frame, code);
        }

        internal bool RemoveBreakPoint(PendingBreakpoint bp)
        {
            if (m_debugger.RemoveBreakpoint(bp))
            {
                m_breakpointManager.Remove(bp);
                if (bp.BoundBreakpoint != null)
                    Send(new BreakpointUnboundEvent(bp.BoundBreakpoint));
                return true;
            }
            return false;
        }

        internal bool EnableBreakpoint(PendingBreakpoint bp, bool value)
        {
            return m_debugger.EnableBreakpoint(bp, value);
        }
    }

    internal enum CompleteEvent
    {
        Step,
        Break
    }
}
