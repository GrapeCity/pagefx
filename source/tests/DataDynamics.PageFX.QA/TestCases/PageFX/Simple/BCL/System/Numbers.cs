using System;

class X
{
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        int i = 10;
        IComparable c = i;
        Console.WriteLine(c.CompareTo(10));
    }

    private const UInt32 MyUInt32_3 = 4294967295;
    private const string MyString3 = "4294967295";

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        uint v = UInt32.Parse(MyString3);
        Console.WriteLine(v);
        Console.WriteLine(MyUInt32_3 == v);
    }

    static void Test3(sbyte v)
    {
        Console.WriteLine(v == sbyte.MinValue);
        Console.WriteLine(v);
    }

    static void Test3()
    {
        Console.WriteLine("--- Test3");
        sbyte v = sbyte.MinValue;
        Console.WriteLine(v);
        Console.WriteLine(v == sbyte.MinValue);
        Test3(v);
    }

    static void Abs(sbyte value)
    {
        if (value == SByte.MinValue)
        {
            Console.WriteLine("overflow");
        }
        else
        {
            Console.WriteLine("ok");
        }
    }

    static void Test4()
    {
        Console.WriteLine("--- Test4");
        sbyte a = -9;
        sbyte b = sbyte.MinValue;
        Abs(a);
        Abs(b);
    }

    static void Test5()
    {
        Console.WriteLine("--- Test5");
        int i = 123;
        string s = i.ToString("####");
        Console.WriteLine(s);
    }

    static void Test6()
    {
        Console.WriteLine("--- Test6");
        {
            ulong v = 0xfffu;
            Console.WriteLine(v);
            Console.WriteLine("{0}", v);
        }
        {
            long v = 0xfff;
            Console.WriteLine(v);
            Console.WriteLine("{0}", v);
        }
    }

    private const UInt64 MyUInt64_2 = 0;

    static void Test7()
    {
        Console.WriteLine("--- Test7");
        try
        {
            MyUInt64_2.CompareTo((object)(Int16)100);
            Console.WriteLine("Should raise a System.ArgumentException");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void Test8()
    {
#if NOT_PFX
        double v = (double)Int64.MaxValue + 1;
        Console.WriteLine(v);
#endif
    }

    static void Test9()
    {
        Console.WriteLine("--- Test9");
        uint a = 400;
        Console.WriteLine(-a);
    }

    static void Test10()
    {
        Console.WriteLine("--- Test10");
        short v1 = Int16.MinValue;
        ulong v = (ulong)v1;
        Console.WriteLine("{0:X}", v);
    }

    static void Test11()
    {
        Console.WriteLine("--- Test11");
        ulong b = 20;
        Console.WriteLine((b & 10).GetType());
        Console.WriteLine(b & 10);
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Test4();
        Test5();
        Test6();
        Test7();
        Test8();
        Test9();
        Test10();
        Test11();
        Console.WriteLine("<%END%>");
    }
}