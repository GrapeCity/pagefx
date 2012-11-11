//http://blogs.msdn.com/ericlippert/archive/2008/05/14/mutating-readonly-structs.aspx

using System;

struct Mutable
{
    private int x;
    public int Mutate()
    {
        x = x + 1;
        return x;
    }
}

class Test
{
    public static readonly Mutable m = new Mutable();

    static void Main()
    {
        Console.WriteLine(m.Mutate());
        Console.WriteLine(m.Mutate());
        Console.WriteLine(m.Mutate());
        Console.WriteLine("<%END%>");
    }
}