using System;

class Array6
{
    static void PrintArray(int[] arr)
    {
        for (int i = 0; i < arr.Length; ++i)
        {
            Console.WriteLine(arr[i]);
        }
    }

    static void Main()
    {
        PrintArray(new int[] { 10, 20, 30 });
        PrintArray(new int[] { 10, 20, 30 });
        Console.WriteLine("<%END%>");
    }
}