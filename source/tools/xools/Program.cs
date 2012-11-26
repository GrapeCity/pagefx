using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using DataDynamics;

namespace xools
{
    internal class Program
    {
        static CommandLine cl;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Usage();
                return;
            }

            cl = CommandLine.Parse(args);
            if (cl == null)
            {
                Usage();
                return;
            }

            if (cl.HasOption("merge"))
            {
                try
                {
                    Merge();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Environment.Exit(-1);
                }
                return;
            }
        }

        static void Usage()
        {
        }

        static void Merge()
        {
            string outpath = Path.Combine(Environment.CurrentDirectory, "result.xml");
            outpath = cl.GetPath(outpath, "out", "output");

            string input = cl.GetPath(Environment.CurrentDirectory, "in", "input");
            string ext = cl.GetOption(null, "ext");
            if (string.IsNullOrEmpty(ext))
                ext = "xml";
            else
            {
                if (ext[0] == '.')
                    ext = ext.Substring(1);
            }

            var docs = new List<XmlDocument>();
            foreach (var docpath in Directory.GetFiles(input, "*." + ext))
            {
                try
                {
                    var doc = new XmlDocument();
                    doc.Load(docpath);
                    docs.Add(doc);
                }
                catch (XmlException exc)
                {
                    Console.WriteLine("{0} ({1})", docpath, exc.LineNumber);
                    throw;
                }
            }

            string xpath = cl.GetOption(null, "xpath");
            string root = cl.GetOption("root", "root", "root-name");

            var xws = new XmlWriterSettings { Indent = true, IndentChars = "  " };
            using (XmlWriter writer = XmlWriter.Create(outpath, xws))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement(root);

                foreach (var doc in docs)
                {
                    if (string.IsNullOrEmpty(xpath))
                    {
                        doc.DocumentElement.WriteTo(writer);
                    }
                    else
                    {
                        try
                        {
                            var sel = doc.SelectNodes(xpath);
                            foreach (XmlNode node in sel)
                                node.WriteTo(writer);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            Environment.Exit(-1);
                        }
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}
