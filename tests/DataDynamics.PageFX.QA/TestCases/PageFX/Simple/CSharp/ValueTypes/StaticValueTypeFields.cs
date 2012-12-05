using System;

struct Point
{
    public int x;
    public int y;
}

class StaticValueTypeFields
{
    private static Point pt;

    static void Main()
    {
        Console.WriteLine(pt.x);
        Console.WriteLine(pt.y);
        Console.WriteLine("<%END%>");
    }
}