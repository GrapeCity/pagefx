using System;

internal class Test
{
    static void IsNullable(object obj)
    {
        Console.WriteLine(obj is bool?);
        Console.WriteLine(obj is int?);
        Console.WriteLine(obj is long?);
    }

    static void Is(object obj)
    {
        Console.WriteLine(obj is bool);
        Console.WriteLine(obj is int);
        Console.WriteLine(obj is long);
    }

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        IsNullable(null);
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        Is(null);
    }

    static object Foo<T>(T? v) where T : struct
    {
        return (object)v;
    }

    static void Test3()
    {
        Console.WriteLine("--- Test3");
        Console.WriteLine(Foo<int>(null) == null);
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Console.WriteLine("<%END%>");
    }
}