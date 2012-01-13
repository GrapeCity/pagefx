using System;

class X
{
    static void f(int a)
    {
        Console.WriteLine(a);
        a = a + 1;
        Console.WriteLine(a);
    }

    static void Main()
    {
        int a = 10;
        f(a);
        f(a);
        Console.WriteLine("<%END%>");
    }
}