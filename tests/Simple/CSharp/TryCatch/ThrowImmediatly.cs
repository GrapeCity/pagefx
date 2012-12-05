using System;

class X
{
    static void Test1()
    {
        try
        {
            throw new InvalidCastException();
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}