using System;

class X
{
    static void Test1()
    {
        Console.WriteLine("--Test1");
        Guid g0 = new Guid("000102030405060708090a0b0c0d0e0f");
        Console.WriteLine(g0);
    }

    static void Test2()
    {
        Console.WriteLine("--Test2");
        Guid g0 = new Guid(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
        Console.WriteLine(g0);
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}