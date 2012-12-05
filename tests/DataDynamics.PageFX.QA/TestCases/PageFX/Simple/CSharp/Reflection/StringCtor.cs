using System;

class Test
{
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        string s = new string('a', 3);
        Console.WriteLine(s);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}