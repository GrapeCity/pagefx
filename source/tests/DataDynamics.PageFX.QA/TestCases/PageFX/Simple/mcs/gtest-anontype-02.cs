
// Tests anonymous types initialized with local variables
using System;
using System.Collections;

public class Test
{
	static object TestA (string s)
	{
		return new { s };
	}
	
    static int Test1()
    {
        Console.WriteLine("----Test1");
        string Foo = "Bar";
        int Baz = 42;
        var v = new { Foo, Baz };

        if (v.Foo != "Bar")
            return 1;
        if (v.Baz != 42)
            return 2;

        if (!TestA("foo").Equals(new { s = "foo" }))
            return 3;

        Console.WriteLine("OK");
        return 0;
    }

	static void Main ()
	{
	    Console.WriteLine(Test1());
        Console.WriteLine("<%END%>");
	}
}
