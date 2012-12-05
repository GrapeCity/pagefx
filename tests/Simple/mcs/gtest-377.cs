using System;

public static class D
{
	static bool? debugging = null;
	static int? extra = 0;

	public static void Main ()
	{
		debugging |= true;
	    Console.WriteLine(debugging);
		
		extra |= 55;
	    Console.WriteLine(extra);
        Console.WriteLine("<%END%>");
	}
}
