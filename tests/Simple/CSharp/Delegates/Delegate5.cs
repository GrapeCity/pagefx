using System;

delegate void Func();

class Delegate5
{
    public static void f1()
    {
        Console.WriteLine("f1");
    }

    public static void f2()
    {
        Console.WriteLine("f2");
    }

    static void Main()
    {
        Func f = f1;
        f += f2;
        f();
        f -= f1;
        f();
        Console.WriteLine("<%END%>");
    }
}