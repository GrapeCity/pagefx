

using System;
using System.Collections;

public class Test
{
	static void Main ()
    {
	    Console.WriteLine(Test1());
        Console.WriteLine("<%END%>");
    }

    private static int Test1()
    {
        var v1 = new {  };
        var v2 = new {  };
		
        if (v1.GetType () != v2.GetType ())
            return 1;
			
        if (!v1.Equals (v2))
            return 2;
			
        Console.WriteLine (v1);
        Console.WriteLine (v2);
        return 0;
    }
}

