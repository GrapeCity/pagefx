using System;

class Test
{
	static void f(object o)
	{
		Console.WriteLine(o);
	}

	static void TestToString()
	{
		Console.WriteLine(true.ToString());
		Console.WriteLine(false.ToString());

		f(true);
		f(false);
	}

    public static void Main()
    {
	    TestToString();

	    Console.WriteLine(true.Equals(null));
	    Console.WriteLine(false.Equals(null));

        Console.WriteLine("<%END%>");
    }
}