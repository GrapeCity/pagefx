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
        Point[] arr = new Point[100];
        int n = 100000;
        for (int i = 0; i < n; ++i)
        {
            int j = i % arr.Length;
            arr[j].X++;
            arr[j].Y++;
        }
        int end = Environment.TickCount;
        Console.WriteLine(end - start);
        Console.WriteLine("<%END%>");
    }
}