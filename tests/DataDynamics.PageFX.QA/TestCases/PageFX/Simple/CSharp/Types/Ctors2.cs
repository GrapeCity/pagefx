using System;

class A
{
    public int x1;

    public A(int p1)
    {
        x1 = p1;
    }
}

class B : A
{
    public int x2;

    public B(int p1, int p2)
        : base(p1)
    {
        x2 = p2;
    }
}

class X
{
    static void Main()
    {
        B b = new B(10, 20);
        Console.WriteLine(b.x1);
        Console.WriteLine(b.x2);
        Console.WriteLine("<%END%>");
    }
}