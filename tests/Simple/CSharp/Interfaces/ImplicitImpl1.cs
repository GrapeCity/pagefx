using System;

interface I
{
	void f();
}

class B
{
	public void f()
	{
		Console.WriteLine("B::f()");
	}
}

class A : B, I
{
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
