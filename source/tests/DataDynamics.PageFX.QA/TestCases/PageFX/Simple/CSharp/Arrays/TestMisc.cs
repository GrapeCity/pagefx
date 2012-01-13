using System;

struct Slot
{
    public Object Key;
    public Object Value;
}

class Program
{
    static void TestCtors()
    {
        {
            int[,] arr = new int[3, 3];
            Console.WriteLine(arr[0, 0]);
            arr[0, 0] = 10;
            Console.WriteLine(arr[0, 0]);
            Console.WriteLine(arr.Rank);
        }
    }

    static void TestRank()
    {
        {
            int[] a1 = { 10, 20, 30 };
            Console.WriteLine(a1.Rank);
        }

        {
            int[,] a2 = new int[3, 3];
            Console.WriteLine(a2.Rank);
        }

        {
            int[, ,] a3 = new int[3, 3, 3];
            Console.WriteLine(a3.Rank);
        }

        {
            char[] a1 = { 'a', 'b', 'c' };
            Console.WriteLine(a1.Rank);
        }
    }

    static void TestBounds()
    {
        {
            int[,] arr = new int[3, 3];
            Console.WriteLine(arr.GetLowerBound(0));
            Console.WriteLine(arr.GetLowerBound(1));
            Console.WriteLine(arr.GetUpperBound(0));
            Console.WriteLine(arr.GetUpperBound(1));
        }
    }

    static void Test1()
    {
        int[] arr = { 10, 20, 30 };
        Console.WriteLine(arr[0] is IComparable);
    }

    static void TestBinarySearch()
    {
        int[] arr = { 10, 20, 30 };
        Console.WriteLine(Array.BinarySearch(arr, 20));
    }

    static void TestGetValue()
    {
        int[] arr = { 10, 20, 30 };
        Console.WriteLine(arr.GetValue(0) is IComparable);
    }

    static void TestCopy()
    {
        Slot[] arr1 = new Slot[3];
    }

    static void Main()
    {
        TestCtors();
        TestRank();
        TestBounds();
        Test1();
        TestBinarySearch();
        TestGetValue();
        Console.WriteLine("<%END%>");
    }
}