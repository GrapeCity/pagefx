using System;

struct Point
{
    public int X;
    public int Y;
}

class New2
{
    static void Main()
    {
        Point pt = new Point();
        pt.X = 10;
        pt.Y = 20;
        Console.WriteLine(pt.X);
        Console.WriteLine(pt.Y);
        Console.WriteLine("<%END%>");
    }
}