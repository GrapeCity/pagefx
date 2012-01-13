

// "cast by example" test

using System;

class CastByExample
{
	static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
	}

    private static void Test1()
    {
        object o = new { Foo = "Data" };
        // Cast object to anonymous type
        var typed = Cast(o, new { Foo = "" });
    }

    static T Cast<T>(object obj, T type)
	{
		return (T)obj;
	}
}
