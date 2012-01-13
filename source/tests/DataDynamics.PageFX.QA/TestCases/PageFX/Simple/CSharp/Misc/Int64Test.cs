using System;

class Program
{
    static ulong GetULong(int i)
    {
        return (ulong)(i * i + 45643271);
    }

    static long GetLong(int i)
    {
        return (long)GetULong(i);
    }

    static void TestBinary(long a, long b)
    {
        Console.WriteLine("--- TestBinary");
        Console.WriteLine(a + b);
        Console.WriteLine(a - b);
        Console.WriteLine(a * b);
        Console.WriteLine(a / b);
        Console.WriteLine(a % b);

        //Bitwise operatios
        Console.WriteLine(a | b);
        Console.WriteLine(a & b);
        Console.WriteLine(a ^ b);

        //Comparison operations
        Console.WriteLine(a == b);
        Console.WriteLine(a != b);
        Console.WriteLine(a < b);
        Console.WriteLine(a <= b);
        Console.WriteLine(a > b);
        Console.WriteLine(a >= b);
    }

    static void TestBinary(ulong a, ulong b)
    {
        Console.WriteLine("--- TestBinary");
        Console.WriteLine(a + b);
        Console.WriteLine(a - b);
        Console.WriteLine(a * b);
        Console.WriteLine(a / b);
        Console.WriteLine(a % b);

        //Bitwise operatios
        Console.WriteLine(a | b);
        Console.WriteLine(a & b);
        Console.WriteLine(a ^ b);

        //Comparison operations
        Console.WriteLine(a == b);
        Console.WriteLine(a != b);
        Console.WriteLine(a < b);
        Console.WriteLine(a <= b);
        Console.WriteLine(a > b);
        Console.WriteLine(a >= b);
    }

    static void TestUnary(long a)
    {
        Console.WriteLine("--- TestUnary");
        Console.WriteLine(-a);
        Console.WriteLine(~a);
    }

    static void TestUnary(ulong a)
    {
        Console.WriteLine("--- TestUnary");
        Console.WriteLine(~a);
    }

    static void TestShifts(long a, int n)
    {
        Console.WriteLine("--- TestShifts {0}", n);
        Console.WriteLine(a << n);
        Console.WriteLine(a >> n);
    }

    static void TestShifts(ulong a, int n)
    {
        Console.WriteLine("--- TestShifts {0}", n);
        Console.WriteLine(a << n);
        Console.WriteLine(a >> n);
    }

    static void TestCasts(long a)
    {
        Console.WriteLine("--- TestCasts");
        Console.WriteLine("sbyte: ");
        Console.WriteLine((sbyte)a);
        Console.WriteLine("byte: ");
        Console.WriteLine((byte)a);
        Console.WriteLine("short: ");
        Console.WriteLine((short)a);
        Console.WriteLine("ushort: ");
        Console.WriteLine((ushort)a);
        Console.WriteLine("int: ");
        Console.WriteLine((int)a);
        Console.WriteLine("uint: ");
        Console.WriteLine((uint)a);
        Console.WriteLine("ulong: ");
        Console.WriteLine((ulong)a);
        //Console.WriteLine("float: ");
        //Console.WriteLine((float)a);
        Console.WriteLine("double: ");
        Console.WriteLine((double)a);
    }

    static void TestCasts(ulong a)
    {
        Console.WriteLine("--- TestCasts");
        Console.WriteLine("sbyte: ");
        Console.WriteLine((sbyte)a);
        Console.WriteLine("byte: ");
        Console.WriteLine((byte)a);
        Console.WriteLine("short: ");
        Console.WriteLine((short)a);
        Console.WriteLine("ushort: ");
        Console.WriteLine((ushort)a);
        Console.WriteLine("int: ");
        Console.WriteLine((int)a);
        Console.WriteLine("uint: ");
        Console.WriteLine((uint)a);
        Console.WriteLine("long: ");
        Console.WriteLine((long)a);
        //Console.WriteLine("float: ");
        //Console.WriteLine((float)a);
        Console.WriteLine("double: ");
        Console.WriteLine((double)a);
    }

    static void Test(long a, long b)
    {
        Console.WriteLine("a = {0}, b = {1}", a, b);
        TestBinary(a, b);
        TestUnary(a);
        TestShifts(a, 10);
        TestShifts(a, -10);
        TestCasts(a);
    }

    static void Test(ulong a, ulong b)
    {
        Console.WriteLine("a = {0}, b = {1}", a, b);
        TestBinary(a, b);
        TestUnary(a);
        TestShifts(a, 10);
        TestShifts(a, -10);
        TestCasts(a);
    }

    static void TestI64()
    {
        Console.WriteLine("--- TestInt64");
        for (int i = 0; i < 10; ++i)
        {
            Console.WriteLine("#" + (i + 1));
            long a = GetLong(i);
            long b = GetLong(i);
            Test(a, b);
        }
    }

    static void TestU64()
    {
        Console.WriteLine("--- TestUInt64");
        for (int i = 0; i < 10; ++i)
        {
            Console.WriteLine("#" + (i + 1));
            ulong a = GetULong(i);
            ulong b = GetULong(i);
            Test(a, b);
        }
    }

    static void Main()
    {
        TestI64();
        TestU64();
        Console.WriteLine("<%END%>");
    }
}