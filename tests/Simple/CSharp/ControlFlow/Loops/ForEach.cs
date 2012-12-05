using System;

class ForEach
{
    public static void Main()
    {
        string s = "Hello, World!";
        foreach (char c in s)
        {
            Console.WriteLine(c);
            Console.WriteLine("<%END%>");
        }
    }
}