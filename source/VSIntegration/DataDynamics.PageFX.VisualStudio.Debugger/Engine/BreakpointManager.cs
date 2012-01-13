using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.Debugger.Interop;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    // This class manages breakpoints for the engine. 
    class BreakpointManager : IEnumerable<PendingBreakpoint>
    {
        readonly Engine m_engine;
        readonly List<PendingBreakpoint> m_list;

        public BreakpointManager(Engine engine)
        {
            m_engine = engine;
            m_list = new List<PendingBreakpoint>();
        }
      
        // A helper method used to construct a new pending breakpoint.
        public PendingBreakpoint CreatePendingBreakpoint(IDebugBreakpointRequest2 request)
        {
            var info = Util.GetRequestInfo(request);
            var loc = Util.GetSourceLocation(info);
            //var pb = Util.Find(m_list,
            //                   b => b.Line == loc.Line
            //                        && string.Compare(b.File, loc.File, true) == 0);
            //if (pb != null) return pb;
            var pb = new PendingBreakpoint(request, info, loc, m_engine, Count);
            m_list.Add(pb);
            return pb;
        }

        // Called from the engine's detach method to remove the debugger's breakpoint instructions.
        public void ClearBoundBreakpoints()
        {
            foreach (var pendingBreakpoint in m_list)
            {
                pendingBreakpoint.ClearBoundBreakpoints();
            }
        }

        public int Count
        {
            get { return m_list.Count; }
        }

        public IEnumerator<PendingBreakpoint> GetEnumerator()
        {
            return m_list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Remove(PendingBreakpoint bp)
        {
            m_list.Remove(bp);
        }
    }
}
