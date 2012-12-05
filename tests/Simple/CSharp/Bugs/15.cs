using System;

class A
{
    public int type;

    public string Type
    {
        get
        {
            switch (type)
            {
                case 0:
                    return "AAA";
                case 1:
                    return "BBB";
            }
            return "CCC";
        }
    }
}

class X
{
    static void Test1()
    {
        Console.WriteLine("--Test1");
        var a = new A();
        Console.WriteLine(a.Type);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}