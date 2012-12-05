using System;
using System.Collections.Generic;

interface I
{
}

class A
{
}

class B : A, I
{
}

internal class Test
{
    static IEnumerable<T> As<T>(object obj)
    {
        return obj as IEnumerable<T>;
    }

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        Console.WriteLine(As<int>(new int[0]) != null);
        Console.WriteLine(As<char>(new int[0]) != null);
        Console.WriteLine(As<char>("Hello") != null);
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        Console.WriteLine(As<object>(new int[0]) != null);
        Console.WriteLine(As<object>(new string[0]) != null);
    }

    static void Test3()
    {
        Console.WriteLine("--- Test3");
        Console.WriteLine(As<object>(new A[0]) != null);
        Console.WriteLine(As<A>(new B[0]) != null);
        Console.WriteLine(As<I>(new B[0]) != null);
    }

    static void Test4()
    {
        Console.WriteLine("--- Test4");
        Console.WriteLine(As<uint>(new int[0]) != null);
    }

    static void print<T>(IEnumerable<T> set)
    {
        foreach (T item in set)
            Console.WriteLine(item);
    }

    static void Test5()
    {
        Console.WriteLine("--- Test5");
        print(new char[] { 'a', 'b', 'c' });
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Test4();
        Test5();
        Console.WriteLine("<%END%>");
    }
}