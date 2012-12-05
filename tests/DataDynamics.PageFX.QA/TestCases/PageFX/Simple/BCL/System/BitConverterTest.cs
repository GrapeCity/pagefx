using System;

class X
{
    static void TestGetBytes(double v)
    {
	    Console.WriteLine("---");
	    var b = BitConverter.GetBytes(v);
	    for (int i = 0; i < b.Length; i++)
	    {
		    Console.WriteLine(b[i]);
	    }
    }

	static void TestGetBytes(float v)
	{
		Console.WriteLine("---");
		var b = BitConverter.GetBytes(v);
		for (int i = 0; i < b.Length; i++)
		{
			Console.WriteLine(b[i]);
		}
	}

    static void Main()
    {
		TestGetBytes(0.5);
		TestGetBytes(3.14);
		TestGetBytes(0.5f);
		TestGetBytes(3.14f);
        Console.WriteLine("<%END%>");
    }
}