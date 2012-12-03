using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.Tools
{
    class Program
    {
        static CommandLine cl;

        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Usage();
                return 1;
            }

            cl = CommandLine.Parse(args);
            if (cl == null)
            {
                Console.WriteLine("error PWX0001: Invalid command line");
                return -1;
            }

            var inputs = cl.GetInputFiles();
            if (inputs.Length == 0)
            {
                Console.WriteLine("error PWX0002: No input xml files");
                return -1;
            }

            var prefix = cl.GetOption(null, "prefix");
            if (inputs.Length > 1)
                prefix = null;

            using (components = new StreamWriter("components.wxi"))
            {
                components.WriteLine("<Include>");
                foreach (var input in inputs)
                    RenameComponents(input, prefix);
                components.WriteLine("</Include>");
            }

            return 0;
        }

        static TextWriter components;

        static void RenameComponents(string path, string prefix)
        {
            try
            {
                if (Path.IsPathRooted(path))
                    path = Path.Combine(Environment.CurrentDirectory, path);

                string ext = cl.GetOption("", "ext");
                string outpath = path + ".2.xml";
                if (!string.IsNullOrEmpty(ext))
                {
                    ext = ext.TrimStart('.');
                    if (!string.IsNullOrEmpty(ext))
                    {
                        outpath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + "." + ext);
                    }
                }

                string xml = File.ReadAllText(path);

                if (string.IsNullOrEmpty(prefix))
                {
                    prefix = Path.GetFileNameWithoutExtension(path);
                }

                var xws = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    OmitXmlDeclaration = true
                };

                using (var reader = XmlReader.Create(new StringReader(xml)))
                using (var writer = XmlWriter.Create(outpath, xws))
                {
                    RenameComponents(reader, writer, prefix);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Environment.Exit(-1);
            }
        }
        
        static void RenameComponents(XmlReader reader, XmlWriter writer, string prefix)
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            writer.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
                            RenameComponent(reader, writer, prefix);
                            if (reader.IsEmptyElement)
                                writer.WriteEndElement();
                        }
                        break;

                    case XmlNodeType.Text:
                        writer.WriteString(reader.Value);
                        break;

                    case XmlNodeType.CDATA:
                        writer.WriteCData(reader.Value);
                        break;

                    case XmlNodeType.ProcessingInstruction:
                        writer.WriteProcessingInstruction(reader.Name, reader.Value);
                        break;

                    case XmlNodeType.Comment:
                        writer.WriteComment(reader.Value);
                        break;

                    case XmlNodeType.XmlDeclaration:
                        //Console.WriteLine("<?xml version=\"1.0\"?>");
                        break;

                    case XmlNodeType.Document:
                        break;

                    case XmlNodeType.DocumentType:
                        //Console.WriteLine("<!DOCTYPE {0} [{1}]", reader.Name, reader.Value);
                        break;

                    case XmlNodeType.EntityReference:
                        writer.WriteEntityRef(reader.Name);
                        break;

                    case XmlNodeType.EndElement:
                        writer.WriteEndElement();
                        break;
                }
            }
        }

        static void RenameComponent(XmlReader reader, XmlWriter writer, string prefix)
        {
            if (!reader.HasAttributes) return;

            string elemName = reader.Name;
            string elemNS = reader.NamespaceURI;
            string id = null;
            if (IsComponent(elemName, elemNS) || IsFile(elemName, elemNS) || IsDirectory(elemName, elemNS))
            {
                while (reader.MoveToNextAttribute())
                {
                    if (reader.Name == Attr_ID)
                    {
                        id = prefix + reader.Value;
                        if (IsComponent(elemName, elemNS))
                        {
                            components.WriteLine("<ComponentRef Id=\"" + id + "\" />");
                        }
                        writer.WriteAttributeString(Attr_ID, id);
                    }
                    else
                    {
                        WriteAttribute(reader, writer);
                    }
                }
            }
            else
            {
                while (reader.MoveToNextAttribute())
                {
                    WriteAttribute(reader, writer);
                }
            }
            // Move the reader back to the element node.
            reader.MoveToElement();
        }

        static void WriteAttribute(XmlReader reader, XmlWriter writer)
        {
            writer.WriteAttributeString(reader.Prefix, reader.LocalName, reader.NamespaceURI, reader.Value);
            //writer.WriteStartAttribute(reader.Prefix, reader.LocalName);
            //writer.WriteValue(reader.Value);
            //writer.WriteEndAttribute();
        }

        const string XmlnsWix = "http://schemas.microsoft.com/wix/2003/01/wi";
        const string Attr_ID = "Id";

        static bool IsComponent(string name, string ns)
        {
            return name == "Component" && ns == XmlnsWix;
        }

        static bool IsFile(string name, string ns)
        {
            return name == "File" && ns == XmlnsWix;
        }

        static bool IsDirectory(string name, string ns)
        {
            return name == "Directory" && ns == XmlnsWix;
        }

        static void Usage()
        {
        }
    }
}
