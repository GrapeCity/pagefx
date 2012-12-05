using System;

class Test2
{
    static void f(int a)
    {
        if ((a & 1) != 0)
            Console.WriteLine("odd");
        else
            Console.WriteLine("even");
    }

    static void Main()
    {
        f(0);
        f(1);
        Console.WriteLine("<%END%>");
    }
}