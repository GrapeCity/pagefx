using System;
using System.Collections.Generic;

internal class Test
{
    static void Print<T>(IEnumerator<T> e)
    {
        while (e.MoveNext())
            Console.WriteLine(e.Current);
    }

    static void PrintEnumerable<T>(object obj)
    {
        IEnumerable<T> e = obj as IEnumerable<T>;
        if (e != null)
        {
            Print(e.GetEnumerator());
        }
        else
        {
            Console.WriteLine("null");
        }
    }

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        PrintEnumerable<char>("Hello!");
        PrintEnumerable<int>("aaa");
        PrintEnumerable<int>(new int[] {10, 20, 30});
        PrintEnumerable<long>(new long[] { 10, 20, 30 });
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        PrintEnumerable<char>(null);
        PrintEnumerable<int>(null);
        PrintEnumerable<int>(new object());
        PrintEnumerable<long>(new object());
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}