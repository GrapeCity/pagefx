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
    static void Swap(ref int a, ref int b)
    {
        int t = a;
        a = b;
        b = t;
    }

    static void Swap(ref long a, ref long b)
    {
        long t = a;
        a = b;
        b = t;
    }

    static void Foo(ref int a1, ref int a2, ref int a3, ref int a4,
        ref long a5, ref long a6, ref long a7, ref long a8, ref long a9, ref long a10)
    {
        Swap(ref a1, ref a2);
        Swap(ref a3, ref a4);
        Swap(ref a5, ref a6);
        Swap(ref a7, ref a8);
        Swap(ref a9, ref a10);
    }

    static void Main()
    {
        int start = Environment.TickCount;
        int n = 100000;
        int a1 = 0, a2 = 0, a3 = 0, a4 = 0;
        long a5 = 0, a6 = 0, a7 = 0, a8 = 0, a9 = 0, a10 = 0;
        Point p1 = new Point();
        Point p2 = new Point();
        int[] A1 = new int[] { 10, 20, 30, 40 };
        long[] A2 = new long[] { 10, 20, 30, 40, 50, 60 };
        for (int i = 0; i < n; ++i)
        {
            Foo(ref a1, ref a2, ref a3, ref a4,
                ref a5, ref a6, ref a7, ref a8, ref a9, ref a10);
            Foo(ref p1.X, ref p1.Y, ref p2.X, ref p2.Y,
                ref a5, ref a6, ref a7, ref a8, ref a9, ref a10);
            Foo(ref A1[0], ref A1[1], ref A1[2], ref A1[3], 
                ref A2[0], ref A2[1], ref A2[2], ref A2[3], ref A2[4], ref A2[5]);
            Swap(ref a1, ref a2);
            Swap(ref a3, ref a4);
            Swap(ref a5, ref a6);
            Swap(ref a7, ref a8);
            Swap(ref a9, ref a10);
            Swap(ref p1.X, ref p1.Y);
            Swap(ref p2.X, ref p2.Y);
        }
        int end = Environment.TickCount;
        Console.WriteLine(end - start);
        Console.WriteLine("<%END%>");
    }
}

