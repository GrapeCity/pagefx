using System;
using System.IO;
using System.Linq;

namespace DataDynamics.PageFX
{
    internal static class FlashPlayerTrust
    {
        private static bool IsWindows
        {
            get { return Path.DirectorySeparatorChar == '\\'; }
        }

		private static string GetSecurityDir()
        {
            if (IsWindows)
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(dir, @"Macromedia\Flash Player\#Security");
            }
            else
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                return Path.Combine(dir, "Library/Preferences/Macromedia/Flash Player/#Security");
            }
        }

		private static string GetTrustDir()
        {
            return Path.Combine(GetSecurityDir(), "FlashPlayerTrust");
        }

		private static bool Contains(string parent, string path)
        {
            if (string.IsNullOrEmpty(parent))
                return false;
            if (string.IsNullOrEmpty(path))
                return false;
            if (!Path.IsPathRooted(parent))
                return false;
            return path.StartsWith(parent,
                                   IsWindows
                                       ? StringComparison.OrdinalIgnoreCase
                                       : StringComparison.Ordinal);
        }

        public static void AddPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("path is null or empty", "path");

            try
            {
                string dir = GetTrustDir();

                string cfgfile = Path.Combine(dir, "pfx.cfg");

                if (File.Exists(cfgfile))
                {
                    string[] lines = File.ReadAllLines(cfgfile);
                    int n = lines.Length;
                    if (n == 0)
                    {
                        File.WriteAllLines(cfgfile, new[] { path });
                        return;
                    }
                    if (lines.Any(l => Contains(l, path)))
                        return;
                    Array.Resize(ref lines, n + 1);
                    lines[n] = path;
                    File.WriteAllLines(cfgfile, lines);
                }
                else
                {
                    Directory.CreateDirectory(dir);
                    File.WriteAllLines(cfgfile, new[] { path });
                }
            }
            catch (Exception e)
            {
                //TODO: Log warning
            }
        }
    }
}