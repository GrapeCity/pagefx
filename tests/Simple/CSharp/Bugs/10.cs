using System;

struct S
{
    public int X
    {
        get;
        set;
    }
}

class Test
{
    static void Main()
    {
        Console.WriteLine("Hello!");
        S s = new S { X = 100 };
        s.X += 100;
        Console.WriteLine(s.X);
        Console.WriteLine("<%END%>");
    }
}