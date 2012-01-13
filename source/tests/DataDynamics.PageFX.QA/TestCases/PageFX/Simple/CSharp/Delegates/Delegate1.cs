using System;

delegate void Func();

class Delegate1
{
    public static void f1()
    {
        Console.WriteLine("f1");
    }

    public static void f2()
    {
        Console.WriteLine("f2");
    }

    public static void  Main()
    {
        Func a = f1;
        Func b = f2;
        a();
        b();
        Console.WriteLine("<%END%>");
    }
}