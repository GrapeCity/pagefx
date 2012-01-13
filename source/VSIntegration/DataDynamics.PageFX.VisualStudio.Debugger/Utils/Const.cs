using System;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    static class Const
    {
        public const int TIMEOUT = -100;

        public const int S_OK = 0;
        public const int S_FALSE = 1;

        public const int E_NOTIMPL = unchecked((int)0x80004001);
        public const int E_FAIL = unchecked((int)0x80004005);

        public const int RPC_E_SERVERFAULT = unchecked((int)0x80010105);

        public const string DebugEngineName = "PageFX Debugger";

        public const string DebugEngineCLSID = "170D1DD5-79EC-41d9-81C3-4684A2E89338";

        /// <summary>
        /// This is the engine GUID of the engine. It needs to be changed here and in the registration
        /// when creating a new engine.
        /// </summary>
        public static readonly Guid DebugEngineId = new Guid("E76FB498-F02D-49a9-8B5B-B9E50F6A03E9");

        public const string ProgramProviderCLSID = "E1315474-071E-4a22-8BC2-A3AB34AB6442";

        public const string LangCharp = "CSharp";
    }
}