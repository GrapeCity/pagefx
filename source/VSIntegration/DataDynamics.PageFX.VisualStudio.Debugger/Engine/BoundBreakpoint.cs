using Microsoft.VisualStudio.Debugger.Interop;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    // This class represents a breakpoint that has been bound to a location in the debuggee. It is a child of the pending breakpoint
    // that creates it. Unless the pending breakpoint only has one bound breakpoint, each bound breakpoint is displayed as a child of the
    // pending breakpoint in the breakpoints window. Otherwise, only one is displayed.
    class BoundBreakpoint : IDebugBoundBreakpoint2
    {
        readonly PendingBreakpoint m_pendingBreakpoint;
        readonly BreakpointResolution m_breakpointResolution;
        
        public BoundBreakpoint(PendingBreakpoint pendingBreakpoint, BreakpointResolution breakpointResolution)
        {
            m_pendingBreakpoint = pendingBreakpoint;
            m_breakpointResolution = breakpointResolution;
        }

        #region IDebugBoundBreakpoint2 Members
        // Called when the breakpoint is being deleted by the user.
        public int Delete()
        {
            return m_pendingBreakpoint.Delete();
        }

        // Called by the debugger UI when the user is enabling or disabling a breakpoint.
        public int Enable(int fEnable)
        {
            return m_pendingBreakpoint.Enable(fEnable);
        }

        // Return the breakpoint resolution which describes how the breakpoint bound in the debuggee.
        int IDebugBoundBreakpoint2.GetBreakpointResolution(out IDebugBreakpointResolution2 ppBPResolution)
        {
            ppBPResolution = m_breakpointResolution;
            return Const.S_OK;
        }

        // Return the pending breakpoint for this bound breakpoint.
        int IDebugBoundBreakpoint2.GetPendingBreakpoint(out IDebugPendingBreakpoint2 ppPendingBreakpoint)
        {
            ppPendingBreakpoint = m_pendingBreakpoint;
            return Const.S_OK;
        }

        public bool IsDeleted
        {
            get { return m_pendingBreakpoint.IsDeleted; }
        }

        public bool IsEnabled
        {
            get { return m_pendingBreakpoint.IsEnabled; }
        }

        // 
        int IDebugBoundBreakpoint2.GetState(out uint pState)
        {
            if (IsDeleted)
            {
                pState = (uint)enum_BP_STATE.BPS_DELETED;
            }
            else
            {
                pState = IsEnabled 
                    ? (uint)enum_BP_STATE.BPS_ENABLED
                    : (uint)enum_BP_STATE.BPS_DISABLED;
            }
            return Const.S_OK;
        }

        public int HitCount { get; set; }

        public bool Hit()
        {
            HitCount++;
            var pc = m_pendingBreakpoint.PassCount;
            switch ((enum_BP_PASSCOUNT_STYLE)pc.stylePassCount)
            {
                case enum_BP_PASSCOUNT_STYLE.BP_PASSCOUNT_EQUAL:
                    return HitCount == pc.dwPassCount;

                case enum_BP_PASSCOUNT_STYLE.BP_PASSCOUNT_EQUAL_OR_GREATER:
                    return HitCount >= pc.dwPassCount;

                case enum_BP_PASSCOUNT_STYLE.BP_PASSCOUNT_MOD:
                    return (HitCount % pc.dwPassCount) == 0;
            }
            return true;
        }

        // The sample engine does not support hit counts on breakpoints. A real-world debugger will want to keep track 
        // of how many times a particular bound breakpoint has been hit and return it here.
        int IDebugBoundBreakpoint2.GetHitCount(out uint pdwHitCount)
        {
            pdwHitCount = (uint)HitCount;
            return Const.S_OK;
        }

        // The sample engine does not support hit counts on breakpoints. A real-world debugger will want to keep track 
        // of how many times a particular bound breakpoint has been hit. The debugger calls SetHitCount when the user 
        // resets a breakpoint's hit count.
        int IDebugBoundBreakpoint2.SetHitCount(uint dwHitCount)
        {
            HitCount = (int)dwHitCount;
            return Const.S_OK;
        }

        // The sample engine does not support conditions on breakpoints.
        // A real-world debugger will use this to specify when a breakpoint will be hit
        // and when it should be ignored.
        public int SetCondition(BP_CONDITION bpCondition)
        {
            return m_pendingBreakpoint.SetCondition(bpCondition);
        }

        // The sample engine does not support pass counts on breakpoints.
        // This is used to specify the breakpoint hit count condition.
        public int SetPassCount(BP_PASSCOUNT bpPassCount)
        {
            HitCount = 0;
            m_pendingBreakpoint.PassCount = bpPassCount;
            return Const.S_OK;
        }
        #endregion
    }
}
