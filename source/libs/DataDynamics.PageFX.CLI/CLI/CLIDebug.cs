#if DEBUG
using System;
using System.Diagnostics;
using System.IO;
using DataDynamics.PageFX.CLI.CFG;
using DataDynamics.PageFX.CodeModel;
using Microsoft.Win32;
using MethodBody=DataDynamics.PageFX.CLI.IL.MethodBody;

namespace DataDynamics.PageFX.CLI
{
    public static class CLIDebug
    {
        public const string KeyLastErrorType = "LastErrorType";
        public const string KeyLastErrorMethod = "LastErrorMethod";

        public static bool DumpILCode;
        public static bool DumpILMap;
        public static bool DumpSrc = true;
        public static bool VisualizeGraphBefore;
        public static bool VisualizeGraphAfter;
        public static bool VisualizeGraphStructuring;
        public static bool DebuggerBreak;
        public static bool RecordLastError;

        public static string TypeName;
        public static string MethodName;
        public static int Phase = -1;
        
        public static bool BreakWhileLoops;
        public static bool BreakDoWhileLoops;
        public static bool BreakEndlessLoops;
        public static bool BreakInvalidMetadataToken;
        public static bool BreakInvalidMemberReference;
        public static bool BreakInvalidTypeReference;

        internal static bool Filter;

        public static bool IsBreak
        {
            get { return Filter && DebuggerBreak; }
        }

        internal static void EvalFilter(IMethod method)
        {
            var type = method.DeclaringType;

            Filter = false;
            string lastErrorType = GetValue(KeyLastErrorType, "");
            string lastErrorMethod = GetValue(KeyLastErrorMethod, "");

            if (((string.IsNullOrEmpty(TypeName)
                || (type.Name == TypeName || type.FullName == TypeName))
                && method.Name == MethodName)
                || (type.FullName == lastErrorType
                && method.Name == lastErrorMethod))
            {
                Filter = true;
            }
        }

        public static bool IsCancel
        {
            get
            {
                if (CancelCallback != null)
                    return CancelCallback();
                return false;
            }
        }

        public static void DoCancel()
        {
            if (IsCancel)
                throw new Exception("Decompiler was canceled");
        }

        public static Func<bool> CancelCallback;

        public static void Reset()
        {
            DumpILCode = false;
            DumpILMap = true;
            DumpSrc = true;
            VisualizeGraphAfter = false;
            VisualizeGraphBefore = false;
            VisualizeGraphStructuring = false;
            RecordLastError = false;
            TypeName = null;
            MethodName = null;
            DebuggerBreak = false;
            Phase = -1;
            BreakWhileLoops = false;
            BreakDoWhileLoops = false;
            BreakEndlessLoops = false;
            BreakInvalidMetadataToken = false;
            BreakInvalidMemberReference = false;
            BreakInvalidTypeReference = false;
        }

        internal static void VisualizeGraph(MethodBody body, Node entry)
        {
            if (entry == null) return;
            var list = entry.GetGraphNodes();
            VisualizeGraph(body, list);  
        }

        internal static void VisualizeGraph(MethodBody body, NodeList list)
        {
            LogInfo("Flow graph constructed");
            DoCancel();

            DotService.NameService = null;

            if (Filter || VisualizeGraphBefore)
            {
                DotService.Write(list, DotService.MakePath(body, "before"), true);
            }

            if (Filter || DumpILCode)
            {
                DumpService.Dump(list, body, Infrastructure.TestCaseDirectory);
                LogInfo("IL code dumped");
            }
        }

        private const string HKEY_CURRENT_USER = "HKEY_CURRENT_USER";

        internal static string GetKeyName(string path)
        {
            string res = HKEY_CURRENT_USER + "\\Software\\Data Dynamics\\PageFX\\QA";
            string key = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(key))
                res += "\\" + key;
            return res;
        }

        internal static T GetValue<T>(string path, T defval)
        {
            string name = Path.GetFileName(path);
            return (T)Registry.GetValue(GetKeyName(path), name, defval);
        }

        internal static bool GetValue(string path, bool defval)
        {
            string name = Path.GetFileName(path);
            var v = Registry.GetValue(GetKeyName(path), name, defval);
            var c = v as IConvertible;
            if (c != null) return c.ToBoolean(null);
            return defval;
        }

        internal static void SetValue(string path, object value)
        {
            string name = Path.GetFileName(path);
            Registry.SetValue(GetKeyName(path), name, value);
        }

        internal static void RemoveValue(string path)
        {
            try
            {
            	string name = Path.GetFileName(path);
            	string key = GetKeyName(path);
            	key = key.Substring(HKEY_CURRENT_USER.Length + 1);
            	var registryKey = Registry.CurrentUser.OpenSubKey(key, true);
            	if (registryKey == null) return;
            	using (registryKey)
            	{
            		registryKey.DeleteValue(name);
            	}
            }
            catch (Exception e)
            {
				Trace.TraceError("{0}", e);
            }
        }

        internal static void SetLastError(MethodBody body)
        {
            SetLastError(body.Method);
        }

        internal static bool HasLastError;

        internal static void SetLastError(IMethod method)
        {
            if (HasLastError) return;
            if (RecordLastError)
            {
                SetValue(KeyLastErrorType, method.DeclaringType.FullName);
                SetValue(KeyLastErrorMethod, method.Name);
            }
            HasLastError = true;
        }

        public static void ResetLastError()
        {
            RemoveValue(KeyLastErrorType);
            RemoveValue(KeyLastErrorMethod);
            HasLastError = false;
        }

        #region Logging
        public static TextWriter Log;

        internal static void LogInfo(string msg)
        {
            if (Log != null)
            {
                Log.WriteLine(msg);
            }
        }

        internal static void LogInfo(string format, params object[] args)
        {
            if (Log != null)
            {
                Log.WriteLine(format, args);
            }
        }

        internal static void LogError(string msg)
        {
            if (Log != null)
            {
                Log.WriteLine("ERROR: {0}", msg);
            }
        }

        internal static void LogError(string format, params object[] args)
        {
            if (Log != null)
            {
                string msg = string.Format(format, args);
                Log.WriteLine("ERROR: {0}", msg);
            }
        }

        internal static void LogWarning(string msg)
        {
            if (Log != null)
            {
                Log.WriteLine("WARNING: {0}", msg);
            }
        }

        internal static void LogWarning(string format, params object[] args)
        {
            if (Log != null)
            {
                string msg = string.Format(format, args);
                Log.WriteLine("WARNING: {0}", msg);
            }
        }

        private static readonly string sep = new string('-', 200);

        internal static void LogSeparator()
        {
            if (Log != null)
                Log.WriteLine(sep);
        }
        #endregion
    }
}
#endif