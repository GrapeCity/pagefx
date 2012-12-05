using System;

class X
{
    static void Test1()
    {
        Type type = Type.GetType("System.String");
        Console.WriteLine(type.FullName);

        type = Type.GetType("System.Int32");
        Console.WriteLine(type.FullName);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}