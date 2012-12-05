using System;
using System.Collections.Generic;

class BoundsChecking
{
    static void GetValue(Array arr, int index)
    {
        try
        {
            object v = arr.GetValue(index);
            Console.WriteLine(v);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void Get<T>(T[] arr, int index)
    {
        try
        {
            T v = arr[index];
            Console.WriteLine(v);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        int[] arr = new int[] { 0, 1, 2 };
        Get<int>(arr, 0);
        Get<int>(arr, -1);
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        int[] arr = new int[] { 0, 1, 2 };
        GetValue(arr, 0);
        GetValue(arr, -1);
    }

    static void Test3()
    {
        Console.WriteLine("--- Test3");
        string[] arr = new string[] { null, "", null };
        Console.WriteLine(arr[0]);
        Get<string>(arr, 0);
        Get<string>(arr, -1);
        GetValue(arr, 0);
        GetValue(arr, -1);
    }

    static void GetItem<T>(IList<T> list, int index)
    {
        try
        {
            T v = list[index];
            Console.WriteLine(v);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void Test4()
    {
        Console.WriteLine("--- Test4");
        int[] arr = new int[] { 0, 1, 2 };
        GetItem<int>(arr, 0);
        GetItem<int>(arr, -1);
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Test4();
        Console.WriteLine("<%END%>");
    }
}
