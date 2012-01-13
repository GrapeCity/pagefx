using System;

class A
{
    public virtual void f1()
    {
        Console.WriteLine("A::f1()");
        f2();
    }

    protected virtual void f2()
    {
        Console.WriteLine("A::f2()");
        f3();
    }

    private void f3()
    {
        Console.WriteLine("A::f3()");
    }
}

class B : A
{
    public override void f1()
    {
        base.f1();
        Console.WriteLine("B::f1()");
    }

    protected override void f2()
    {
        base.f2();
        Console.WriteLine("B::f2()");
    }
}

class Visibility1
{
    static void Main()
    {
        A obj = new A();
        obj.f1();
        obj = new B();
        obj.f1();
        Console.WriteLine("<%END%>");
    }
}