using System;

interface I
{
	void f();
}

class B : I
{
	public void f()
	{
		Console.WriteLine("B::f()");
	}
}

class A : B
{
	public new void f()
	{
		Console.WriteLine("A::f()");
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
