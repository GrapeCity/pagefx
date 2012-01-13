using System;
using System.Collections.Generic;

public abstract class TestClass
{
	public abstract void ToString (object obj);

	public IEnumerable<object> TestEnumerator ()
	{
		ToString (null);
		yield break;
	}

	public void Test ()
	{
		ToString (null);
	}
}

class A : TestClass
{
    public override void ToString(object obj)
    {
        Console.WriteLine(obj);
    }
}

class M
{
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        A a = new A();
        foreach (var v in a)
            Console.WriteLine(v);
    }

	public static void Main ()
	{
        Test1();
        Console.WriteLine("<%END%>");
	}
}