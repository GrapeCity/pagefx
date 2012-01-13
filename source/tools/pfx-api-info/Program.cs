using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using DataDynamics.PageFX.CLI;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.Tools
{
    class Program
    {
        static CommandLine cl;

        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                //args = new[] { @"c:\Silverlight\mscorlib.dll" };
                //args = new[] { @"c:\Silverlight\System.Xml.dll" };
                Console.WriteLine("usage: pfx-api-info.exe <assemblies>");
                return 1;
            }

            cl = CommandLine.Parse(args);

            var assemblies = LoadAssemblies(cl.GetInputFiles());

            WriteXml(assemblies);
            return 0;
        }

        private static void WriteXml(IEnumerable<IAssembly> assemblies)
        {
            try
            {
                var cout = Console.Out;
                string output = cl.GetOutputFile();
                if (!string.IsNullOrEmpty(output))
                    cout = new StreamWriter(output);

                using (cout)
                using (var writer = new XmlTextWriter(cout))
                {
                    writer.Formatting = Formatting.Indented;
                    writer.WriteStartDocument();

                    writer.WriteStartElement("assemblies");

                    foreach (var assembly in assemblies)
                        ApiInfo.Write(writer, assembly);

                    writer.WriteEndElement();

                    writer.WriteEndDocument();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Environment.Exit(-1);
            }
        }

        static IAssembly[] LoadAssemblies(string[] files)
        {
            int n = files.Length;
            var arr = new IAssembly[n];
            for (int i = 0; i < n; ++i)
            {
                arr[i] = LoadAssembly(files[i]);
            }
            return arr;
        }

        static IAssembly LoadAssembly(string path)
        {
            try
            {
                Infrastructure.SubstituteFrameworkAssemblies = false;
                if (!Path.IsPathRooted(path))
                    path = Path.Combine(Environment.CurrentDirectory, path);
                return Infrastructure.Deserialize(path, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Environment.Exit(-1);
                return null;
            }
        }
    }
}
