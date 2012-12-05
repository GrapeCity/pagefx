using System;

class X
{
    static void Test1()
    {
        try
        {
            Console.WriteLine("aaa"[5]);
        }
        catch (IndexOutOfRangeException)
        {
            Console.WriteLine("ok");
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