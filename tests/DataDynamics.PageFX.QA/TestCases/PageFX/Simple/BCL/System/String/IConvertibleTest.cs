using System;

//public interface IConvertible {
	
//    TypeCode GetTypeCode ();
	
//    bool     ToBoolean  (IFormatProvider provider);
//    byte     ToByte     (IFormatProvider provider);
//    char     ToChar     (IFormatProvider provider);
//    DateTime ToDateTime (IFormatProvider provider);
//    decimal  ToDecimal  (IFormatProvider provider);
//    double   ToDouble   (IFormatProvider provider);
//    short    ToInt16    (IFormatProvider provider);
//    int      ToInt32    (IFormatProvider provider);
//    long     ToInt64    (IFormatProvider provider);
//    sbyte    ToSByte    (IFormatProvider provider);
//    float    ToSingle   (IFormatProvider provider);
//    string   ToString   (IFormatProvider provider);
//    object   ToType     (Type conversionType, IFormatProvider provider);
//    ushort   ToUInt16   (IFormatProvider provider);
//    uint     ToUInt32   (IFormatProvider provider);
//    ulong    ToUInt64   (IFormatProvider provider);
//    }

class X
{
    static void Test_GetTypeCode(IConvertible c)
    {
        Console.WriteLine(c.GetTypeCode());
    }

    static void Test_ToBoolean(IConvertible c)
    {
        Console.WriteLine("ToBoolean");
        try
        {
            Console.WriteLine(c.ToBoolean(null));
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
    }

    static void Test_ToByte(IConvertible c)
    {
        Console.WriteLine("ToByte");
        try
        {
            Console.WriteLine(c.ToByte(null));
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
    }

    static void Test_ToChar(IConvertible c)
    {
        Console.WriteLine("ToChar");
        try
        {
            Console.WriteLine(c.ToChar(null));
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
    }

    static void Test_ToDateTime(IConvertible c)
    {
        Console.WriteLine("ToDateTime");
        try
        {
            Console.WriteLine(c.ToDateTime(null));
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
    }

    static void Test_ToDouble(IConvertible c)
    {
        Console.WriteLine("ToDouble");
        try
        {
            Console.WriteLine(c.ToDouble(null));
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
    }

    static void Test_ToInt16(IConvertible c)
    {
        Console.WriteLine("ToInt16");
        try
        {
            Console.WriteLine(c.ToInt16(null));
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
    }

    static void Test_ToInt32(IConvertible c)
    {
        Console.WriteLine("ToInt32");
        try
        {
            Console.WriteLine(c.ToInt32(null));
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
    }

    static void Test_ToInt64(IConvertible c)
    {
        Console.WriteLine("ToInt64");
        try
        {
            Console.WriteLine(c.ToInt64(null));
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
    }

    static void Test_ToSByte(IConvertible c)
    {
        Console.WriteLine("ToSByte");
        try
        {
            Console.WriteLine(c.ToSByte(null));
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
    }

    static void Test_ToSingle(IConvertible c)
    {
        Console.WriteLine("ToSingle");
        try
        {
            Console.WriteLine(c.ToSingle(null));
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
    }

    static void Test_ToString(IConvertible c)
    {
        Console.WriteLine("ToString");
        try
        {
            Console.WriteLine(c.ToString(null));
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
    }

    static void Test_ToUInt16(IConvertible c)
    {
        Console.WriteLine("ToUInt16");
        try
        {
            Console.WriteLine(c.ToUInt16(null));
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
    }

    static void Test_ToUInt32(IConvertible c)
    {
        Console.WriteLine("ToUInt32");
        try
        {
            Console.WriteLine(c.ToUInt32(null));
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
    }

    static void Test_ToUInt64(IConvertible c)
    {
        Console.WriteLine("ToUInt64");
        try
        {
            Console.WriteLine(c.ToUInt64(null));
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
    }

    static void TestIConvertible(IConvertible c)
    {
        Console.WriteLine("----------");
        Console.WriteLine(c);
        Test_GetTypeCode(c);
        Test_ToBoolean(c);
        Test_ToByte(c);
        Test_ToChar(c);
        //Test_ToDateTime(c);
        Test_ToDouble(c);
        Test_ToInt16(c);
        Test_ToInt32(c);
        Test_ToInt64(c);
        Test_ToSByte(c);
        Test_ToSingle(c);
        Test_ToString(c);
        Test_ToUInt16(c);
        Test_ToUInt32(c);
        Test_ToUInt64(c);
    }

    static void TestAll()
    {
        TestIConvertible("aaa");
        TestIConvertible("10");
        TestIConvertible("true");
        TestIConvertible("false");
        TestIConvertible("27/06/2008");
    }

    static void Test1(IConvertible c)
    {
        Console.WriteLine(c.ToByte(null));
    }

    static void Test1()
    {
        Test1("10");
    }

    static void Main()
    {
        TestAll();
        Test1();
        Console.WriteLine("<%END%>");
    }
}