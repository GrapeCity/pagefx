using System;
using System.Collections;

internal class Test
{
    static void Test1()
    {
        Console.WriteLine("--Test1");
        Test1(100);
    }

    static void Test1(double n)
    {
        long bits = BitConverter.DoubleToInt64Bits(n);
        Console.WriteLine(bits);
        bits &= long.MaxValue;
        Console.WriteLine(bits);
        double n2 = BitConverter.Int64BitsToDouble(bits);
        Console.WriteLine(n2.ToString());
        byte[] b = BitConverter.GetBytes(n);
        for (int i = 0; i < b.Length; ++i)
            Console.WriteLine(b[i]);
    }

    static void Test2()
    {
        Console.WriteLine("--Test2");
        Test2(100.5);
    }

    static void Test2(double n)
    {
        Console.WriteLine(n.ToString("G", null));
    }

    static void Main()
    {
        Console.WriteLine("Hello!");
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}