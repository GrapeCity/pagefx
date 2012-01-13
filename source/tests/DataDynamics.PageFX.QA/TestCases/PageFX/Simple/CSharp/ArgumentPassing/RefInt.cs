using System;

class X
{
    static void f(ref int a)
    {
        Console.WriteLine(a);
        a = a + 1;
        Console.WriteLine(a);
    }

    static void Main()
    {
        int a = 10;
        f(ref a);
        f(ref a);
        Console.WriteLine("<%END%>");
    }
}