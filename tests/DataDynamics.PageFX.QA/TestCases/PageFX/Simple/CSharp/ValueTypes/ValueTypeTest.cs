using System;

struct Point
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

	public static readonly Point Empty;

    public override string ToString()
    {
        return string.Format("({0}, {1})", x, y);
    }
}

struct PointF
{
    public float X;
    public float Y;

    public static readonly PointF Empty;

    public override string ToString()
    {
        return string.Format("({0}, {1})", X, Y);
    }
}

class A
{
    public Point location;
    public static double ZeroD;
    public static float ZeroF;

    public A()
    {
        location.x = 10;
        location.y = 20;
    }
}

class ValueTypeTest
{
    static void Test1()
    {
        Point a = new Point(10, 10);
        Point b = a;
        a.x = 20;
        a.y = 20;
        Console.WriteLine(a.x);
        Console.WriteLine(a.y);
        Console.WriteLine(b.x);
        Console.WriteLine(b.y);
    }

    static void Test2()
    {
        Console.WriteLine(Equals(new Point(), new Point()));
        Console.WriteLine(Equals(new Point(10, 10), new Point(10, 10)));
        Console.WriteLine(Equals(new Point(10, 10), new Point(20, 20)));
    }

    static void Test3()
    {
        A obj = new A();
        Console.WriteLine(obj.location.x);
        Console.WriteLine(obj.location.y);
    }

    static void Test4()
    {
        Console.WriteLine(Point.Empty);
        Console.WriteLine(PointF.Empty);
        Console.WriteLine(A.ZeroD);
        Console.WriteLine(A.ZeroF);
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Test4();
        Console.WriteLine("<%END%>");
    }
}