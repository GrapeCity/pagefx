using System;

class TestGetValue
{
    static void Test1()
    {
        int[] arr = { 10, 20, 30 };
        //object obj = arr[0];
        //Console.WriteLine(obj is IComparable);
        Console.WriteLine(arr.GetValue(0) is IComparable);
    }

    static void Test2()
    {
        int[,] arr = { { 10, 20, 30 }, { 40, 50, 60 } };
        Console.WriteLine(arr.GetLength(0));
        Console.WriteLine(arr.GetLength(1));
        Console.WriteLine(arr.GetLowerBound(0));
        Console.WriteLine(arr.GetLowerBound(1));

        int[] i = { 1, 1 };
        Console.WriteLine(arr.GetValue(i));
    }

    static void Test3()
    {
        int[, ,] arr = { { { 1, 2, 3 }, { 4, 5, 6 } }, { { 7, 8, 10 }, { 11, 12, 13 } } };
        int[] i = { 1, 1, 1 };
        Console.WriteLine(arr.GetValue(i));
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Console.WriteLine("<%END%>");
    }
}