using System;
using Microsoft.VisualStudio.Debugger.Interop;

// This file contains the various event objects that are sent to the debugger from the engine via IDebugEventCallback2::Event.
// The events are how the engine tells the debugger about what is happening in the debuggee process. 
// Most events sent the debugger are asynchronous events.

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    interface IDebugEvent : IDebugEvent2
    {
        Guid IID { get; }
    }

    #region Event base classes
    class AsyncEvent : IDebugEvent2
    {
        public const uint Attributes = (uint)enum_EVENTATTRIBUTES.EVENT_ASYNCHRONOUS;

        int IDebugEvent2.GetAttributes(out uint eventAttributes)
        {
            eventAttributes = Attributes;
            return Const.S_OK;
        }
    }

    class StoppingEvent : IDebugEvent2
    {
        public const uint Attributes = (uint)enum_EVENTATTRIBUTES.EVENT_ASYNC_STOP;

        int IDebugEvent2.GetAttributes(out uint eventAttributes)
        {
            eventAttributes = Attributes;
            return Const.S_OK;
        }
    }

    class SyncEvent : IDebugEvent2
    {
        public const uint Attributes = (uint)enum_EVENTATTRIBUTES.EVENT_SYNCHRONOUS;

        int IDebugEvent2.GetAttributes(out uint eventAttributes)
        {
            eventAttributes = Attributes;
            return Const.S_OK;
        }
    }

    class SyncStoppingEvent : IDebugEvent2
    {
        public const uint Attributes = (uint)enum_EVENTATTRIBUTES.EVENT_STOPPING | (uint)enum_EVENTATTRIBUTES.EVENT_SYNCHRONOUS;

        int IDebugEvent2.GetAttributes(out uint eventAttributes)
        {
            eventAttributes = Attributes;
            return Const.S_OK;
        }
    }
    #endregion

    #region class EngineCreateEvent
    // The debug engine (DE) sends this interface to the session debug manager (SDM) when an instance of the DE is created.
    sealed class EngineCreateEvent : AsyncEvent, IDebugEngineCreateEvent2, IDebugEvent
    {
        static readonly Guid _IID = new Guid("FE5B734C-759D-4E59-AB04-F103343BDD06");

        readonly IDebugEngine2 m_engine;

        public EngineCreateEvent(IDebugEngine2 engine)
        {
            m_engine = engine;
        }

        int IDebugEngineCreateEvent2.GetEngine(out IDebugEngine2 engine)
        {
            engine = m_engine;
            return Const.S_OK;
        }

        public Guid IID
        {
            get { return _IID; }
        }
    }
    #endregion

    #region class ProgramCreateEvent
    // This interface is sent by the debug engine (DE) to the session debug manager (SDM) when a program is attached to.
    sealed class ProgramCreateEvent : AsyncEvent, IDebugProgramCreateEvent2, IDebugEvent
    {
        static readonly Guid _IID = new Guid("96CD11EE-ECD4-4E89-957E-B5D496FC4139");

        public Guid IID
        {
            get { return _IID; }
        }
    }
    #endregion

    #region class ProgramDestroyEvent
    // This interface is sent by the debug engine (DE) to the session debug manager (SDM) when a program has run to completion
    // or is otherwise destroyed.
    sealed class ProgramDestroyEvent : SyncEvent, IDebugProgramDestroyEvent2, IDebugEvent
    {
        static readonly Guid _IID = new Guid("E147E9E3-6440-4073-A7B7-A65592C714B5");

        readonly uint m_exitCode;

        public ProgramDestroyEvent(uint exitCode)
        {
            m_exitCode = exitCode;
        }

        public Guid IID
        {
            get { return _IID; }
        }

        #region IDebugProgramDestroyEvent2 Members

        int IDebugProgramDestroyEvent2.GetExitCode(out uint exitCode)
        {
            exitCode = m_exitCode;

            return Const.S_OK;
        }

        #endregion
    }
    #endregion

    #region class ModuleLoadEvent
    // This interface is sent by the debug engine (DE) to the session debug manager (SDM) when a module is loaded or unloaded.
    sealed class ModuleLoadEvent : AsyncEvent, IDebugModuleLoadEvent2, IDebugEvent
    {
        static readonly Guid _IID = new Guid("989DB083-0D7C-40D1-A9D9-921BF611A4B2");

        readonly Module m_module;
        readonly bool m_fLoad;

        public ModuleLoadEvent(Module module, bool fLoad)
        {
            m_module = module;
            m_fLoad = fLoad;
        }

        int IDebugModuleLoadEvent2.GetModule(out IDebugModule2 module, ref string debugMessage, ref int fIsLoad)
        {
            module = m_module;

            if (m_fLoad)
            {
                debugMessage = String.Concat("Loaded '", m_module.Name, "'");
                fIsLoad = 1;
            }
            else
            {
                debugMessage = String.Concat("Unloaded '", m_module.Name, "'");
                fIsLoad = 0;
            }

            return Const.S_OK;
        }

        public Guid IID
        {
            get { return _IID; }
        }
    }
    #endregion

    #region class ThreadCreateEvent
    // This interface is sent by the debug engine (DE) to the session debug manager (SDM) when a thread is created in a program being debugged.
    sealed class ThreadCreateEvent : AsyncEvent, IDebugThreadCreateEvent2, IDebugEvent
    {
        static readonly Guid _IID = new Guid("2090CCFC-70C5-491D-A5E8-BAD2DD9EE3EA");

        public Guid IID
        {
            get { return _IID; }
        }
    }
    #endregion

    #region class ThreadDestroyEvent
    // This interface is sent by the debug engine (DE) to the session debug manager (SDM) when a thread has exited.
    sealed class ThreadDestroyEvent : AsyncEvent, IDebugThreadDestroyEvent2, IDebugEvent
    {
        static readonly Guid _IID = new Guid("2C3B7532-A36F-4A6E-9072-49BE649B8541");

        public Guid IID
        {
            get { return _IID; }
        }

        readonly uint m_exitCode;

        public ThreadDestroyEvent(uint exitCode)
        {
            m_exitCode = exitCode;
        }

        #region IDebugThreadDestroyEvent2 Members
        int IDebugThreadDestroyEvent2.GetExitCode(out uint exitCode)
        {
            exitCode = m_exitCode;
            
            return Const.S_OK;
        }
        #endregion
    }
    #endregion

    #region class LoadCompleteEvent
    // This interface is sent by the debug engine (DE) to the session debug manager (SDM) when a program is loaded, but before any code is executed.
    sealed class LoadCompleteEvent : StoppingEvent, IDebugLoadCompleteEvent2, IDebugEvent
    {
        static readonly Guid _IID = new Guid("B1844850-1349-45D4-9F12-495212F5EB0B");

        public Guid IID
        {
            get { return _IID; }
        }
    }
    #endregion

    #region class BreakCompleteEvent
    // This interface tells the session debug manager (SDM) that an asynchronous break has been successfully completed.
    sealed class BreakCompleteEvent : StoppingEvent, IDebugBreakEvent2, IDebugEvent
    {
        static readonly Guid _IID = new Guid("c7405d1d-e24b-44e0-b707-d8a5a4e1641b");

        public Guid IID
        {
            get { return _IID; }
        }
    }
    #endregion

    #region class OutputDebugStringEvent
    // This interface is sent by the debug engine (DE) to the session debug manager (SDM) to output a string for debug tracing.
    sealed class OutputDebugStringEvent : AsyncEvent, IDebugOutputStringEvent2, IDebugEvent
    {
        static readonly Guid _IID = new Guid("569c4bb1-7b82-46fc-ae28-4536ddad753e");

        public Guid IID
        {
            get { return _IID; }
        }

        readonly string m_str;

        public OutputDebugStringEvent(string str)
        {
            m_str = str;
        }

        #region IDebugOutputStringEvent2 Members
        int IDebugOutputStringEvent2.GetString(out string pbstrString)
        {
            pbstrString = m_str;
            return Const.S_OK;
        }
        #endregion
    }
    #endregion

    #region class SymbolSearchEvent
    // This interface is sent by the debug engine (DE) to indicate the results of searching for symbols for a module in the debuggee
    sealed class SymbolSearchEvent : AsyncEvent, IDebugSymbolSearchEvent2, IDebugEvent
    {
        static readonly Guid _IID = new Guid("638F7C54-C160-4c7b-B2D0-E0337BC61F8C");

        public Guid IID
        {
            get { return _IID; }
        }

        readonly Module m_module;
        readonly string m_searchInfo;
        readonly uint m_symbolFlags;

        public SymbolSearchEvent(Module module, string searchInfo, uint symbolFlags)
        {
            m_module = module;
            m_searchInfo = searchInfo;
            m_symbolFlags = symbolFlags;
        }

        #region IDebugSymbolSearchEvent2 Members

        int IDebugSymbolSearchEvent2.GetSymbolSearchInfo(out IDebugModule3 pModule, ref string pbstrDebugMessage, out uint pdwModuleInfoFlags)
        {
            pModule = m_module;
            pbstrDebugMessage = m_searchInfo;
            pdwModuleInfoFlags = m_symbolFlags;

            return Const.S_OK;
        }

        #endregion
    }
    #endregion

    #region class BreakpointBoundEvent
    // This interface is sent when a pending breakpoint has been bound in the debuggee.
    sealed class BreakpointBoundEvent : AsyncEvent, IDebugBreakpointBoundEvent2, IDebugEvent
    {
        static readonly Guid _IID = new Guid("1dddb704-cf99-4b8a-b746-dabb01dd13a0");

        public Guid IID
        {
            get { return _IID; }
        }

        readonly PendingBreakpoint m_pendingBreakpoint;
        readonly BoundBreakpoint m_boundBreakpoint;

        public BreakpointBoundEvent(PendingBreakpoint pendingBreakpoint, BoundBreakpoint boundBreakpoint)
        {
            m_pendingBreakpoint = pendingBreakpoint;
            m_boundBreakpoint = boundBreakpoint;
        }

        #region IDebugBreakpointBoundEvent2 Members

        int IDebugBreakpointBoundEvent2.EnumBoundBreakpoints(out IEnumDebugBoundBreakpoints2 ppEnum)
        {
            ppEnum = new BoundBreakpointsEnum(new IDebugBoundBreakpoint2[] {m_boundBreakpoint});
            return Const.S_OK;
        }

        int IDebugBreakpointBoundEvent2.GetPendingBreakpoint(out IDebugPendingBreakpoint2 ppPendingBP)
        {
            ppPendingBP = m_pendingBreakpoint;
            return Const.S_OK;
        }

        #endregion
    }
    #endregion

    #region class BreakpointUnboundEvent
    class BreakpointUnboundEvent : AsyncEvent, IDebugBreakpointUnboundEvent2, IDebugEvent
    {
        readonly IDebugBoundBreakpoint2 m_bp;

        public BreakpointUnboundEvent(IDebugBoundBreakpoint2 bp)
        {
            m_bp = bp;
        }

        public int GetBreakpoint(out IDebugBoundBreakpoint2 ppBP)
        {
            ppBP = m_bp;
            return Const.S_OK;
        }

        public int GetReason(out uint pdwUnboundReason)
        {
            pdwUnboundReason = 0;
            return Const.S_OK;
        }

        static readonly Guid _IID = new Guid("78d1db4f-c557-4dc5-a2dd-5369d21b1c8c");

        public Guid IID
        {
            get { return _IID; }
        }
    }
    #endregion

    #region class BreakpointEvent
    // This Event is sent when a breakpoint is hit in the debuggee
    sealed class BreakpointEvent : StoppingEvent, IDebugBreakpointEvent2, IDebugEvent
    {
        static readonly Guid _IID = new Guid("501C1E21-C557-48B8-BA30-A1EAB0BC4A74");

        public Guid IID
        {
            get { return _IID; }
        }

        readonly IEnumDebugBoundBreakpoints2 m_boundBreakpoints;

        public BreakpointEvent(IDebugBoundBreakpoint2 bp)
        {
            m_boundBreakpoints = new BoundBreakpointsEnum(new[] { bp });
        }

        public BreakpointEvent(IEnumDebugBoundBreakpoints2 boundBreakpoints)
        {
            m_boundBreakpoints = boundBreakpoints;
        }

        #region IDebugBreakpointEvent2 Members
        int IDebugBreakpointEvent2.EnumBreakpoints(out IEnumDebugBoundBreakpoints2 ppEnum)
        {
            ppEnum = m_boundBreakpoints;
            return Const.S_OK;
        }
        #endregion
    }
    #endregion

    #region class StepCompleteEvent
    sealed class StepCompleteEvent : StoppingEvent, IDebugStepCompleteEvent2, IDebugEvent
    {
        static readonly Guid _IID = new Guid("0f7f24c1-74d9-4ea6-a3ea-7edb2d81441d");

        public Guid IID
        {
            get { return _IID; }
        }
    }
    #endregion

    #region class StopCompleteEvent
    class StopCompleteEvent : AsyncEvent, IDebugStopCompleteEvent2, IDebugEvent
    {
        static readonly Guid _IID = new Guid("3dca9dcd-fb09-4af1-a926-45f293d48b2d");

        public Guid IID
        {
            get { return _IID; }
        }
    }
    #endregion
}
