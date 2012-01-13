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

    static void Main()
    {
        Point pt = new Point();
        pt.x = 10;
        pt.y = 20;
        f(ref pt.x);
        f(ref pt.y);
        f(ref pt.x);
        f(ref pt.y);
        Console.WriteLine("<%END%>");
    }
}