using System;

public class Test
{
	delegate void D ();
	
	public static void Main ()
	{
        try
        {
            new Test().Test_3<int>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
		
        Console.WriteLine("<%END%>");
	}
	
	public void Test_3<T> () where T : struct
	{
		D d = delegate () {
			T? tt = null;
            Console.WriteLine("ok");
		};
		d ();
	}
}
