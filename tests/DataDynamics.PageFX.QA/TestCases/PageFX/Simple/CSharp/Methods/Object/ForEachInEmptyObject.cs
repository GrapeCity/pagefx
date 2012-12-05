using System;

class X
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        try
        {
            PageFX.Util.TestForEachInEmptyObject();
            Console.WriteLine("ok");
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc);
            Console.WriteLine("fail");
        }
    }

    static void Main()
    {
        Test1();        
        Console.WriteLine("<%END%>");
    }
}