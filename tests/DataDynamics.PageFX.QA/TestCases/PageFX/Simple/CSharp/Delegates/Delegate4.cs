using System;

delegate void Func(int value);

class Delegate4
{
    public static void f1(int value)
    {
        Console.WriteLine("f1");
        Console.WriteLine(value);
    }

    public static void f2(int value)
    {
        Console.WriteLine("f2");
        Console.WriteLine(value);
    }

    public static void Main()
    {
        Func a = f1;
        Func b = f2;
        a(0);
        b(1);
        Console.WriteLine("<%END%>");
    }
}