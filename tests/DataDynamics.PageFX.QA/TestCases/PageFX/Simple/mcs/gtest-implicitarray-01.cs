using System;

public class Test
{
	static void Main ()
	{
        Console.WriteLine(Test1());
        Console.WriteLine("<%END%>");
	}

    private static int Test1()
    {
        Console.WriteLine("----Test1");
        string[] array = new [] { "Foo", "Bar", "Baz" };
        foreach (string s in array)
            if (s.Length != 3)
                return 1;

        string[] s1 = new[] { null, "a", default (string) };
        double[] s2 = new[] { 0, 1.0, 2 };
			
        var a1 = new[] { null, "a", default (string) };
        var a2 = new[] { 0, 1.0, 2 }; 
        var a3 = new[] { new Test (), null }; 
        var a4 = new[,] { { 1, 2, 3 }, { 4, 5, 6 } };
        var a5 = new[] { default (object) };
        var a6 = new[] { new [] { 1, 2, 3 }, new [] { 4, 5, 6 } };
		
        const byte b = 100;
        var a7 = new[] { b, 10, b, 999, b };
		
        return 0;
    }
}
