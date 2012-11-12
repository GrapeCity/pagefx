using System;

class X
{
    static void Test1()
    {
	    Console.WriteLine("# Test1");

        try
        {
            "abc".EndsWith(null);
            Console.WriteLine("error");
        }
        catch (ArgumentNullException)
        {
            Console.WriteLine("ok");
        }
    }

	static void Test2()
	{
		Console.WriteLine("# Test2");

		try
		{
			"abc".EndsWith(null);
			Console.WriteLine("error");
		}
		catch (ArgumentNullException)
		{
			Console.WriteLine("ok");
		}
		finally
		{
			Console.WriteLine("finally");
		}
	}

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}