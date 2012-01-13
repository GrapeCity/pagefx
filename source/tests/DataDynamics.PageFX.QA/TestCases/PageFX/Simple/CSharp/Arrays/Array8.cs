using System;

struct Point
{
    public int x;
    public int y;
}

class Array8
{
    static void Main()
    {
        Point p1 = new Point();
        p1.x = 100;
        p1.y = 100;

        Point p2 = new Point();
        p2.x = 200;
        p2.y = 200;

        Point p3 = new Point();
        p3.x = 300;
        p3.y = 300;

        Point[] arr = new Point[3];
        arr[0] = p1;
        arr[1] = p2;
        arr[2] = p3;

        p1.x += 1;
        p1.y += 1;
        p2.x += 2;
        p2.y += 2;
        p3.x += 3;
        p3.y += 3;

        Console.WriteLine(arr[0].x);
        Console.WriteLine(arr[0].y);
        Console.WriteLine(arr[1].x);
        Console.WriteLine(arr[1].y);
        Console.WriteLine(arr[2].x);
        Console.WriteLine(arr[2].y);
        Console.WriteLine(p1.x);
        Console.WriteLine(p1.y);
        Console.WriteLine(p2.x);
        Console.WriteLine(p2.y);
        Console.WriteLine(p3.x);
        Console.WriteLine(p3.y);
        Console.WriteLine("<%END%>");
    }
}