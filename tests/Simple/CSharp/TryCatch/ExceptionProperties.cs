using System;

class Test
{
    static void Fail()
    {
        throw new Exception("FAIL!");
    }

    static void Main()
    {
        try
        {
            Fail();
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.Message);
            Console.WriteLine(exc.StackTrace != "");
        }

        Console.WriteLine("<%END%>");
    }
}