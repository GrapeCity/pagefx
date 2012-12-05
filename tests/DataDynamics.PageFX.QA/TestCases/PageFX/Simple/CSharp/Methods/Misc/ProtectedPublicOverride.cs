using System;

class A
{
    protected void f()
    {
        Console.WriteLine("A::f()");
    }

    protected virtual void foo()
    {
        Console.WriteLine("A::foo()");
    }
}

class B : A
{
    public void f()
    {
        Console.WriteLine("B::f()");
    }

    public void foo()
    {
        Console.WriteLine("B::foo()");
    }
}

class X
{
    static void Main()
    {
        B b = new B();
        b.foo();
        b.f();
        Console.WriteLine("<%END%>");
    }
}