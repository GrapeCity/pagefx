using System.Xml;
using DataDynamics.PageFX.CLI.Metadata;

namespace DataDynamics.PageFX.CLI.Tools
{
    public static class MetadataXmlExport
    {
        public static void Export(string inputPath, string outputPath)
        {
            var xws = new XmlWriterSettings {Indent = true, IndentChars = "  "};
            using (var reader = new MetadataReader(inputPath))
            using (var writer = XmlWriter.Create(outputPath, xws))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("mdb");

                for (int i = 0; i < MetadataReader.MaxTableNum; ++i)
                {
                    var id = (TableId)i;
                    var table = reader[id];
                    if (table != null)
                    {
                        writer.WriteStartElement("table");
                        writer.WriteAttributeString("name", table.Name);
                        for (int k = 0; k < table.RowCount; ++k)
                        {
                            var row = reader.GetRow(id, k);
                            writer.WriteStartElement("row");
                            writer.WriteAttributeString("id", table.Name + k);
                            foreach (var cell in row.Cells)
                            {
                                writer.WriteStartElement("cell");
                                writer.WriteAttributeString("name", cell.Name);
                                writer.WriteAttributeString("value", cell.ValueString);
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}