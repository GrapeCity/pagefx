using System;

class CallRedirect
{
    static void Main()
    {
        Console.WriteLine(Math.Cos(0.0));
        Console.WriteLine(Math.Sin(0.0));
        Console.WriteLine("<%END%>");
    }
}