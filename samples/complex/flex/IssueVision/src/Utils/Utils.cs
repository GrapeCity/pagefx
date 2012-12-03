using System;
using System.Data;
using System.IO;

namespace IssueVision
{
    internal class Utils
    {
        public static string ToXmlString(object result)
        {
            if (result == null)
                return "";

            Avm.XMLList list = result as Avm.XMLList;
            if (list != null)
                return list.toXMLString();

            Avm.XML xml = result as Avm.XML;
            if (xml != null)
                return xml.toXMLString();

            return "";
        }

        public static void DumpDataSet(TextWriter writer, DataSet data)
        {
            writer.WriteLine(data.DataSetName);
            foreach (DataTable table in data.Tables)
            {
                writer.WriteLine("--- Table {0} ---", table.TableName);
                int n = table.Columns.Count;
                for (int i = 0; i < n; ++i)
                {
                    if (i > 0) writer.Write(" | ");
                    writer.Write(table.Columns[i].ColumnName);
                }
                writer.WriteLine();
                foreach (DataRow row in table.Rows)
                {
                    for (int i = 0; i < n; ++i)
                    {
                        if (i > 0) writer.Write(" | ");
                        object v = row[table.Columns[i]];
                        //if (v is DateTime)
                        //{
                        //    string vs = ((DateTime)v).ToString("dd-MM-yy");
                        //    writer.Write(vs);
                        //}
                        //else
                        //{
                        //    writer.Write(v);
                        //}
                        writer.Write(v);
                    }
                    writer.WriteLine();
                }
            }
        }

        public static string ToString(DataSet data)
        {
            StringWriter writer = new StringWriter();
            DumpDataSet(writer, data);
            return writer.ToString();
        }
    }
}