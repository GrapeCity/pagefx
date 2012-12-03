namespace System.IO
{
    internal static class PfxIO
    {
        public static string CurrentDirectory
        {
            get
            {
                if (Environment.IsRunningOnWindows)
                    return "c:";
                return "";
            }
            set { throw new NotSupportedException(); }
        }

        public static char VolumeSeparatorChar
        {
            get
            {
                if (Environment.IsRunningOnWindows)
                    return ':';
                return '/';
            }
        }

        public static char DirectorySeparatorChar
        {
            get
            {
                if (Environment.IsRunningOnWindows)
                    return '\\';
                return '/';
            }
        }

        public static char AltDirectorySeparatorChar
        {
            get
            {
                if (Environment.IsRunningOnWindows)
                    return '/';
                return '\\';
            }
        }

        public static char PathSeparator
        {
            get
            {
                return ';';
            }
        }

        public const string NewLine = "\n";
    }
}