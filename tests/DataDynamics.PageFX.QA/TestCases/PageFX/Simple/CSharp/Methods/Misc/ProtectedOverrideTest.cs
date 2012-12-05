using System;

class XNode
{
    protected virtual string XmlLang
    {
        get
        {
            return "en";
        }
    }

    public string Lang
    {
        get { return XmlLang; }
    }
}

class XDoc : XNode
{
    protected override string XmlLang
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
        Console.WriteLine(doc.Lang);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine();
        Console.WriteLine("<%END%>");
    }
}