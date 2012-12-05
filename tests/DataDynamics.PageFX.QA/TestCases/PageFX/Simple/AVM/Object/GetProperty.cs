using System;
using Avm;

class X
{
    static void Test1()
    {
        Avm.Object obj = avm.NewObject("a", "1", "b", "2");
        Console.WriteLine(obj.GetProperty("a"));
        Console.WriteLine(obj.GetProperty("b"));
    }

    static void Test2()
    {
        Avm.Object obj = avm.NewObject("a", "1", "b", "2");
        Console.WriteLine(obj.GetProperty("", "a"));
        Console.WriteLine(obj.GetProperty("", "b"));
    }

    static void Test3()
    {
        Namespace ns = new Namespace("");
        Avm.Object obj = avm.NewObject("a", "1", "b", "2");
        Console.WriteLine(obj.GetProperty(ns, "a"));
        Console.WriteLine(obj.GetProperty(ns, "b"));
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Console.WriteLine("<%END%>");
    }
}