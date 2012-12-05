using System;

class IgnoreReturn
{
    static int f1()
    {
        Console.WriteLine("f1");
        return 1;
    }

    static int f2()
    {
        Console.WriteLine("f2");
        return 2;
    }

    static void Main()
    {
        f1();
        f2();
        Console.WriteLine(f1());
        Console.WriteLine(f2());
        Console.WriteLine("<%END%>");
    }
}