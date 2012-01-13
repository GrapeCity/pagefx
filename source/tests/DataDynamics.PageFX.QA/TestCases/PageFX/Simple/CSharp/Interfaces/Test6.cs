using System;

namespace NS1
{
    public interface I
    {
        int Count { get; }
    }
}

namespace NS2
{
    public interface I
    {
        int Count { get; }
    }
}

class A : NS1.I, NS2.I
{
    public int Count
    {
        get { return 10; }
    }
}

class Program
{
    static void Main()
    {
        A a = new A();
        Console.WriteLine(a.Count);
        Console.WriteLine("<%END%>");
    }
}