using System;

class Test1
{
    static void f(int a)
    {
        Console.WriteLine(a);
    }

    static void f(int a, int b)
    {
        Console.WriteLine(a);
        Console.WriteLine(b);
    }

    static void Main()
    {
        f(0);
        f(0, 1);
        Console.WriteLine("<%END%>");
    }
}