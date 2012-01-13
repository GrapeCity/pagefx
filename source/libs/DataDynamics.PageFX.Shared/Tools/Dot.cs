using System.IO;
using DataDynamics.Compression.Zip;

namespace DataDynamics
{
    public static class Dot
    {
        private static string GetPath()
        {
            string dir = "c:\\DevTools\\dot";
            string path = Path.Combine(dir, "dot.exe");
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(dir);
                var asm = typeof(Dot).Assembly;
                var rs = ResourceHelper.GetStream(asm, "Tools.dot.zip");
                var zip = new ZipFile(rs);
                foreach (ZipEntry entry in zip)
                {
                    var ms = Stream2.ToMemoryStream(entry.Data);
                    Stream2.SaveStream(ms, Path.Combine(dir, entry.Name));
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