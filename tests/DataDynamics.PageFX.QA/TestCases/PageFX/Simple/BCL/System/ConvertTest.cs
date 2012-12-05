using System;

class X
{
    static void TestSByte()
    {
        Console.WriteLine("--- TestSByte");
        int i = 123;
        sbyte v = Convert.ToSByte((object)i);
        Console.WriteLine(v);
    }

    static void Test1(short value)
    {
        ulong v = (ulong)value;
        Console.WriteLine((v & 0x07).GetType());
        Console.WriteLine((v & 0x07));
    }

    static void Test1()
    {
        Test1(Int16.MinValue);
        Test1(7);
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        string s = Convert.ToString(Int16.MinValue, 8);
        Console.WriteLine(s);
    }

    static void Main()
    {
        TestSByte();
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}