using System;

class X
{
    static void Test1()
    {
        Console.WriteLine("---Test1");
        string s = string.Format("{0} {1} {2}", 1, 2, 3);
        Console.WriteLine(s);
    }

    static void Test2()
    {
        Console.WriteLine("---Test2");
        double n1 = 3.14;
        string s = string.Format("{0}", n1);
        Console.WriteLine(s);
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}