using System;
using System.Collections;
using System.Collections.Generic;

class X
{
    static void Print<T>(IEnumerable<T> set, Predicate<T> f)
    {
        foreach (T item in set)
        {
            if (f(item))
            {
                Console.WriteLine(item);
            }
        }
    }

    static void Test1()
    {
        Console.WriteLine("--Test1");
        int[] arr = new[] { -30, -20, -10, 0, 10, 20, 30 };
        Print(arr, delegate(int v) { return v > 0; });
        Print(arr, delegate(int v) { return v >= 0; });
        Print(arr, delegate(int v) { return v < 0; });
        Print(arr, delegate(int v) { return v <= 0; });
    }

    public static void  Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}