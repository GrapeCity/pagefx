using System;

namespace My.System
{
	static class Test
	{
        static void Test1()
        {
            Console.WriteLine("----Test1");
            var a = new { X = 1 };
            Console.WriteLine(a);
        }

	    public static void Main ()
	    {
	        Test1();
            Console.WriteLine("<%END%>");
	    }
	}
}
