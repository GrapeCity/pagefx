using System;

enum E { A, B, C, D }

class Program
{
    static void Foo(E v)
    {
	    switch (v)
	    {
			case E.A:
			    Console.WriteLine("A");
				break;
			case E.B:
				Console.WriteLine("B");
				break;
			case E.C:
				Console.WriteLine("C");
				break;
			case E.D:
				Console.WriteLine("D");
				break;
	    }
    }

    static void Main()
    {
	    Foo(E.A);
	    Foo(E.B);
	    Foo(E.C);
	    Foo(E.D);
        Console.WriteLine("<%END%>");
    }
}