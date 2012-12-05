using System;

struct Point
{
    public int X, Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

struct PointF
{
    public float X, Y;

    public PointF(float x, float y)
    {
        X = x;
        Y = y;
    }
}

class Test
{
    static int Foo(Point p)
    {
        return p.X * p.X + p.Y * p.Y;
    }

    static float Foo(PointF p)
    {
        return p.X * p.X + p.Y * p.Y;
    }

    static void Main()
    {
        int start = Environment.TickCount;
        for (int y = 0; y < 1000; ++y)
        {
            for (int x = 0; x < 1000; ++x)
            {
                {
                    Point p = new Point(x, y);
                    p.X = Foo(p);

                    p.X = x + y;
                    p.Y = x - y;
                    p.X = Foo(p);

                    p.X = x * y;
                    p.Y = x ^ y;
                    p.Y = Foo(p);
                }
                {
                    PointF p = new PointF(x, y);
                    p.X = Foo(p);

                    p.X = x + y;
                    p.Y = x - y;
                    p.X = Foo(p);

                    p.X = x * y;
                    p.Y = p.X * y;
                    p.Y = Foo(p);
                }
            }
        }
        int end = Environment.TickCount;
        Console.WriteLine(end - start);
        Console.WriteLine("<%END%>");
    }
}