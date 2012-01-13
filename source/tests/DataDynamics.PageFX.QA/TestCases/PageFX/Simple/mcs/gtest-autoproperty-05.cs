using System;

partial class Test
{
}

abstract partial class Test
{
	public string X { get; set; }
}

class M
{
	public static void Main ()
	{
        Console.WriteLine("Hello World!");
        Console.WriteLine("<%END%>");
    }
}
