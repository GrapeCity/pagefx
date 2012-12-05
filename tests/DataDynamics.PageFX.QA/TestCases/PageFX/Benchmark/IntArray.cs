using System;

class Test
{
    static void Main()
    {
        int start = Environment.TickCount;
        int[] arr = new int[100];
        int n = 100000;
        for (int i = 0; i < n; ++i)
        {
            int j = i % arr.Length;
            ++arr[j];
        }
        int end = Environment.TickCount;
        Console.WriteLine(end - start);
        Console.WriteLine("<%END%>");
    }
}