using System;

class Program
{
	static void Test()
	{
		try
		{
			try
			{
				Console.WriteLine("step1");
			}
			finally
			{
				Console.WriteLine("finally1");
			}

			try
			{
				Console.WriteLine("step2");
				var e = new Exception();
				Console.WriteLine("Throw!");
				throw e;
			}
			finally
			{
				Console.WriteLine("finally2");
			}

			Console.WriteLine("after finally2");
		}
		finally
		{
			Console.WriteLine("finally3");
		}
	}
    
    static void Main()
    {
    	var program = new Program();

    	try
    	{
    		Test();
    	}
    	catch (Exception e)
    	{
    		Console.WriteLine("OK!");
    	}		
        
        Console.WriteLine("<%END%>");
    }
}