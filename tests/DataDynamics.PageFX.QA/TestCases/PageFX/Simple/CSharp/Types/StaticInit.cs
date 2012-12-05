using System;

class StaticInit1
{
    private static int s_int = 10;
    private static string s_str = "sss";

    static void Main()
    {
        Console.WriteLine(s_int);
        Console.WriteLine(s_str);
        Console.WriteLine("<%END%>");
    }
}