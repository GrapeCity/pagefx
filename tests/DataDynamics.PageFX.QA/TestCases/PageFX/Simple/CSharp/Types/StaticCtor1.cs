using System;

class StaticCtor1
{
    static StaticCtor1()
    {
        Console.WriteLine("static ctor");
    }

    static void Main()
    {
        Console.WriteLine("Simple Static Constructor Example...");
        Console.WriteLine("<%END%>");
    }
}