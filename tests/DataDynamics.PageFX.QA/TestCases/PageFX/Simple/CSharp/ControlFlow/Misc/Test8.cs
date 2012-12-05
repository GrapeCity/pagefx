using System;

class Test8
{
    static void f1(bool a, bool b)
    {
        Console.WriteLine(a && b);
    }

    static void f2(int i, bool b, bool c)
    {
        f1((i++ > 3 && b) || (i++ > 3 && c), b && c);
    }

    static void Main()
    {
        for (int i = 0; i <= 5; ++i)
        {
            f2(i, false, false);
            f2(i, false, true);
            f2(i, true, false);
            f2(i, true, true);
        }
        Console.WriteLine("<%END%>");
    }
}