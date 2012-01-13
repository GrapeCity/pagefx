using System;

internal class Test
{
    static bool Fail()
    {
        return true;
    }

    static void Main()
    {
        Console.WriteLine("Hello!");
        Console.WriteLine(Fail() ? "fail" : "success");
        Console.WriteLine("<%END%>");
    }
}