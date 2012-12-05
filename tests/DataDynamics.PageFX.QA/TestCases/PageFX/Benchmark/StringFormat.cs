using System;

struct Point
{
    public int X, Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return string.Format("({0}, {1})", X, Y);
    }
}

class Test
{
    static void Main()
    {
        int start = Environment.TickCount;
        int n = 1000;
        for (int i = 0; i < n; ++i)
        {
            string s = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                                     i, i - 1, i - 2, i - 3, i - 4,
                                     i - 5, i - 6, i - 7, i - 8, i - 9);
            Point p1 = new Point(i, i);
            Point p2 = new Point(n, n);
            Point p3 = new Point(start, start);
            s = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9:X4},",
                                     p1, p2, p3, new object(), "aaa",
                                     new Test(), new Test(), new Test(), 3.14, 0xFF);

            s = string.Format("{0:X4},{1:X4},{2:X4},{3:X4},{4:X4},{5:X4},{6:X4},{7:X4},{8:X4},{9:X4},",
                                     p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y, i, n, start, 0xAABB);
        }
        int end = Environment.TickCount;
        Console.WriteLine(end - start);
        Console.WriteLine("<%END%>");
    }

    public override string ToString()
    {
        return string.Format("Test{0}", Environment.TickCount);
    }
}