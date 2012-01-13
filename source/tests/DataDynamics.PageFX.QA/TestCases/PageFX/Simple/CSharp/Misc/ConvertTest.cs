using System;

class X
{
    static void TestToByte()
    {
        bool boolTrue = true;
        byte v = Convert.ToByte((object)boolTrue);
        Console.WriteLine(v);
    }

    static void TestToUInt16(sbyte v)
    {
        Console.WriteLine(v);
        try
        {
            ushort v2 = Convert.ToUInt16(v);
            Console.WriteLine(v2);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void TestToUInt16()
    {
        TestToUInt16(sbyte.MinValue);
    }

    static void Main()
    {
        TestToByte();
        TestToUInt16();
        Console.WriteLine("<%END%>");
    }
}