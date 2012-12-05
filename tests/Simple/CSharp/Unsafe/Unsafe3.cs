using System;

struct Point
{
    public int x;
    public int y;
    public static int N = 100;
}

unsafe class TestUnsafe3
{
    static void print(Point pt)
    {
        Console.WriteLine("({0}, {1})", pt.x, pt.y);
    }

    static void print(Point* ptr)
    {
        Console.WriteLine("({0}, {1})", ptr->x, ptr->y);
    }

    static void print(int* ptr)
    {
        Console.WriteLine(*ptr);
    }

    static void Main()
    {
        Point pt = new Point();
        pt.x = 10;
        pt.y = 20;
        print(pt);

        Point* ptr = &pt;
        ptr->x = ptr->x + 10;
        ptr->y = ptr->y + 10;
        print(pt);
        print(ptr);

        print(&pt.x);
        print(&pt.N);
    }
}