using System;

class X
{
    static void PrintDecimal(decimal d)
    {
        Console.WriteLine(d);
    }

    static void Test1()
    {
        Int64 v = -1;
        PrintDecimal((decimal)v);
    }

    static void Test2()
    {
        Decimal d = 254.9m;
        Console.WriteLine((d as IConvertible).ToInt64(null));

        d = -d;

        Int64 v64 = Decimal.ToInt64(d);
        Console.WriteLine(v64);
        Console.WriteLine("{0:X}", v64);

        int v32 = (v64 as IConvertible).ToInt32(null);
        Console.WriteLine(v32);

        short v16 = (v64 as IConvertible).ToInt16(null);
        Console.WriteLine(v16);
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}