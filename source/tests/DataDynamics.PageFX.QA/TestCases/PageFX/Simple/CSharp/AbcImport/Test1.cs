using System;

class Test1
{
    static void Main()
    {
        Shapes.Point pt = new Shapes.Point(10, 10);
        Console.WriteLine(pt.X);
        Console.WriteLine(pt.Y);
        pt.Move(10, 10);
        Console.WriteLine(pt.X);
        Console.WriteLine(pt.Y);
        Console.WriteLine("<%END%>");
    }
}