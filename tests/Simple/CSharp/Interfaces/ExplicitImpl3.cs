using System;

interface I
{
	void f();
}

class B
{
	public virtual void f()
	{
		Console.WriteLine("B::f()");
	}
}

class A : B, I
{
	public override void f()
	{
		Console.WriteLine("A::f()");
	}

	void I.f()
	{
		Console.WriteLine("A::I.f()");
	}
}

class Program
{
    static void Main()
    {
        var obj = new A();
        obj.f();
        ((I)obj).f();
        Console.WriteLine("<%END%>");
    }
}
