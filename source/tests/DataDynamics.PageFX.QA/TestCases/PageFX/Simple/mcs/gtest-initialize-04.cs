

using System;
using System.Collections.Generic;

public class C
{
	static readonly List<int> values = new List<int> { 1, 2, 3 };
	
	public static void Main ()
	{
	    Console.WriteLine(Test1());
	    Console.WriteLine("<%END%>");
	}

    private static int Test1()
    {
        Console.WriteLine("----Test1");
        if (values.Count != 3)
            return 1;
		
        Console.WriteLine ("OK");
        return 0;
    }
}
