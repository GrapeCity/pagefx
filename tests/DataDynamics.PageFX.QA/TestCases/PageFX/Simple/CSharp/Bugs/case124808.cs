using System;

class Z
{
    public int WindowSize;
}

class A
{
    public int wrap;
    public B B;

    public void Foo(Z z)
    {
        Console.WriteLine("A::Foo()");
        B = new B(z, wrap != 0 ? null : this, z.WindowSize);
        B.Foo();
    }
}

class B
{
    public B(Z z, object obj, int windowSize)
    {
    }

    public void Foo()
    {
        Console.WriteLine("B::Foo()");
    }
}

class X
{
    static void Main()
    {
        Z z = new Z();
        A a = new A();
        a.Foo(z);
        Console.WriteLine("<%END%>");
    }
}