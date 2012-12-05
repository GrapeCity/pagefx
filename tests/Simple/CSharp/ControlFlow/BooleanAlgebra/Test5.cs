using System;

class Test5
{
    static void f1(bool a, bool b, bool c, bool d, bool e, bool f, bool g, bool h, bool i, bool j, bool k, bool l)
    {
        bool res = (((((a && b) || (c && d)) && e) || f) && (g || h) && (i || j)) || (k && l);
        Console.WriteLine(res);
    }

    static void f2(bool a, bool b, bool c, bool d, bool e, bool f, bool g, bool h, bool i, bool j, bool k, bool l)
    {
        bool res = (((((a && b) || (c && d)) && e) || f) && (g && h) && (i || j)) || (k && l);
        Console.WriteLine(res);
    }

    static bool a(int k, int pos)
    {
        return (k & (1 << pos)) != 0;
    }

    static void Main()
    {
        int N = 1 << 12;
        for (int k = 0; k < N; ++k)
        {
            f1(a(k, 11), a(k, 10), a(k, 9), a(k, 8), a(k, 7), a(k, 6), a(k, 5), a(k, 4), a(k, 3), a(k, 2), a(k, 1), a(k, 0));
            f2(a(k, 11), a(k, 10), a(k, 9), a(k, 8), a(k, 7), a(k, 6), a(k, 5), a(k, 4), a(k, 3), a(k, 2), a(k, 1), a(k, 0));
        }
        Console.WriteLine("<%END%>");
    }
}