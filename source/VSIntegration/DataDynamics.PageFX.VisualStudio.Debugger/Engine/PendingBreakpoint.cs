using System;
using System.Runtime.InteropServices;
using DataDynamics.PageFX.FLI;
using Microsoft.VisualStudio.Debugger.Interop;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    // This class represents a pending breakpoint which is an abstract representation of a breakpoint before it is bound.
    // When a user creates a new breakpoint, the pending breakpoint is created and is later bound. The bound breakpoints
    // become children of the pending breakpoint.
    class PendingBreakpoint : IDebugPendingBreakpoint2
    {       
        // The breakpoint request that resulted in this pending breakpoint being created.
        readonly IDebugBreakpointRequest2 m_request;
        BP_REQUEST_INFO m_reqInfo;
        readonly Engine m_engine;
        
        BoundBreakpoint m_boundBreakpoint;
        readonly SourceLocation m_location;
        public string OldValue = ""; //used for conditional breakpoints

        bool m_enabled = true;
        bool m_deleted;
        readonly uint m_addr;

        public PendingBreakpoint(
            IDebugBreakpointRequest2 request,
            BP_REQUEST_INFO info,
            SourceLocation location, 
            Engine engine, int index)
        {
            m_request = request;
            m_reqInfo = info;
            m_location = location;
            
            m_engine = engine;
            m_addr = (uint)(index + 0x8000);
        }

        public enum_BP_FLAGS Flags
        {
            get { return (enum_BP_FLAGS)m_reqInfo.dwFlags; }
        }

        public BoundBreakpoint BoundBreakpoint
        {
            get { return m_boundBreakpoint; }
        }

        public string File
        {
            get { return m_location.File; }
        }

        public int Line
        {
            get { return m_location.Line; }
        }

        public string ID { get; set; }

        public string EncodedFile
        {
            get 
            {
                if (m_encodedFile == null)
                    m_encodedFile = DebugUtils.Encode(File);
                return m_encodedFile;
            }
        }
        string m_encodedFile;

        bool CanBind()
        {
            if (m_boundBreakpoint != null)
                return false;
            if (m_deleted)
                return false;

            //only support breakpoints on a file and line number.
            if (m_reqInfo.bpLocation.bpLocationType != (uint)enum_BP_LOCATION_TYPE.BPLT_CODE_FILE_LINE)
            {
                return false;
            }

            return true;
        }

        // Get the document context for this pending breakpoint. A document context is a abstract representation of a source file 
        // location.
        public DocumentContext GetDocumentContext(uint address)
        {
            var docPosition = (IDebugDocumentPosition2)(Marshal.GetObjectForIUnknown(m_reqInfo.bpLocation.unionmember2));
            string documentName;
            Util.CheckOk(docPosition.GetFileName(out documentName));

            // Get the location in the document that the breakpoint is in.
            var startPosition = new TEXT_POSITION[1];
            var endPosition = new TEXT_POSITION[1];
            Util.CheckOk(docPosition.GetRange(startPosition, endPosition));           

            var codeContext = new CodeContext(m_engine, address);
            
            return new DocumentContext(documentName, startPosition[0], endPosition[0], codeContext);
        }

        // Remove all of the bound breakpoints for this pending breakpoint
        public void ClearBoundBreakpoints()
        {
            if (m_boundBreakpoint != null)
            {
                m_boundBreakpoint.Delete();
            }
        }

        public BP_PASSCOUNT PassCount
        {
            get { return m_reqInfo.bpPassCount; }
            set { m_reqInfo.bpPassCount = value; }
        }

        #region IDebugPendingBreakpoint2 Members
        // Binds this pending breakpoint to one or more code locations.
        public int Bind()
        {
            try
            {
                if (CanBind())
                {
                    if (m_engine.Debugger.BindBreakpoint(this))
                    {
                        var breakpointResolution = new BreakpointResolution(m_engine, m_addr, GetDocumentContext(m_addr));

                        m_boundBreakpoint = new BoundBreakpoint(this, breakpointResolution);
                        
                        m_engine.Send(new BreakpointBoundEvent(this, m_boundBreakpoint));

                        return Const.S_OK;
                    }
                }
                // The breakpoint could not be bound. This may occur for many reasons such as an invalid location, an invalid expression, etc...
                // The sample engine does not support this, but a real world engine will want to send an instance of IDebugBreakpointErrorEvent2 to the
                // UI and return a valid instance of IDebugErrorBreakpoint2 from IDebugPendingBreakpoint2::EnumErrorBreakpoints. The debugger will then
                // display information about why the breakpoint did not bind to the user.
                return Const.S_FALSE;
            }
            catch (Exception e)
            {
                return Util.UnexpectedException(e);
            }
        }

        // Determines whether this pending breakpoint can bind to a code location.
        int IDebugPendingBreakpoint2.CanBind(out IEnumDebugErrorBreakpoints2 ppErrorEnum)
        {
            ppErrorEnum = null;

            if (!CanBind())
            {
                // Called to determine if a pending breakpoint can be bound. 
                // The breakpoint may not be bound for many reasons such as an invalid location, an invalid expression, etc...
                // The sample engine does not support this, but a real world engine will want to return a valid enumeration of IDebugErrorBreakpoint2.
                // The debugger will then display information about why the breakpoint did not bind to the user.
                return Const.S_FALSE;
            }

            return Const.S_OK;
        }

        public bool IsDeleted
        {
            get { return m_deleted; }
        }

        public bool IsEnabledNow = true;

        public bool IsEnabled
        {
            get { return m_enabled; }
        }

        // Deletes this pending breakpoint and all breakpoints bound from it.
        public int Delete()
        {
            if (m_deleted) return Const.S_OK;
            if (m_engine.RemoveBreakPoint(this))
            {
                m_deleted = true;
                m_boundBreakpoint = null;
                return Const.S_OK;
            }
            return Const.E_FAIL;
        }

        // Toggles the enabled state of this pending breakpoint.
        public int Enable(int fEnable)
        {
            bool f = fEnable != 0;
            if (m_enabled == f) return Const.S_OK;
            m_engine.EnableBreakpoint(this, f);
            m_enabled = f;
            return Const.S_OK;
        }

        // Enumerates all breakpoints bound from this pending breakpoint
        int IDebugPendingBreakpoint2.EnumBoundBreakpoints(out IEnumDebugBoundBreakpoints2 ppEnum)
        {
            lock (this)
            {
                ppEnum = m_boundBreakpoint != null 
                    ? new BoundBreakpointsEnum(new IDebugBoundBreakpoint2[] { m_boundBreakpoint }) 
                    : new BoundBreakpointsEnum(new IDebugBoundBreakpoint2[0]);
            }
            return Const.S_OK;
        }

        // Enumerates all error breakpoints that resulted from this pending breakpoint.
        int IDebugPendingBreakpoint2.EnumErrorBreakpoints(uint bpErrorType, out IEnumDebugErrorBreakpoints2 ppEnum)
        {
            // Called when a pending breakpoint could not be bound. This may occur for many reasons such as an invalid location, an invalid expression, etc...
            // The sample engine does not support this, but a real world engine will want to send an instance of IDebugBreakpointErrorEvent2 to the
            // UI and return a valid enumeration of IDebugErrorBreakpoint2 from IDebugPendingBreakpoint2::EnumErrorBreakpoints. The debugger will then
            // display information about why the breakpoint did not bind to the user.
            ppEnum = null;
            return Const.E_NOTIMPL;
        }

        // Gets the breakpoint request that was used to create this pending breakpoint
        int IDebugPendingBreakpoint2.GetBreakpointRequest(out IDebugBreakpointRequest2 ppBPRequest)
        {
            ppBPRequest = m_request;
            return Const.S_OK;
        }

        // Gets the state of this pending breakpoint.
        int IDebugPendingBreakpoint2.GetState(PENDING_BP_STATE_INFO[] pState)
        {
            if (m_deleted)
            {
                pState[0].state = (uint)enum_BP_STATE.BPS_DELETED;
            }
            else
            {
                pState[0].state = m_enabled
                                      ? (uint)enum_BP_STATE.BPS_ENABLED
                                      : (uint)enum_BP_STATE.BPS_DISABLED;
            }
            return Const.S_OK;
        }

        public bool HasCondition
        {
            get
            {
                return m_reqInfo.bpCondition.styleCondition != 0
                    && !string.IsNullOrEmpty(m_reqInfo.bpCondition.bstrCondition);
            }
        }

        public string ConditionalExpression
        {
            get 
            { 
                if (HasCondition)
                    return m_reqInfo.bpCondition.bstrCondition;
                return "";
            }
        }

        public enum_BP_COND_STYLE ConditionStyle
        {
            get 
            {
                return (enum_BP_COND_STYLE)m_reqInfo.bpCondition.styleCondition;
            }
        }

        public int SetCondition(BP_CONDITION bpCondition)
        {
            m_reqInfo.bpCondition = bpCondition;
            return Const.S_OK;

            //if ((enum_BP_COND_STYLE)bpCondition.styleCondition == enum_BP_COND_STYLE.BP_COND_WHEN_TRUE)
            //{
            //    m_reqInfo.bpCondition = bpCondition;
            //    m_engine.Debugger.SetCondition(this);
            //    return Const.S_OK;
            //}
            //return Const.E_NOTIMPL;
        }

        // The sample engine does not support pass counts on breakpoints.
        int IDebugPendingBreakpoint2.SetPassCount(BP_PASSCOUNT bpPassCount)
        {
            PassCount = bpPassCount;
            return Const.S_OK;
        }

        // Toggles the virtualized state of this pending breakpoint. When a pending breakpoint is virtualized, 
        // the debug engine will attempt to bind it every time new code loads into the program.
        // The sample engine will does not support this.
        int IDebugPendingBreakpoint2.Virtualize(int fVirtualize)
        {
            return Const.S_OK;
        }
        #endregion
    }
}
