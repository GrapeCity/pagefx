using System;

class X
{
    static void Test1()
    {
        Avm.Object obj1 = new Avm.Object();
        Avm.Object obj2 = new Avm.Object();
        var hc1 = obj1.GetHashCode();
        var hc2 = obj2.GetHashCode();
        Console.WriteLine(hc1 != hc2);
        hc1 = obj1.GetHashCode();
        hc2 = obj2.GetHashCode();
        Console.WriteLine(hc1 != hc2);
    }

    static void Main()
    {
        Console.WriteLine("----Test1");
        Console.WriteLine(true);
        Console.WriteLine(123);
        Test1();
        
        Console.WriteLine("<%END%>");
    }
}