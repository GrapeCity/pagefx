using System;

public class Test
{
	delegate void D ();
	
	public static void Main ()
	{
		Test t = new Test ();
		t.Test_1 (t);
		t.Test_2<int> (1);
        Console.WriteLine("<%END%>");
	}

	public void Test_1<T> (T t) where T : Test
	{
		D d = delegate () {
			Action<T> d2 = new Action<T> (t.Test_1);
		};
		d ();
	}
	
	public void Test_2<T> (T? t) where T : struct
	{
		bool b;
		T t2;
		D d = delegate () {
			b = t == null;
			t2 = (T)t;
			t2 = t ?? default (T);
		};
		
		d ();
	}
}