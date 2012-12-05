using System;

class XNode
{
    internal virtual string XmlLang
    {
        get
        {
            return "en";
        }
    }
}

class XDoc : XNode
{
    internal override string XmlLang
    {
        get
        {
            return String.Empty;
        }
    }
}

class X
{
    static void Test1()
    {
        XDoc doc = new XDoc();
        Console.WriteLine(doc.XmlLang);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine();
        Console.WriteLine("<%END%>");
    }
}