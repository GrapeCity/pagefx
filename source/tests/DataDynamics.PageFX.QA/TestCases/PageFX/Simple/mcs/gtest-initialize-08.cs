using System;
//using System.Collections.Generic;

class T
{
	public X[] x;
}

class X
{
	public Z[] Prop { get; set; }
}

class Z
{
}

class Test
{
	T t = new T () { x = new X [] { 
		new X () {
			Prop = new Z[] { new Z (), new Z () }
		}
	}};
	
	public Test (string s)
	{
	}
	
	public Test (int i)
	{
	}
}

public class C
{
	public static void Main ()
	{
        Console.WriteLine(Test1());
        Console.WriteLine("<%END%>");
	}

    private static int Test1()
    {
        Console.WriteLine("----Test1");
        new Test ("2");
        return 0;
    }
}
