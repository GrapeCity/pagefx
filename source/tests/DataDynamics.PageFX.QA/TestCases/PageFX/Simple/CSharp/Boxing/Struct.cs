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

    public override string ToString()
    {
        return string.Format("{{X = {0}, Y = {1}}}", X, Y);
    }
}

class Test
{
    static void Unbox(object obj)
    {
        Console.WriteLine(obj);
        try
        {
            Point v = (Point)obj;
            Console.WriteLine(v);
            Console.WriteLine("ok");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType().FullName);
        }
    }

    public static void Main()
    {
        Unbox(new Point(10, 10));
        Unbox(null);
        Unbox(new Test());
        Unbox("str");
    }
}