using System;

class A
{
    B _impl = new B {Position = 100};

    public long Position
    {
        get { return _impl.Position; }
    }

    public ulong Position2
    {
        get { return _impl.Position; }
    }

    public ulong Position3
    {
        get { return (ulong)_impl.Position2; }
    }
}

class B
{
    public uint Position { get; set; }

    public int Position2
    {
        get { return (int)Position; }
    }
}

class X
{
    static void Test1()
    {
        Console.WriteLine("---Test1");
        var a = new A();
        Console.WriteLine(a.Position);
        Console.WriteLine(a.Position2);
        Console.WriteLine(a.Position3);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}