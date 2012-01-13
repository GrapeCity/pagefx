using System;

class X
{
    static void Test1(object obj)
    {
        try
        {
            string s = ((IConvertible)obj).ToString(null);
            Console.WriteLine(s);
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType().FullName);
        }
    }

    static void Test1()
    {
        Test1(null);
        Test1(new X());
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}