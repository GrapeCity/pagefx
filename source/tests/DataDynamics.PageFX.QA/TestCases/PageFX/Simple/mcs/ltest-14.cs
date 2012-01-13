// identity functions
using System;

class Test {
	public static void Main ()
	{
        Console.WriteLine("<%END%>");
	}

	static void Foo<T> ()
	{
		Func<T,T> f = n => n;
	}
}
