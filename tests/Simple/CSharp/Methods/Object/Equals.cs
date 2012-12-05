using System;

class X
{
    static void Test1()
    {
        Avm.Object obj1 = new Avm.Object();
        Avm.Object obj2 = new Avm.Object();
        Console.WriteLine(Equals(obj1, obj2));
    }

    static void Test2()
    {
        Object obj1 = new Object();
        Avm.Object obj2 = new Avm.Object();
        Console.WriteLine(Equals(obj1, obj2));
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}