using System.IO;
using System.Xml;

namespace DataDynamics.PageFX.CLI.Metadata
{
    public static class MdbHtmlExport
    {
        private static void WriteStyle(string outputDir)
        {
            string path = Path.Combine(outputDir, "style.css");
            if (!File.Exists(path))
            {
                using (var writer = new StreamWriter(path))
                {
                    writer.WriteLine("td, th { border: 1 solid black; }");
                }
            }
        }

        private static void ExportTable(MdbReader reader, MdbTable table, string outdir)
        {
            var xws = new XmlWriterSettings();
            xws.Indent = true;
            xws.IndentChars = "  ";
            string path = Path.Combine(outdir, table.Name + ".htm");
            using (var writer = XmlWriter.Create(path, xws))
            {
                writer.WriteStartDocument();
                //<?xml-stylesheet href="style.css" rel="stylesheet" type="text/css"?>
                //writer.WriteProcessingInstruction("xml-stylesheet", "href=\"style.css\" rel=\"stylesheet\" type=\"text/css\"");
                writer.WriteStartElement("html");

                writer.WriteStartElement("head");

                WriteStyle(outdir);

                writer.WriteStartElement("style");
                writer.WriteAttributeString("type", "text/css");
                writer.WriteAttributeString("href", "style.css");
                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteStartElement("body");

                writer.WriteStartElement("h1");
                writer.WriteAttributeString("id", table.Name);
                writer.WriteString(table.Name + " Table");
                writer.WriteEndElement();

                writer.WriteStartElement("table");
                for (int k = 0; k < table.RowCount; ++k)
                {
                    var row = reader.GetRow(table.Id, k);
                    if (k == 0)
                    {
                        writer.WriteStartElement("tr");
                        foreach (var cell in row.Cells)
                        {
                            writer.WriteStartElement("td");
                            writer.WriteString(cell.Name);
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }

                    writer.WriteStartElement("tr");
                    writer.WriteAttributeString("id", "row" + k);
                    foreach (var cell in row.Cells)
                    {
                        writer.WriteStartElement("td");

                        switch (cell.Column.Type)
                        {
                            case MdbColumnType.SimpleIndex:
                            case MdbColumnType.CodedIndex:
                                {
                                    writer.WriteStartElement("a");
                                    MdbIndex idx = cell.Value;
                                    writer.WriteAttributeString("href", idx.Table + ".htm#row" + idx.Index);
                                    writer.WriteString(idx.Table + "[" + idx.Index + "]");
                                    writer.WriteEndElement();
                                }
                                break;

                            default:
                                writer.WriteString(cell.ValueString);
                                break;
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement(); //table
                writer.WriteEndElement(); //body
                writer.WriteEndElement(); //html
                writer.WriteEndDocument();
            }
        }

        public static void Export(string path, string outdir)
        {
            using (var reader = new MdbReader(path))
            {
                Directory.CreateDirectory(outdir);
                for (int i = 0; i < MdbReader.MaxTableNum; ++i)
                {
                    var id = (MdbTableId)i;
                    var table = reader[id];
                    if (table != null)
                    {
                        ExportTable(reader, table, outdir);
                    }
                }
            }
        }
    }
}