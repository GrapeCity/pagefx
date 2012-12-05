using System;

class Test1
{
    static void f1(int a, int b)
    {
        Console.WriteLine("f1");
        int c = a > b ? 10 : -10;
        System.Console.WriteLine(c);
    }

    static void f2(int a, int b)
    {
        Console.WriteLine("f2");
        int c = a > b ? 1 : -1;
        System.Console.WriteLine(c);
    }

    static void f3(bool a, bool b)
    {
        Console.WriteLine("f2");
        bool r = a ? true : b;
        Console.WriteLine(r);
    }

    static void f4(int a, int b, int c)
    {
        Console.WriteLine("f3");
        if (((a > b ? 10 : -10) - c >= 0) && c < b)
        {
            Console.WriteLine("1");
        }
        else
        {
            Console.WriteLine("0");
        }
    }

    static void Main()
    {
        f1(10, 20);
        f1(20, 10);
        f2(10, 20);
        f2(20, 10);
        f3(true, false);
        f4(10, 10, 10);
        Console.WriteLine("<%END%>");
    }
}