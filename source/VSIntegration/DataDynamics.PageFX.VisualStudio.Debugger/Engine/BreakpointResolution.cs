using System;
using Microsoft.VisualStudio.Debugger.Interop;
using System.Runtime.InteropServices;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    // This class represents the information that describes a bound breakpoint.
    class BreakpointResolution : IDebugBreakpointResolution2
    {
        readonly Engine m_engine;
        readonly uint m_address;
        readonly DocumentContext m_documentContext;

        public BreakpointResolution(Engine engine, uint address, DocumentContext documentContext)
        {
            m_engine = engine;
            m_address = address;
            m_documentContext = documentContext;
        }

        #region IDebugBreakpointResolution2 Members

        // Gets the type of the breakpoint represented by this resolution. 
        int IDebugBreakpointResolution2.GetBreakpointType(out uint pBPType)
        {
            // The sample engine only supports code breakpoints.
            pBPType = (uint)enum_BP_TYPE.BPT_CODE;
            return Const.S_OK;
        }

        // Gets the breakpoint resolution information that describes this breakpoint.
        int IDebugBreakpointResolution2.GetResolutionInfo(uint dwFields, BP_RESOLUTION_INFO[] pBPResolutionInfo)
        {
	        if ((dwFields & (uint)enum_BPRESI_FIELDS.BPRESI_BPRESLOCATION) != 0) 
            {
                // The sample engine only supports code breakpoints.
                var location = new BP_RESOLUTION_LOCATION();
                location.bpType = (uint)enum_BP_TYPE.BPT_CODE;

                // The debugger will not QI the IDebugCodeContex2 interface returned here. We must pass the pointer
                // to IDebugCodeContex2 and not IUnknown.
                var codeContext = new CodeContext(m_engine, m_address);
                codeContext.SetDocumentContext(m_documentContext);
                location.unionmember1 = Marshal.GetComInterfaceForObject(codeContext, typeof(IDebugCodeContext2));
                pBPResolutionInfo[0].bpResLocation = location;
                pBPResolutionInfo[0].dwFields |= (uint)enum_BPRESI_FIELDS.BPRESI_BPRESLOCATION;

            }
	        
            if ((dwFields & (uint)enum_BPRESI_FIELDS.BPRESI_PROGRAM) != 0) 
            {
                pBPResolutionInfo[0].pProgram = m_engine;
                pBPResolutionInfo[0].dwFields |= (uint)enum_BPRESI_FIELDS.BPRESI_PROGRAM;
            }
	       
            return Const.S_OK;
        }

        #endregion
    }

    class ErrorBreakpointResolution : IDebugErrorBreakpointResolution2
    {
        #region IDebugErrorBreakpointResolution2 Members

        int IDebugErrorBreakpointResolution2.GetBreakpointType(out uint pBPType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        int IDebugErrorBreakpointResolution2.GetResolutionInfo(uint dwFields, BP_ERROR_RESOLUTION_INFO[] pErrorResolutionInfo)
        {
            if ((dwFields & (uint)enum_BPERESI_FIELDS.BPERESI_BPRESLOCATION) != 0) {}
            if ((dwFields & (uint)enum_BPERESI_FIELDS.BPERESI_PROGRAM) != 0) {}
            if ((dwFields & (uint)enum_BPERESI_FIELDS.BPERESI_THREAD) != 0) {}
            if ((dwFields & (uint)enum_BPERESI_FIELDS.BPERESI_MESSAGE) != 0) {}
            if ((dwFields & (uint)enum_BPERESI_FIELDS.BPERESI_TYPE) != 0) {}

            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

}
