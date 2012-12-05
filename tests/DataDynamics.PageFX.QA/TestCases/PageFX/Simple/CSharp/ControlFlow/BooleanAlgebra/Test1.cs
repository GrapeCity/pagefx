using System;

class Test1
{
    static void f1(bool a, bool b)
    {
        bool res = a && b;
        Console.WriteLine(res);
    }

    static void f2(bool a, bool b)
    {
        bool res = !a && b;
        Console.WriteLine(res);
    }

    static void f3(bool a, bool b)
    {
        bool res = !a && !b;
        Console.WriteLine(res);
    }

    static void f4(bool a, bool b)
    {
        bool res = a || b;
        Console.WriteLine(res);
    }

    static void f5(bool a, bool b)
    {
        bool res = !a || b;
        Console.WriteLine(res);
    }

    static void f6(bool a, bool b)
    {
        bool res = !a || !b;
        Console.WriteLine(res);
    }

    static void Main()
    {
        Console.WriteLine("f1 = a && b");
        f1(false, false);
        f1(false, true);
        f1(true, false);
        f1(true, true);

        Console.WriteLine("f2 = !a && b");
        f2(false, false);
        f2(false, true);
        f2(true, false);
        f2(true, true);

        Console.WriteLine("f3 = !a && !b");
        f3(false, false);
        f3(false, true);
        f3(true, false);
        f3(true, true);

        Console.WriteLine("f4 = a || b");
        f4(false, false);
        f4(false, true);
        f4(true, false);
        f4(true, true);

        Console.WriteLine("f5 = !a || b");
        f5(false, false);
        f5(false, true);
        f5(true, false);
        f5(true, true);

        Console.WriteLine("f6 = !a || !b");
        f6(false, false);
        f6(false, true);
        f6(true, false);
        f6(true, true);
        Console.WriteLine("<%END%>");
    }
}