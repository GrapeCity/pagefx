using System;

class X
{
    static void print(string[] arr)
    {
        for (int i = 0; i < arr.Length; ++i)
            Console.WriteLine(arr[i]);
    }

    static string[] ToStringArray(Array arr)
    {
        return (string[])arr;
    }

    static void Test1()
    {
        Console.WriteLine("object[] to string[]");
        try
        {
            object[] arr1 = new object[] { "a", "b", "c" };
            string[] arr2 = (string[])arr1;
            print(arr2);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void Test2()
    {
        Console.WriteLine("int[] to string[]");
        try
        {
            int[] arr1 = new int[] { 10, 20, 30 };
            string[] arr2 = ToStringArray(arr1);
            print(arr2);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}