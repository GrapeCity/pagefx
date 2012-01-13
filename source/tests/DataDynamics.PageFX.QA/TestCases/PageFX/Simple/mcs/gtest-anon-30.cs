using System;
public delegate void Simple ();

public delegate Simple Foo ();

class X
{
	public void Hello<U> (U u)
	{
        Console.WriteLine("Hello" + typeof(U));
	}

	public void Test<T> (T t)
	{
		{
			T u = t;
			Hello (u);
			Foo foo = delegate {
				T v = u;
				Hello (u);
				return delegate {
					Hello (u);
					Hello (v);
				};
			};
            (foo())();
		}
	}

    static void Test1()
    {
        Console.WriteLine("----Test1");
        try
        {
            X x = new X();
            x.Test(5);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());   
        }

    }

    static void Main ()
	{
        Test1();
        Console.WriteLine("<%END%>");
	}
} 
