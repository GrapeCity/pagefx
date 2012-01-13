using System;

class X
{
	public static int N;
}
#region A1
class A1
{
	static A1()
	{
		Console.WriteLine("A1.cctor");
		X.N++;
		A4.Foo();
	}
	static bool foo_called;
	public static void Foo()
	{
		if (foo_called) return;
		foo_called = true;
		Console.WriteLine("A1.Foo");
		X.N++;
		A2.Foo();
		A3.Foo();
		A4.Foo();
		A5.Foo();
		A6.Foo();
		A7.Foo();
		A8.Foo();
		A9.Foo();
		A10.Foo();
	}

}
#endregion
#region A2
class A2
{
	static A2()
	{
		Console.WriteLine("A2.cctor");
		X.N++;
		A8.Foo();
	}
	static bool foo_called;
	public static void Foo()
	{
		if (foo_called) return;
		foo_called = true;
		Console.WriteLine("A2.Foo");
		X.N++;
		A1.Foo();
		A3.Foo();
		A4.Foo();
		A5.Foo();
		A6.Foo();
		A7.Foo();
		A8.Foo();
		A9.Foo();
		A10.Foo();
	}

}
#endregion
#region A3
class A3
{
	static A3()
	{
		Console.WriteLine("A3.cctor");
		X.N++;
		A5.Foo();
	}
	static bool foo_called;
	public static void Foo()
	{
		if (foo_called) return;
		foo_called = true;
		Console.WriteLine("A3.Foo");
		X.N++;
		A1.Foo();
		A2.Foo();
		A4.Foo();
		A5.Foo();
		A6.Foo();
		A7.Foo();
		A8.Foo();
		A9.Foo();
		A10.Foo();
	}

}
#endregion
#region A4
class A4
{
	static A4()
	{
		Console.WriteLine("A4.cctor");
		X.N++;
		A8.Foo();
	}
	static bool foo_called;
	public static void Foo()
	{
		if (foo_called) return;
		foo_called = true;
		Console.WriteLine("A4.Foo");
		X.N++;
		A1.Foo();
		A2.Foo();
		A3.Foo();
		A5.Foo();
		A6.Foo();
		A7.Foo();
		A8.Foo();
		A9.Foo();
		A10.Foo();
	}

}
#endregion
#region A5
class A5
{
	static A5()
	{
		Console.WriteLine("A5.cctor");
		X.N++;
		A3.Foo();
	}
	static bool foo_called;
	public static void Foo()
	{
		if (foo_called) return;
		foo_called = true;
		Console.WriteLine("A5.Foo");
		X.N++;
		A1.Foo();
		A2.Foo();
		A3.Foo();
		A4.Foo();
		A6.Foo();
		A7.Foo();
		A8.Foo();
		A9.Foo();
		A10.Foo();
	}

}
#endregion
#region A6
class A6
{
	static A6()
	{
		Console.WriteLine("A6.cctor");
		X.N++;
		A9.Foo();
	}
	static bool foo_called;
	public static void Foo()
	{
		if (foo_called) return;
		foo_called = true;
		Console.WriteLine("A6.Foo");
		X.N++;
		A1.Foo();
		A2.Foo();
		A3.Foo();
		A4.Foo();
		A5.Foo();
		A7.Foo();
		A8.Foo();
		A9.Foo();
		A10.Foo();
	}

}
#endregion
#region A7
class A7
{
	static A7()
	{
		Console.WriteLine("A7.cctor");
		X.N++;
		A2.Foo();
	}
	static bool foo_called;
	public static void Foo()
	{
		if (foo_called) return;
		foo_called = true;
		Console.WriteLine("A7.Foo");
		X.N++;
		A1.Foo();
		A2.Foo();
		A3.Foo();
		A4.Foo();
		A5.Foo();
		A6.Foo();
		A8.Foo();
		A9.Foo();
		A10.Foo();
	}

}
#endregion
#region A8
class A8
{
	static A8()
	{
		Console.WriteLine("A8.cctor");
		X.N++;
		A5.Foo();
	}
	static bool foo_called;
	public static void Foo()
	{
		if (foo_called) return;
		foo_called = true;
		Console.WriteLine("A8.Foo");
		X.N++;
		A1.Foo();
		A2.Foo();
		A3.Foo();
		A4.Foo();
		A5.Foo();
		A6.Foo();
		A7.Foo();
		A9.Foo();
		A10.Foo();
	}

}
#endregion
#region A9
class A9
{
	static A9()
	{
		Console.WriteLine("A9.cctor");
		X.N++;
		A8.Foo();
	}
	static bool foo_called;
	public static void Foo()
	{
		if (foo_called) return;
		foo_called = true;
		Console.WriteLine("A9.Foo");
		X.N++;
		A1.Foo();
		A2.Foo();
		A3.Foo();
		A4.Foo();
		A5.Foo();
		A6.Foo();
		A7.Foo();
		A8.Foo();
		A10.Foo();
	}

}
#endregion
#region A10
class A10
{
	static A10()
	{
		Console.WriteLine("A10.cctor");
		X.N++;
		A7.Foo();
	}
	static bool foo_called;
	public static void Foo()
	{
		if (foo_called) return;
		foo_called = true;
		Console.WriteLine("A10.Foo");
		X.N++;
		A1.Foo();
		A2.Foo();
		A3.Foo();
		A4.Foo();
		A5.Foo();
		A6.Foo();
		A7.Foo();
		A8.Foo();
		A9.Foo();
	}

}
#endregion
class Test
{
	static void Main()
	{
		A1.Foo();
		A2.Foo();
		A3.Foo();
		A4.Foo();
		A5.Foo();
		A6.Foo();
		A7.Foo();
		A8.Foo();
		A9.Foo();
		A10.Foo();
		Console.WriteLine("<%END%>");
	}
}
