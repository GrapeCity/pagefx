using System;

class Test
{
    static void Main()
    {
		Console.WriteLine(new string('a', 5));
		Console.WriteLine(new string(new char[]{'a', 'b', 'c'}));
		Console.WriteLine(new string(new char[]{'a', 'b', 'c'}, 0, 3));
		Console.WriteLine(new string(new char[]{'a', 'b', 'c'}, 1, 2));
        Console.WriteLine("<%END%>");
    }
}