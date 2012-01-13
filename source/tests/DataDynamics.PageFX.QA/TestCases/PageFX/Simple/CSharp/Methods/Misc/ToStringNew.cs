using System;

struct Point
{
    public int X;
    public int Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public new string ToString()
    {
        return "X: " + X + "Y: " + Y;
    }
}

class X
{
    public new string ToString()
    {
        return "X";
    }

    static void Test1()
    {
        X x = new X();
        Console.WriteLine(x.ToString());
    }

    static void Test2()
    {
        Point pt = new Point(10, 10);
        Console.WriteLine(pt.ToString());
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}