using System;

interface I1
{
	void f();
}

interface I2
{
	void f();
}

interface I3 : I2
{
	void g();
}

class B : I1
{
	public virtual void f()
	{
		Console.WriteLine("B::f()");
	}
}

class A : B, I2
{
}

class C : A, I3
{
	public override void f()
	{
		Console.WriteLine("C::f()");
		base.f();
	}

	public void g()
	{
		Console.WriteLine("C::g()");
	}
}

class Program
{
    static void Main()
    {
        var obj = new C();
        obj.f();
        ((I1)obj).f();
        ((I2)obj).f();
        obj.g();
        ((I3)obj).g();
        Console.WriteLine("<%END%>");
    }
}
