using System;
using System.IO;
using System.Xml;
using System.Text;

class X
{
    static void Test1()
    {
        StringWriter sw = new StringWriter();
        XmlTextWriter writer = new XmlTextWriter(sw);
        writer.QuoteChar = '\'';
    }

    static void Test2()
    {
        MemoryStream ms = new MemoryStream();
        XmlTextWriter xtw = new XmlTextWriter(ms, new UnicodeEncoding());
    }

    static void Main()
    {
        //Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}