using System;

class X
{
    static void Test1()
    {
        try
        {
            Avm.Object obj = new Avm.Object();
            Type type = obj.GetType();
            Console.WriteLine(type.FullName);
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