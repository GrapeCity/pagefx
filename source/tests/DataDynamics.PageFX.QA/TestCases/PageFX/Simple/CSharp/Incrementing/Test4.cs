using System;

class Test4
{
    private static int[] arr;

    static void f(int i)
    {
        Console.WriteLine(i);
    }

    static void Main()
    {
        arr = new int[10];
        for (int i = 0; i < 10; ++i)
        {
            f(arr[i]);
            f(arr[i]++);
            f(++arr[i]);
            f(arr[i]);
            f(arr[i]--);
            f(--arr[i]);
            f(arr[i]);
        }
        Console.WriteLine("<%END%>");
    }
}