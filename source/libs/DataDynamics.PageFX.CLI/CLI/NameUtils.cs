namespace DataDynamics.PageFX.CLI
{
    public static class NameUtils
    {
        public static string MakeFullName(string ns, string name)
        {
            if (string.IsNullOrEmpty(ns)) return name;
            return ns + "." + name;
        }

        public static string GetShortName(string name)
        {
            int i = name.LastIndexOf('`');
            if (i >= 0) return name.Substring(0, i);
            return name;
        }
    }
}