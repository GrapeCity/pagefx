using System;

class X
{
    static void FromSingle(float v)
    {
        try
        {
            decimal d = (decimal)v;
            Console.WriteLine(d);
        }
        catch (OverflowException)
        {
            Console.WriteLine("overflow");
        }
    }

    static void FromSingle()
    {
        FromSingle(float.MinValue);
        FromSingle(float.MaxValue);
        FromSingle(1.2f);
    }

    static void Main()
    {
        FromSingle();
        Console.WriteLine("<%END%>");
    }
}