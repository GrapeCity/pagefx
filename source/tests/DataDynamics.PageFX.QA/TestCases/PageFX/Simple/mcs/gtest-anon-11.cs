using System;

public delegate void Foo ();

public class Test<R>
{
	public void World<S,T> (S s, T t)
		where S : X
		where T : S
	{
        Console.WriteLine(s);
        Console.WriteLine(t);
    }

	public void Hello<U,V> (U u, V v)
		where U : X
		where V : U
	{
        Console.WriteLine(u);
        Console.WriteLine(v);
		Foo foo = delegate {
			World (u, v);
		};
        foo();
	}

    public override string ToString()
    {
        return "Test";
    }
}

public class X
{
	public static void Main ()
	{
		X x = new X ();
		Test<int> test = new Test<int> ();
		test.Hello (x, x);
        Console.WriteLine("<%END%>");
	}

    public override string ToString()
    {
        return "X";
    }
}
