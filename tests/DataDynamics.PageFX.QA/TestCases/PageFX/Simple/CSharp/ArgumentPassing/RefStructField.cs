using System;

struct Point
{
    public int x;
    public int y;
}

class X
{
    static void f(ref int a)
    {
        Console.WriteLine(a);
        a = a + 10;
        Console.WriteLine(a);
    }

    static void f(ref Point pt)
    {
        Console.WriteLine(pt.x);
        Console.WriteLine(pt.y);
        pt.x += 10;
        pt.y += 20;
        Console.WriteLine(pt.x);
        Console.WriteLine(pt.y);
        pt = new Point();
    }

    static void Main()
    {
        Point pt = new Point();
        f(ref pt);
        f(ref pt.x);
        f(ref pt);
        f(ref pt.y);
        Console.WriteLine("<%END%>");
    }
}