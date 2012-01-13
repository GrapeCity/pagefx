using System;
using System.Globalization;
using System.IO;

namespace DataDynamics.PageFX.FLI
{
    class HtmlTemplate
    {
        public static void Deploy(SwfCompiler sfc)
        {
            //Console.WriteLine("Deploy HTML teplate");

            string outdir = sfc.OutputDirectory;
            if (string.IsNullOrEmpty(outdir))
            {
                //Console.WriteLine("No output directory");
                return;
            }

            try
            {
                BuildCore(sfc, outdir);
            }
            catch (Exception exc)
            {
                //TODO: Report Warning!!!
                Console.WriteLine("warning PFC9999: Unable to deply HTML template");
            }
        }

        static void BuildCore(SwfCompiler sfc, string outdir)
        {
            string src = GlobalSettings.Dirs.HtmlTemplates;
            if (!Directory.Exists(src))
            {
                //Console.WriteLine("No 'HTML Templates' directory");
                return;
            }

            string templateName = sfc.Options.HtmlTemplate;
            if (string.IsNullOrEmpty(templateName))
                templateName = PfxConfig.HTML.Template;

            src = Path.Combine(src, templateName);
            if (!Directory.Exists(src))
            {
                //Console.WriteLine("No {0} template", templateName);
                return;
            }

            Directory.CreateDirectory(outdir);
            CopyTo(src, outdir);

            string swfname = Path.GetFileNameWithoutExtension(sfc.OutputPath);
            string templatePath = Path.Combine(outdir, "index.template.html");
            string htmlPath = Path.Combine(outdir, swfname + ".html");
            File.Copy(templatePath, htmlPath, true);
            File.Delete(templatePath);

            string text = File.ReadAllText(htmlPath);

            string flashInstallSwf = Path.Combine(outdir, PlayerProductInstallSwf);
            bool hasInstallator = File.Exists(flashInstallSwf);
            
            text = ReplaceVars(text, sfc, hasInstallator);
            File.WriteAllText(htmlPath, text);
        }

        const string PlayerProductInstallSwf = "playerProductInstall.swf";

        static string ReplaceVars(string template, SwfCompiler sfc, bool hasInstallator)
        {
            return Str.ReplaceVars(
                template, RVScheme.Ant,
                "title", sfc.Title,
                "application", sfc.Application,
                "version_major", sfc.PlayerVersion.ToString(),
                "version_minor", "0",
                "version_revision", "0",
                //"expressInstallSwf", (hasInstallator ? PlayerProductInstallSwf : ""),
                "expressInstallSwf", "",
                "swf", Path.GetFileNameWithoutExtension(sfc.OutputPath),
                "width", sfc.Width.ToString(CultureInfo.InvariantCulture),
                "height", sfc.Height.ToString(CultureInfo.InvariantCulture),
                "bgcolor", sfc.HexBgColor);
        }

        static void CopyTo(string srcDir, string destDir)
        {
            foreach (var file in Directory.GetFiles(srcDir))
            {
                string file2 = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, file2, true);
            }
            foreach (var dir in Directory.GetDirectories(srcDir))
            {
                string dir2 = Path.Combine(destDir, Path.GetFileName(dir));
                Directory.CreateDirectory(dir2);
                CopyTo(dir, dir2);
            }
        }
    }
}