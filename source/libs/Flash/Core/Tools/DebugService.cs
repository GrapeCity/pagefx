#if DEBUG
using System;
using System.IO;

namespace DataDynamics.PageFX.FlashLand.Core.Tools
{
    public static class DebugService
    {
        public static bool AvmDump;
        public static bool AbcDump;
        public static bool DumpILMap;

        /// <summary>
        /// To break when unresolved internal call is occured.
        /// </summary>
        public static bool BreakInternalCall;

        public static bool IsCancel
        {
            get
            {
                if (CancelCallback != null)
                    return CancelCallback();
                return false;
            }
        }

	    public static Func<bool> CancelCallback;

        public static TextWriter Log;

        public static void LogInfo(string msg)
        {
            if (Log != null)
            {
                Log.WriteLine(msg);
            }
        }

        public static void LogInfo(string format, params object[] args)
        {
            if (Log != null)
            {
                Log.WriteLine(format, args);
            }
        }

        public static void LogError(string msg)
        {
            if (Log != null)
            {
                Log.WriteLine("ERROR: {0}", msg);
            }
        }

        public static void LogError(string format, params object[] args)
        {
            if (Log != null)
            {
                string msg = string.Format(format, args);
                Log.WriteLine("ERROR: {0}", msg);
            }
        }

        public static void LogWarning(string msg)
        {
            if (Log != null)
            {
                Log.WriteLine("WARNING: {0}", msg);
            }
        }

        public static void LogWarning(string format, params object[] args)
        {
            if (Log != null)
            {
                string msg = string.Format(format, args);
                Log.WriteLine("WARNING: {0}", msg);
            }
        }

        private static readonly string sep = new string('-', 200);

        public static void LogSeparator()
        {
            if (Log != null)
                Log.WriteLine(sep);
        }
    }
}
#endif