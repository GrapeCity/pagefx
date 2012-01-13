using System;

class Point
{
    public int x;
    public int y;
}

class X
{
    static void f(Point p)
    {
        Console.WriteLine(p.x);
        Console.WriteLine(p.y);
        p.x += 10;
        p.y -= 10;
        Console.WriteLine(p.x);
        Console.WriteLine(p.y);
    }

    static void Main()
    {
        Point pt = new Point();
        pt.x = 10;
        pt.y = 10;
        f(pt);
        Console.WriteLine(pt.x);
        Console.WriteLine(pt.y);
        f(pt);
        Console.WriteLine("<%END%>");
    }
}