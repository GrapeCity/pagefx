using System.IO;

namespace DataDynamics.PageFX
{
    internal static class Utils
    {
        public static string RemovePrefix(string s, string prefix)
        {
            if (s.StartsWith(prefix))
                return s.Substring(prefix.Length);
            return s;
        }

        public static void WriteTemplate(string path, string template, params string[] vars)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            var T = typeof(Program).GetTextResource(template);
            string result = Str.ReplaceVars(T, vars);
            File.WriteAllText(path, result);
        }
    }
}