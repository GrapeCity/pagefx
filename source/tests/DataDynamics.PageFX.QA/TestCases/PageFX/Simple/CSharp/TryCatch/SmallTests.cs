using System;

class X
{
    static void Test1()
    {
        string s1 = "original";

        try
        {
            s1.EndsWith(null);
            Console.WriteLine("error");
        }
        catch (ArgumentNullException)
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