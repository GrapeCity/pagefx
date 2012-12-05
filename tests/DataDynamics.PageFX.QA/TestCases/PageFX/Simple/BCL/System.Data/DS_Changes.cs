using System;
using System.Data;
using System.IO;

class X
{
    static int N = 0;

    static void Print(DataSet ds)
    {
        ++N;
        Console.WriteLine(N + ". " + ds.DataSetName);
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

    static void PrintChanges(DataSet ds)
    {
        DataSet changes = ds.GetChanges();
        if (changes != null)
        {
            Console.WriteLine("--- changes ---");
            Print(changes);
        }
        else
        {
            Console.WriteLine("--- no changes ---");
        }
    }

    static void Main()
    {
        string xml = @"<set>
<table>
  <name>A</name>
  <value>1</value>
</table>
<table>
  <name>B</name>
  <value>2</value>
</table>
<table>
  <name>C</name>
  <value>3</value>
</table>
</set>";
        DataSet ds = new DataSet();
        ds.ReadXml(new StringReader(xml));
        Print(ds);

        ds.AcceptChanges();
        Change(ds, 2);
        PrintChanges(ds);

        ds.RejectChanges();
        Print(ds);

        Change(ds, 3);
        ds.AcceptChanges();
        ds.RejectChanges();
        Print(ds);
        PrintChanges(ds);
        Console.WriteLine("<%END%>");
    }

    static void Change(DataSet ds, int n)
    {
        DataTable table = ds.Tables["table"];

        table.Rows[0][0] = new string('A', n);
        table.Rows[1][0] = new string('B', n);
    }
}