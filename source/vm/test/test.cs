using System;

class Test
{
	private int _value;

	static Test()
	{
		Console.WriteLine("static ctor");
	}

	Test(int value)
	{
		_value = value;
		Console.WriteLine("ctor({0})", _value);
	}

	public static int X;

	private delegate void D(int value);

	public static void Foo(int value)
	{
		X = value;
		Console.WriteLine(value);
	}

	public static void Bar()
	{
		Console.WriteLine("Bar called!");
	}

	void Set(int value)
	{
		_value = value;
		Console.WriteLine("set({0})", _value);
	}

	public static void Main(string[] args)
	{
		Console.WriteLine("Hello");

		var program = new Test(123);

		for (var i = 0; i < args.Length; i++)
		{
			Console.WriteLine(args[i]);
		}

		int n = 10;
		var arr = new int[n];

		for (var i = 0; i < n; i++)
		{
			arr[i] = i * i;
			Foo(i);
			program.Set(arr[i]);
			new D(program.Set)(i);
		}

		switch (X)
		{
			case  0:
				Console.WriteLine(X+1);
				break;
			case  1:
				Console.WriteLine(X+2);
				break;
			case  2:
				Console.WriteLine(X+3);
				break;
			default:
				Console.WriteLine(X*X);
				break;
		}

		var x = 100;
		var y = 200;

		Console.WriteLine("Arithmetic:");
		Console.WriteLine(x + y);
		Console.WriteLine(x - y);
		Console.WriteLine(x * y);
		Console.WriteLine(y / x);
		Console.WriteLine(y - x);
		Console.WriteLine(y % x);
		Console.WriteLine(x < y);
		Console.WriteLine(x <= y);
		Console.WriteLine(x > y);
		Console.WriteLine(x >= y);
		Console.WriteLine(x == y);
		Console.WriteLine(x != y);
		Console.WriteLine(x | y);
		Console.WriteLine(x & y);
		Console.WriteLine(x ^ y);
		Console.WriteLine(x << 2);
		Console.WriteLine(x >> 2);

		X = 100;
		Console.WriteLine(X);

		Action action = Bar;

		action();
	}
}