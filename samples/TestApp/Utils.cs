using System;

class Utils
{
    public static void SayHello()
    {
        Console.WriteLine("Hello!");
    }

    public static int Sum(int[] arr)
    {
        if (arr == null) return 0;
        int sum = 0;
        int n = arr.Length;
        for (int i = 0; i < n; ++i)
            sum += arr[i];
        return sum;
    }
}