using System;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    // These are well-known guids in AD7. Most of these are used to specify filters in what the debugger UI is requesting.
    // For instance, guidFilterLocals can be passed to IDebugStackFrame2::EnumProperties to specify only locals are requested.
    static class Guids
    {
        static readonly Guid _FilterRegisters = new Guid("223ae797-bd09-4f28-8241-2763bdc5f713");
        static public Guid FilterRegisters
        {
            get { return _FilterRegisters; }
        }

        static readonly Guid _FilterLocals = new Guid("b200f725-e725-4c53-b36a-1ec27aef12ef");
        static public Guid FilterLocals
        {
            get { return _FilterLocals; }
        }

        static readonly Guid _FilterAllLocals = new Guid("196db21f-5f22-45a9-b5a3-32cddb30db06");
        static public Guid FilterAllLocals
        {
            get { return _FilterAllLocals; }
        }

        static readonly Guid _FilterArgs = new Guid("804bccea-0475-4ae7-8a46-1862688ab863");
        static public Guid FilterArgs
        {
            get { return _FilterArgs; }
        }

        static readonly Guid _FilterLocalsPlusArgs = new Guid("e74721bb-10c0-40f5-807f-920d37f95419");
        static public Guid FilterLocalsPlusArgs
        {
            get { return _FilterLocalsPlusArgs; }
        }

        static readonly Guid _FilterAllLocalsPlusArgs = new Guid("939729a8-4cb0-4647-9831-7ff465240d5f");
        static public Guid FilterAllLocalsPlusArgs
        {
            get { return _FilterAllLocalsPlusArgs; }
        }

        // Language guid for C++. Used when the language for a document context or a stack frame is requested.
        static readonly Guid _LanguageCpp = new Guid("3a12d0b7-c26c-11d0-b442-00a0244a1dd2");
        static public Guid LanguageCpp
        {
            get { return _LanguageCpp; }
        }

        // Language guid for C#. Used when the language for a document context or a stack frame is requested.
        static readonly Guid _LanguageCSharp = new Guid("3F5162F8-07C6-11D3-9053-00C04FA302A1");
        static public Guid LanguageCSharp
        {
            get { return _LanguageCSharp; }
        }
    }
}
