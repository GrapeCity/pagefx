using System;

class Base
{
    public virtual void f()
    {
        Console.WriteLine("Base::f()");
    }
}

class Derrived : Base
{
    public override void f()
    {
        base.f();
        Console.WriteLine("Derrived::f()");
    }
}

class Virtual3
{
    static void Main()
    {
        Base obj = new Derrived();
        obj.f();
        Console.WriteLine("<%END%>");
    }
}