using System;

class A
{
    public virtual void f()
    {
        Console.WriteLine("A::f()");
    }
}

class B : A
{
    public new int f()
    {
        Console.WriteLine("B::f()");
        return 0;
    }
}

class Virtual4
{
    static void Main()
    {
        B b = new B();
        Console.WriteLine(b.f());
        Console.WriteLine("<%END%>");
    }
}