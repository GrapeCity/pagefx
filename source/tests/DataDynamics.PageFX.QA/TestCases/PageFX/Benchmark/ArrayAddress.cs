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

class Test
{
    static void Main()
    {
        int start = Environment.TickCount;
        const int D1 = 10;
        const int D2 = 10;
        Point[,] arr = new Point[D1,D2];
        int n = 10000;
        for (int i = 0; i < n; ++i)
        {
            for (int x = 0; x < D1; ++x)
            {
                for (int y = 0; y < D2; ++y)
                {
                    arr[x, y].X = i;
                    arr[x, y].Y = i;
                }
            }
        }
        int end = Environment.TickCount;
        Console.WriteLine(end - start);
        Console.WriteLine("<%END%>");
    }
}