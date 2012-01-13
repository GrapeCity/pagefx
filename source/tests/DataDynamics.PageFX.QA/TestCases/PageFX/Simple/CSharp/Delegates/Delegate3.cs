using System;

delegate void Func();

class A
{
    public virtual void f()
    {
        Console.WriteLine("A::f()");
    }
}

class B : A
{
    public override void f()
    {
        Console.WriteLine("B::f()");
    }
}

class Delegate3
{
    static void Main()
    {
        A a = new A();
        Func f = a.f;
        f();

        B b = new B();
        f = b.f;
        f();
        Console.WriteLine("<%END%>");
    }
}