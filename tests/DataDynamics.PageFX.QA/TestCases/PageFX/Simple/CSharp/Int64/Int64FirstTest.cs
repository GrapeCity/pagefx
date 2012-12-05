using System;

class Program
{
    static void Test1()
    {
	    long v = -11L;
        Console.WriteLine(v);
    }

    static void Test2()
    {
        ulong v = 11UL;
        Console.WriteLine(v);
    }

    static void Test3()
    {
        long v = 45643271;
        Console.WriteLine((ulong)v);
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Console.WriteLine("<%END%>");
    }
}