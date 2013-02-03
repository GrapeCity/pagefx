using System.IO;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common.Services
{
    public static class ExportService
    {
        static string MakeValidPath(string path)
        {
            path = path.Replace('`', '!');
            path = path.Replace('<', '{');
            path = path.Replace('>', '}');
            return path;
        }

        public static bool CanWrite(IType type)
        {
            if (type == null) return false;
            while (type != null)
            {
                if (!CanWriteCore(type)) return false;
                type = type.DeclaringType;
            }
            return true;
        }

        public static bool CanWriteCore(IType type)
        {
            if (type.IsCompilerGenerated())
                return false;

            if (type.IsSpecialName)
                return false;

            if (type.Name == "<Module>")
                return false;

            return true;
        }

        public static void ToDirectory(ITypeContainer asm, string lang, string dir)
        {
            string format = string.Format("lang = {0}; mode = full", lang);

            Directory.CreateDirectory(dir);
            foreach (var type in asm.Types)
            {
                if (CanWrite(type))
                {
                    string text = type.ToString(format, null);

                    string typeDir = dir;
                    string ns = type.Namespace;
                    if (!string.IsNullOrEmpty(ns))
                    {
                        typeDir = Path.Combine(dir, ns);
                        Directory.CreateDirectory(typeDir);
                    }

                    string path = Path.Combine(typeDir, MakeValidPath(type.Name) + ".cs");
                    File.WriteAllText(path, text);
                }
            }
        }

        public static void ToFile(IAssembly asm, string lang, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                ToWriter(asm, lang, writer);
            }
        }

        public static void ToWriter(IAssembly asm, string lang, TextWriter writer)
        {
            string format = string.Format("lang = {0}; mode = full", lang);
            var swr = new SyntaxWriter(writer);
            swr.Init(format);
            swr.Write(asm);
        }

        public static string ToString(IAssembly asm, string lang)
        {
            using (var writer = new StringWriter())
            {
                ToWriter(asm, lang, writer);
                return writer.ToString();
            }
        }
    }
}