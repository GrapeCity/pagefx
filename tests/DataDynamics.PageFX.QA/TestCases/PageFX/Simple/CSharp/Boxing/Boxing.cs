using System;

public struct CharX : IComparable
{
    public char c;

    public CharX(char c)
    {
        this.c = c;
    }

    public int CompareTo(object obj)
    {
        if (obj is CharX)
            return c.CompareTo(((CharX)obj).c);
        else
            return c.CompareTo(obj);
    }
}

class Boxing
{
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        int a = 10;
        object obj = a;
        Console.WriteLine(a);
        Console.WriteLine(obj);

        a = (int)obj;
        Console.WriteLine(a);
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        CharX x = new CharX('a');
        Console.WriteLine(x.CompareTo('a'));
        Console.WriteLine(x.CompareTo('b'));
    }

    static void Test3()
    {
        Console.WriteLine("--- Test3");
        double a = 10;
        object obj = a;
        Console.WriteLine(a);
        Console.WriteLine(obj);

        a = (double)obj;
        Console.WriteLine(a);
    }

    static void Test4()
    {
        Console.WriteLine("--- Test4");
        object o = 1;
        Console.WriteLine(o is bool);
    }

    //http://blogs.msdn.com/brada/archive/2005/04/07/406098.aspx
    static void Question1()
    {
        Console.WriteLine("Question1");
        int x = 1; byte y = 1;
        Console.WriteLine(x == y && !x.Equals(y));
    }

    static void Question2()
    {
        //https://code.datadynamics.com/default.asp?pgx=EV&ixBug=109162&=#311561
        //Console.WriteLine("Question2");
        //string x = "hello"; object y = string.Copy(x);
        //Console.WriteLine(x != y && x.Equals(y));
    }

    static void Question4()
    {
        Console.WriteLine("Question4");
        int x = 1; int y = 1;
        Console.WriteLine(x == y && (object)x != (object)y);
    }

    public static void Main()
    {
        Console.WriteLine("Hello!");
        Test1();
        Test2();
        Test3();
        Test4();
        Question1();
        Question2();
        Question4();
        Console.WriteLine("<%END%>");
    }
}