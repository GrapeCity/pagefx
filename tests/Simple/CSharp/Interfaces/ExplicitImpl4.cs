using System;

interface I
{
	void f();
}

class B : I
{
	public virtual void f()
	{
		Console.WriteLine("B::f()");
	}

	void I.f()
	{
		Console.WriteLine("B::I.f()");
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
	static void Test1()
	{
		var obj = new A();
        obj.f();
        ((I)obj).f();
	}

	static void Test2()
	{
		var obj = new B();
        obj.f();
        ((I)obj).f();
	}

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}
