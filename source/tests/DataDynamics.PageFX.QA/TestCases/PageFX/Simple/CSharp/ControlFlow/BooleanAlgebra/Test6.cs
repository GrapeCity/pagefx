using System;

class Test6
{
    static void f(int i, bool b, bool c)
    {
        Console.WriteLine((i++ > 3 && b) || (i++ > 3 && c));
    }

    static void Main()
    {
        for (int i = 0; i <= 5; ++i)
        {
            f(i, false, false);
            f(i, false, true);
            f(i, true, false);
            f(i, true, true);
        }
        Console.WriteLine("<%END%>");
    }
}