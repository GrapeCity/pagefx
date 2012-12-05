using System;

class Test4
{
    static void f1(bool a, bool b, bool c, bool d, bool e, bool f)
    {
        bool res = (a && b && c) || (d && e && f);
        Console.WriteLine(res);
    }

    static void f2(bool a, bool b, bool c, bool d, bool e, bool f)
    {
        bool res = (a || b || c) && (d || e || f);
        Console.WriteLine(res);
    }

    static void f3(bool a, bool b, bool c, bool d, bool e, bool f, bool g)
    {
        bool res = (a && b || c) && (d || e) || (f && g);
        Console.WriteLine(res);
    }

    static void f4(bool a, bool b, bool c, bool d, bool e, bool f, bool g)
    {
        bool res = (a || b && c) || (d && e) && (f || g);
        Console.WriteLine(res);
    }

    static void f5(bool a, bool b, bool c, bool d, bool e, bool f)
    {
        bool res = (a || b && c) && (d && e || f);
        Console.WriteLine(res);
    }

    static bool a(int k, int pos)
    {
        return (k & (1 << pos)) != 0;
    }

    static void Main()
    {
        Console.WriteLine("f1 = (a && b && c) || (d && e && f)");
        for (int k = 0; k < 1 << 6; ++k)
        {
            f1(a(k, 5), a(k, 4), a(k, 3), a(k, 2), a(k, 1), a(k, 0));
        }

        Console.WriteLine("f2 = (a || b || c) && (d || e || f)");
        for (int k = 0; k < 1 << 6; ++k)
        {
            f2(a(k, 5), a(k, 4), a(k, 3), a(k, 2), a(k, 1), a(k, 0));
        }
        
        Console.WriteLine("f3 = (a && b || c) && (d || e) || (f && g)");
        for (int k = 0; k < 1 << 7; ++k)
        {
            f3(a(k, 6), a(k, 5), a(k, 4), a(k, 3), a(k, 2), a(k, 1), a(k, 0));
        }
        
        Console.WriteLine("f4 = (a || b && c) || (d && e) && (f || g)");
        for (int k = 0; k < 1 << 7; ++k)
        {
            f4(a(k, 6), a(k, 5), a(k, 4), a(k, 3), a(k, 2), a(k, 1), a(k, 0));
        }
        
        Console.WriteLine("f5 = (a || b && c) && (d && e || f)");
        for (int k = 0; k < 1 << 6; ++k)
        {
            f5(a(k, 5), a(k, 4), a(k, 3), a(k, 2), a(k, 1), a(k, 0));
        }
        Console.WriteLine("<%END%>");
    }
}