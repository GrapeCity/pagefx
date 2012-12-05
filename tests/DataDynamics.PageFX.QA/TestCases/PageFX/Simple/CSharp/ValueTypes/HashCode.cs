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

class Test
{
	static void Test1()
	{
		Console.WriteLine("# Test1");
		var pt1 = new Point(10, 10);
		var pt2 = new Point(20, 20);
		var pt3 = new Point(10, 10);
		var pt4 = new Point2(10, 10);
		var pt5 = new Point2(20, 20);
		Console.WriteLine(pt1.GetHashCode() == pt2.GetHashCode());
		Console.WriteLine(pt1.GetHashCode() == pt3.GetHashCode());
		Console.WriteLine(pt1.GetHashCode() == pt4.GetHashCode());
		Console.WriteLine(pt1.GetHashCode() == pt5.GetHashCode());
		Console.WriteLine(pt2.GetHashCode() == pt4.GetHashCode());
		Console.WriteLine(pt2.GetHashCode() == pt5.GetHashCode());
	}

    static void Main()
    {
	    Test1();
		Console.WriteLine("<%END%>");
    }
}