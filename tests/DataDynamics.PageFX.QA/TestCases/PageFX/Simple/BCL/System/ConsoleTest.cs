using System;

class X
{
    static void Test1()
    {
        Console.WriteLine("Hello, World!");
        string s = null;
        Console.WriteLine(s);
    }

    static void Test2()
    {
        Console.WriteLine(10);
    }

    static void Test3()
    {
        string s = "";
        if (s == null)
        {
            Console.WriteLine("error");
        }
        else
        {
            Console.WriteLine("ok");
        }
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Console.WriteLine("<%END%>");
    }
}