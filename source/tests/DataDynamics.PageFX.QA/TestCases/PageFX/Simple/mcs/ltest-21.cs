using System;

class Program
{
	static void Foo (Action<string> a)
	{
		a ("action");
	}

	static T Foo<T> (Func<string, T> f)
	{
		return f ("function");
	}

	static string Bar ()
	{
		return Foo (str => str.ToLower ());
	}

	static int Test1()
	{
	    Console.WriteLine("----Test1");
        var str = Foo(s => s);
        Console.WriteLine(str);
        if (str != "function")
            return 1;
        Foo(s => Console.WriteLine(s));
        return 0;
	}
    
    static void Main ()
	{
        Console.WriteLine(Test1());
        Console.WriteLine("<%END%>");
	}
}
