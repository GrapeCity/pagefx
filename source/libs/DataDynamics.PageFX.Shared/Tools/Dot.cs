using System.IO;
using Ionic.Zip;

namespace DataDynamics
{
    public static class Dot
    {
        private static string GetPath()
        {
            const string dir = "c:\\DevTools\\dot";
            string path = Path.Combine(dir, "dot.exe");
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(dir);
                var asm = typeof(Dot).Assembly;
                var rs = asm.GetResourceStream("Tools.dot.zip");
                var zip = ZipFile.Read(rs);
                foreach (var entry in zip)
                {
                    var ms = entry.OpenReader().ToMemoryStream();
                    ms.Save(Path.Combine(dir, entry.FileName));
                }
            }
            return path;
        }

        public static void Render(string dotFile, string pngPath)
        {
            int exitCode;
            if (string.IsNullOrEmpty(pngPath))
                pngPath = Path.ChangeExtension(dotFile, ".png");
            string args = string.Format("-Tpng {0} -o {1}", dotFile, pngPath);
            string cmd = GetPath();
            CommandPromt.Run(cmd, args, out exitCode);
        }

        public static void RenderDirectory(string dir, string pngSubdir)
        {
            string pngDir = dir;
            if (!string.IsNullOrEmpty(pngSubdir))
                pngDir = Path.Combine(dir, pngSubdir);
            Directory.CreateDirectory(pngDir);
            foreach (var dotFile in Directory.GetFiles(dir, "*.dot"))
            {
                string png = Path.Combine(pngDir, Path.GetFileNameWithoutExtension(dotFile) + ".png");
                Render(dotFile, png);
            }
        }
    }
}