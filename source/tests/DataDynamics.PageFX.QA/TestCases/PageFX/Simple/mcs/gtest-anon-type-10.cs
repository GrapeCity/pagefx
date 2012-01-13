using System;

class A
{
	public int X {
		get { 
			return 100;
		}
	}
}

class B : A
{
	static int Test1()
	{
	    Console.WriteLine("----Test1");
        return new B().Test();
	}

    public static void Main ()
    {
        Console.WriteLine(Test1());
        Console.WriteLine("<%END%>");
	}
	
	int Test ()
	{
		var x = new { base.X };
		return x.X == 100 ? 0 : 1;
	}
}
