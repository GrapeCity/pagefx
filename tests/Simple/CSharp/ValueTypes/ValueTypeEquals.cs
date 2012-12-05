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

class Test
{
    static void Main()
    {
		Console.WriteLine(Equals(new Point(), new Point()));
		Console.WriteLine(Equals(new Point(10, 10), new Point(10, 10)));
		Console.WriteLine(Equals(new Point(10, 10), new Point(20, 20)));
        Console.WriteLine("<%END%>");
    }
}