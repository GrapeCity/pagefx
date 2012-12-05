using System;

namespace N
{
	public partial class C
	{
	    static private double e;
        static partial void Foo();
	
		public static void Main ()
		{
            Foo();
            Console.WriteLine(e);
            Console.WriteLine("<%END%>");
		}
	}
}

namespace N
{
	public partial class C
	{
        static partial void Foo ()
		{
            e = 2.718281828;
		}
	}
}