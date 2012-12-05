using System;

class Test3
{
    static void f1(bool a, bool b, bool c, bool d)
    {
        bool res = a && b && c && d;
        Console.WriteLine(res);
    }

    static void f2(bool a, bool b, bool c, bool d)
    {
        bool res = a || b || c || d;
        Console.WriteLine(res);
    }

    static void f3(bool a, bool b, bool c, bool d)
    {
        bool res = (a && b) || (c && d);
        Console.WriteLine(res);
    }

    static void f4(bool a, bool b, bool c, bool d)
    {
        bool res = (a || b) && (c || d);
        Console.WriteLine(res);
    }

    static bool a(int k, int pos)
    {
        return (k & (1 << pos)) != 0;
    }

    static void Main()
    {
        Console.WriteLine("f1 = a && b && c && d");
        for (int k = 0; k < 1 << 4; ++k)
        {
            f1(a(k, 3), a(k, 2), a(k, 1), a(k, 0));
        }

        Console.WriteLine("f2 = a || b || c || d");
        for (int k = 0; k < 1 << 4; ++k)
        {
            f2(a(k, 3), a(k, 2), a(k, 1), a(k, 0));
        }
        
        Console.WriteLine("f3 = (a && b) || (c && d)");
        for (int k = 0; k < 1 << 4; ++k)
        {
            f3(a(k, 3), a(k, 2), a(k, 1), a(k, 0));
        }

        Console.WriteLine("f4 = (a || b) && (c || d)");
        for (int k = 0; k < 1 << 4; ++k)
        {
            f4(a(k, 3), a(k, 2), a(k, 1), a(k, 0));
        }
        Console.WriteLine("<%END%>");
    }
}