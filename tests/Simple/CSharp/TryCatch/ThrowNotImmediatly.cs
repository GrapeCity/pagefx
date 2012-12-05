using System;

class X
{
    static void Throw()
    {
        throw new InvalidCastException();
    }

    static void Test1()
    {
        try
        {
            Throw();
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