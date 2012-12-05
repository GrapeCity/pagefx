using System;

struct Point1
{
    public int x;
    public int y;

    public Point1(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

struct Point2
{
    public int x;
    public int y;

    public Point2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

class X
{
    static Point2[] ToPoint2Array(Array arr)
    {
        return (Point2[])arr;
    }

    static void Test1()
    {
        try
        {
            Console.WriteLine("1");
            Point1[] a = new Point1[] { new Point1(1, 2), new Point1(2, 3), new Point1(4, 5) };
            Console.WriteLine("2");
            Point2[] b = ToPoint2Array(a);
            Console.WriteLine("3");
            for (int i = 0; i < b.Length; ++i)
                Console.WriteLine("{0}; {1}", b[i].x, b[i].y);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}