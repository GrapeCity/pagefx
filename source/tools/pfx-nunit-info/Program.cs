using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using DataDynamics.PageFX.Common.NUnit;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.Core;

namespace DataDynamics.PageFX.Tools
{
    class Program
    {
        static void Usage()
        {
        }

        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Usage();
                return 1;
            }

            var cl = CommandLine.Parse(args);
            if (cl == null)
            {
                Console.WriteLine("error: Invalid command line");
                return -1;
            }

            var inputs = cl.GetInputFiles();
            if (inputs.Length == 0)
            {
                Console.WriteLine("error: No input files");
                return -1;
            }

            var assembly = LoadAssembly(inputs[0]);

            try
            {
                string output = cl.GetOutputFile();
                var cout = Console.Out;
                if (!string.IsNullOrEmpty(output))
                    cout = new StreamWriter(output);

                using (cout)
                {
                    WriteReport(cout, assembly);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
            return 0;
        }

        private static void WriteReport(TextWriter writer, IAssembly assembly)
        {
            using (var xmlWriter = new XmlTextWriter(writer))
            {
                xmlWriter.Formatting = Formatting.Indented;
                WriteReport(xmlWriter, assembly);
            }
        }

        private static void WriteReport(XmlWriter writer, IAssembly assembly)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("assembly");
            writer.WriteAttributeString("name", assembly.Name);
            writer.WriteAttributeString("version", assembly.Version.ToString());
			WriteTestFixtures(writer, assembly.GetTestFixtures());
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        private static void WriteTestFixtures(XmlWriter writer, IEnumerable<IType> set)
        {
            bool parent = false;
            foreach (var type in set)
            {
                if (!parent)
                {
                    writer.WriteStartElement("test-suites");
                    parent = true;
                }
                WriteTestFixture(writer, type);
            }
            if (parent)
                writer.WriteEndElement();
        }

        private static void WriteTestFixture(XmlWriter writer, IType type)
        {
            writer.WriteStartElement("test-suite");
            writer.WriteAttributeString("name", type.FullName);

			foreach (var method in type.GetUnitTests())
                WriteTest(writer, method);

            writer.WriteEndElement();
        }

        private static void WriteTest(XmlWriter writer, IMethod method)
        {
            var body = method.Body;
            if (body == null) return;

            var calls = body.GetCalls();
            if (calls == null) return;
            if (calls.Length <= 0) return;

            writer.WriteStartElement("test");
            writer.WriteAttributeString("name", method.ApiName());

            WriteCalls(writer, calls);

            writer.WriteEndElement();
        }

        private static bool FilterCall(IMethod call)
        {
            IType type = call.DeclaringType;
            string ns = type.Namespace;
            if (ns.StartsWith("NUnit")) return false;
            return true;
        }

        private static void WriteCalls(XmlWriter writer, IEnumerable<IMethod> calls)
        {
            bool parent = false;
            foreach (var call in calls)
            {
                if (!FilterCall(call)) continue;
                if (!parent)
                {
                    writer.WriteStartElement("calls");        
                    parent = true;
                }
                writer.WriteStartElement("call");
                writer.WriteAttributeString("type", call.DeclaringType.FullName);
                writer.WriteAttributeString("name", call.ApiName());
                writer.WriteEndElement();
            }
            if (parent)
                writer.WriteEndElement();
        }

        static IAssembly LoadAssembly(string path)
        {
            try
            {
                CommonLanguageInfrastructure.SubstituteFrameworkAssemblies = false;
                if (!Path.IsPathRooted(path))
                    path = Path.Combine(Environment.CurrentDirectory, path);
				return CommonLanguageInfrastructure.Deserialize(path, null);
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
