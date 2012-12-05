using System;

struct Point
{
    public int x;
    public int y;

    public Point(string s)
    {
        Point pt = Parse(s);
        this = pt;
    }

    public static Point Parse(string s)
    {
        Point pt = new Point();
        pt.x = 10;
        pt.y = 20;
        return pt;
    }
}

class Test
{
    static void Main()
    {
        Point pt = new Point("10;10");
        Console.WriteLine(pt.x);
        Console.WriteLine(pt.y);
        Console.WriteLine("<%END%>");
    }
}