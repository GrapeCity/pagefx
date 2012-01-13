using System;
using System.Data;
using System.IO;

class X
{
    static void Print(DataSet ds)
    {
        Console.WriteLine(ds.DataSetName);
        foreach (DataTable table in ds.Tables)
            PrintTable(table);
    }

    static void PrintTable(DataTable table)
    {
        Console.WriteLine("--- Table {0}", table.TableName);
        string str = "";
        int n = table.Columns.Count;
        for (int i = 0; i < n; ++i)
        {
            if (i > 0) str += " | ";
            str += table.Columns[i].ColumnName;
        }
        Console.WriteLine(str);
        foreach (DataRow row in table.Rows)
        {
            str = "";
            for (int i = 0; i < n; ++i)
            {
                if (i > 0) str += " | ";
                object v = row[table.Columns[i]];
                if (v is DateTime)
                {
                    string vs = ((DateTime)v).ToString("dd-MM-yy");
                    str += vs;
                }
                else
                {
                    str += v;
                }
            }
            Console.WriteLine(str);
        }
    }

    static void Test1()
    {
        string xml = "<set><table1><col1>sample text</col1><col2/></table1><table2 attr='value'><col3>sample text 2</col3></table2></set>";
        DataSet ds = new DataSet();
        ds.ReadXml(new StringReader(xml));
        Print(ds);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}