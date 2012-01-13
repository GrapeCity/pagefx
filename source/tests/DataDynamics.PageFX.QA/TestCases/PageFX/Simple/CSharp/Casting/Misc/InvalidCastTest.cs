using System;

class X
{
    static byte ToByte(object value)
    {
        if (value == null)
            throw new ArgumentNullException("value");
        return ((IConvertible)value).ToByte(null);
    }

    static void Test1()
    {
        try
        {
            ToByte(new X());
            Console.WriteLine("no expected InvalidCastException");
        }
        catch(InvalidCastException)
        {
            Console.WriteLine("ok");
        }
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}