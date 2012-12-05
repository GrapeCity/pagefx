using System;

class Test
{
    public static void Main()
    {
	    char c = 'a';
	    var s = ((object)c).ToString();
	    Console.WriteLine(s);
        Console.WriteLine("<%END%>");
    }
}