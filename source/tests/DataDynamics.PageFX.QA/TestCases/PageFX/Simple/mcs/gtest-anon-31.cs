using System;

public static class A
{
	public static void Fail<X> ()
	{
		EventHandler t = delegate
        {
            Console.WriteLine("1");
            t = delegate 
            {
                Console.WriteLine("2");
                X foo;
                Console.WriteLine(typeof(X));
            };
            t(null, null);
            Console.WriteLine("3");
		};
        t(null, null);
	}

    public static void Test1()
    {
        Console.WriteLine("----Test1");
        try
        {
            Fail<int>();
            Console.WriteLine("ok");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

	public static void Main ()
	{
        Test1();
        Console.WriteLine("<%END%>");
	}
} 
