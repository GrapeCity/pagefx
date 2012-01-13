using Microsoft.VisualStudio.Debugger.Interop;

// These classes use a generic enumerator implementation to create the various enumerators required by the engine.
// They allow the enumeration of everything from programs to breakpoints.
namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    #region Base Class
    class EnumImpl<T,I> where I: class
    {
        readonly T[] m_data;
        uint m_position;

        public EnumImpl(T[] data)
        {
            m_data = data;
            m_position = 0;
        }

        public int Clone(out I ppEnum)
        {
            ppEnum = null;
            return Const.E_NOTIMPL;
        }

        public int GetCount(out uint pcelt)
        {
            pcelt = (uint)m_data.Length;
            return Const.S_OK;
        }

        public int Next(uint celt, T[] rgelt, out uint celtFetched)
        {
            return Move(celt, rgelt, out celtFetched);
        }

        public int Reset()
        {
            lock (this)
            {
                m_position = 0;

                return Const.S_OK;
            }
        }

        public int Skip(uint celt)
        {
            uint celtFetched;

            return Move(celt, null, out celtFetched);
        }

        private int Move(uint celt, T[] rgelt, out uint celtFetched)
        {
            lock (this)
            {
                int hr = Const.S_OK;
                celtFetched = (uint)m_data.Length - m_position;

                if (celt > celtFetched)
                {
                    hr = Const.S_FALSE;
                }
                else if (celt < celtFetched)
                {
                    celtFetched = celt;
                }

                if (rgelt != null)
                {
                    for (int c = 0; c < celtFetched; c++)
                    {
                        rgelt[c] = m_data[m_position + c];
                    }
                }

                m_position += celtFetched;

                return hr;
            }
        }
    }
    #endregion Base Class

    class ProgramEnum : EnumImpl<IDebugProgram2, IEnumDebugPrograms2>, IEnumDebugPrograms2
    {
        public ProgramEnum(IDebugProgram2[] data) : base(data)
        {
        }

        public int Next(uint celt, IDebugProgram2[] rgelt, ref uint celtFetched)
        {
            return Next(celt, rgelt, out celtFetched);
        }
    }

    class FrameInfoEnum : EnumImpl<FRAMEINFO, IEnumDebugFrameInfo2>, IEnumDebugFrameInfo2
    {
        public FrameInfoEnum(FRAMEINFO[] data)
            : base(data)
        {
        }

        public int Next(uint celt, FRAMEINFO[] rgelt, ref uint celtFetched)
        {
            return Next(celt, rgelt, out celtFetched);
        }
    }

    class PropertyInfoEnum : EnumImpl<DEBUG_PROPERTY_INFO, IEnumDebugPropertyInfo2>, IEnumDebugPropertyInfo2
    {
        public PropertyInfoEnum(DEBUG_PROPERTY_INFO[] data)
            : base(data)
        {
        }
    }

    class ThreadEnum : EnumImpl<IDebugThread2, IEnumDebugThreads2>, IEnumDebugThreads2
    {
        public ThreadEnum(IDebugThread2[] threads)
            : base(threads)
        {
            
        }

        public int Next(uint celt, IDebugThread2[] rgelt, ref uint celtFetched)
        {
            return Next(celt, rgelt, out celtFetched);
        }
    }

    class ModuleEnum : EnumImpl<IDebugModule2, IEnumDebugModules2>, IEnumDebugModules2
    {
        public ModuleEnum(IDebugModule2[] modules)
            : base(modules)
        {

        }

        public int Next(uint celt, IDebugModule2[] rgelt, ref uint celtFetched)
        {
            return Next(celt, rgelt, out celtFetched);
        }
    }

    class PropertyEnum : EnumImpl<DEBUG_PROPERTY_INFO, IEnumDebugPropertyInfo2>, IEnumDebugPropertyInfo2
    {
        public PropertyEnum(DEBUG_PROPERTY_INFO[] properties)
            : base(properties)
        {

        }
    }

    class CodeContextEnum : EnumImpl<IDebugCodeContext2, IEnumDebugCodeContexts2>, IEnumDebugCodeContexts2
    {
        public CodeContextEnum(IDebugCodeContext2[] codeContexts)
            : base(codeContexts)
        {

        }

        public int Next(uint celt, IDebugCodeContext2[] rgelt, ref uint celtFetched)
        {
            return Next(celt, rgelt, out celtFetched);
        }
    }

    class BoundBreakpointsEnum : EnumImpl<IDebugBoundBreakpoint2, IEnumDebugBoundBreakpoints2>, IEnumDebugBoundBreakpoints2
    {
        public BoundBreakpointsEnum(IDebugBoundBreakpoint2[] breakpoints)
            : base(breakpoints)
        {

        }

        public int Next(uint celt, IDebugBoundBreakpoint2[] rgelt, ref uint celtFetched)
        {
            return Next(celt, rgelt, out celtFetched);
        }
    }  

}
