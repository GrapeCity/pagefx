using System;

struct Point
{
    public int x;
    public int y;
}

class X
{
    static void f(ref Point pt)
    {
        pt.x += 10;
        pt.y -= 10;
    }

    static void Main()
    {
        Point pt = new Point();
        pt.x = 10;
        pt.y = 10;
        Console.WriteLine(pt.x);
        Console.WriteLine(pt.y);
        f(ref pt);
        Console.WriteLine(pt.x);
        Console.WriteLine(pt.y);
        f(ref pt);
        Console.WriteLine(pt.x);
        Console.WriteLine(pt.y);
        Console.WriteLine("<%END%>");
    }
}