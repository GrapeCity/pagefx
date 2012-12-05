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
    public readonly Mutable m = new Mutable();

    static void Main()
    {
        Test t = new Test();
        Console.WriteLine(t.m.Mutate());
        Console.WriteLine(t.m.Mutate());
        Console.WriteLine(t.m.Mutate());
        Console.WriteLine("<%END%>");
    }
}