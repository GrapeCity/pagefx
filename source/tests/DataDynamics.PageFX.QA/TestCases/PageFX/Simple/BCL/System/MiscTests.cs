using System;

class Program
{
    static void TestCompareTo()
    {
        {
            Boolean t = true, f = false;
            Console.WriteLine(f.CompareTo(t) < 0);
        }
    }

    static void TestUInt64_ToStringX()
    {
        ulong v = new ulong();
        Console.WriteLine(v.ToString("x16"));
        Console.WriteLine(v.ToString("X16"));
    }

    static void TestStringFormat()
    {
        Console.WriteLine("--- TestStringFormat");
        try
        {
            Console.WriteLine(string.Format("Abc {0}" + 123));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void Main()
    {
        TestCompareTo();
        TestUInt64_ToStringX();
        TestStringFormat();
        Console.WriteLine("<%END%>");
    }
}