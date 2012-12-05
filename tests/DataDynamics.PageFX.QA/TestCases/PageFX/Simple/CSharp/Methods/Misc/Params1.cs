using System;

class ParamsTest1
{
    static void f(params int[] args)
    {
        int n = args.Length;
        for (int i = 0; i < n; ++i)
            Console.WriteLine(args[i]);
    }

    static void Main()
    {
        f(1, 2, 3);
        Console.WriteLine("<%END%>");
    }
}