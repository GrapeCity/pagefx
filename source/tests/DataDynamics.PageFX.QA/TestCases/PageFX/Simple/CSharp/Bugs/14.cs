using System;

class X
{
    static uint GetU()
    {
        return 100;
    }

    static long GetPos()
    {
        return GetU();
    }

    static void Test1()
    {
        Console.WriteLine("--Test1");
        Console.WriteLine(GetPos());
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}