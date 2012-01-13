using System;

class A
{
    public virtual void f()
    {
        Console.WriteLine("A::f()");
    }

    public virtual void f(int a)
    {
        Console.WriteLine("A::f(int)");
    }

    public virtual void f(string s)
    {
        Console.WriteLine("A::f(string)");
    }
}

class B : A
{
    public override void f(int a)
    {
        base.f(a);
        Console.WriteLine("B::f(int)");
    }

    public virtual void f(int a, int b)
    {
        Console.WriteLine("B::f(int, int)");
    }
}

class C : B
{
    public override void f()
    {
        base.f();
        Console.WriteLine("C::f()");
    }

    public override void f(string s)
    {
        base.f(s);
        Console.WriteLine("C::f(string)");
    }

    public virtual void f(int a, int b, int c)
    {
        Console.WriteLine("C::f(int, int, int)");
    }
}

class Test3
{
    static void Main()
    {
        C obj = new C();
        obj.f();
        obj.f(1);
        obj.f(1, 2);
        obj.f(1, 2, 3);
        obj.f("aaa");
        Console.WriteLine("<%END%>");
    }
}