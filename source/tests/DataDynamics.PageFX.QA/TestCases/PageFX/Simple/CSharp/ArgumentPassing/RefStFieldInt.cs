using System;

class X
{
    private static int N = 10;

    static void f(ref int a)
    {
        Console.WriteLine(a);
        a = a + 1;
        Console.WriteLine(a);
    }

    static void Main()
    {
        f(ref N);
        f(ref N);
        Console.WriteLine("<%END%>");
    }
}