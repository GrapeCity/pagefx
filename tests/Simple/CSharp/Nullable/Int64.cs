using System;

internal class Test
{
    static object Foo<T>(T? v) where T : struct
    {
        return (object)v;
    }

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        Console.WriteLine(Foo<Int64>(0));
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}