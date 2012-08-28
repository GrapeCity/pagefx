using System;

class Program
{
    static void TestCompare()
    {
        ulong v = 11UL;
        Console.WriteLine(v < 10UL);
    }

    static void TestCast()
    {
        {
            long a = -11L;
            ulong b = (ulong)a;
            Console.WriteLine(a);
            Console.WriteLine(b);
        }
        {
            ulong a = ulong.MaxValue;
            long b = (long)a;
            Console.WriteLine(a);
            Console.WriteLine(b);
        }
    }

    static void TestMulDiv()
    {
        {
            ulong a = 11UL;
            ulong b = 10UL;
            Console.WriteLine(a * b);
            Console.WriteLine(a / b);
        }
        {
            long a = 11L;
            long b = 10L;
            Console.WriteLine(a * b);
            Console.WriteLine(a / b);
        }
    }

    static void Test1()
    {
        long v = -11L;
        Console.WriteLine(v);
    }

    static void Test2()
    {
        ulong v = 11UL;
        Console.WriteLine(v);
    }

    static void Test3()
    {
        long v = 45643271;
        Console.WriteLine((ulong)v);
    }

    static void Test4()
    {
        long v = 45643271;
        Console.WriteLine(v);
        v = v >> 10;
        Console.WriteLine(v);
    }

    const long minL = long.MinValue;
    const long maxL = long.MaxValue;

    const ulong minUL = ulong.MinValue;
    const ulong maxUL = ulong.MaxValue;

    static void Test5()
    {
        Console.WriteLine(minL);
        Console.WriteLine(maxL);
        Console.WriteLine(minUL);
        Console.WriteLine(maxUL);
    }

    static void Main()
    {
        TestCompare();
        TestCast();
        TestMulDiv();
        Test1();
        Test2();
        Test3();
        Test4();
        Test5();
        Console.WriteLine("<%END%>");
    }
}