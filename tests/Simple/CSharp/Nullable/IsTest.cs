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
        IsNullable(true);
        IsNullable(new bool?(true));
        IsNullable(0);
        IsNullable(new int?(0));
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        Is(true);
        Is(new bool?(true));
        Is(0);
    }

    static void Foo<T>(T? v) where T : struct
    {
        Console.WriteLine(v is bool);
        Console.WriteLine(v is int);
        Console.WriteLine(v is long);
        Console.WriteLine(v is string);
        Console.WriteLine(v is object);
        Console.WriteLine(v is bool?);
        Console.WriteLine(v is int?);
        Console.WriteLine(v is long?);
    }

    static void Test3()
    {
        Console.WriteLine("--- Test3");
        Console.WriteLine("-- Foo<int>");
        Foo<int>(null);
        Console.WriteLine("-- Foo<bool>");
        Foo<bool>(null);
        Console.WriteLine("-- Foo<long>");
        Foo<long>(null);
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Console.WriteLine("<%END%>");
    }
}