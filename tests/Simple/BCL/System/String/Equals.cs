using System;

internal class Test
{
    static void Eq(object x, object y)
    {
        Console.WriteLine(Equals(x, y));
    }

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        Eq("aaa", "bbb");
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}