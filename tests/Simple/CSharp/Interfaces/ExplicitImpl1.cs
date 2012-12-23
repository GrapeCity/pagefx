using System;

interface I
{
	void f();
}

class A : I
{
	internal void f()
	{
		Console.WriteLine("A::f()");
	}

	void I.f()
    {
    	f();
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
