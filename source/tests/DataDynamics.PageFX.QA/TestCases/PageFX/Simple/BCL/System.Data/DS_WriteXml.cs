using System;
using System.Data;
using System.IO;

class X
{
    static void Main()
    {
        string xml = "<set><table1><col1>sample text</col1><col2/></table1><table2 attr='value'><col3>sample text 2</col3></table2></set>";
        DataSet ds = new DataSet();
        ds.ReadXml(new StringReader(xml));
        StringWriter writer = new StringWriter();
        ds.WriteXml(writer);
        Console.WriteLine(writer.ToString());
        Console.WriteLine("<%END%>");
    }
}