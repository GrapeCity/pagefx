using System;

namespace mono_test_adaptor
{
    class VB
    {
        public static bool StartsWith(string line, string prefix)
        {
            if (string.IsNullOrEmpty(line)) return false;
            line = line.Trim();
            if (string.IsNullOrEmpty(line)) return false;
            return line.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsModuleBegin(string line)
        {
            return StartsWith(line, "Module");
        }

        public static bool IsModuleEnd(string line)
        {
            return StartsWith(line, "End Module");
        }

        public static bool IsMainSub(string line)
        {
            if (string.IsNullOrEmpty(line)) return false;
            return line.ToLower().Contains("sub main");
        }

        public static bool IsMainFunction(string line)
        {
            if (string.IsNullOrEmpty(line)) return false;
            return line.ToLower().Contains("function main");
        }

        public static bool IsEndSub(string line)
        {
            return StartsWith(line, "End Sub");
        }

        public static bool IsEndFunction(string line)
        {
            return StartsWith(line, "End Function");
        }
    }
}