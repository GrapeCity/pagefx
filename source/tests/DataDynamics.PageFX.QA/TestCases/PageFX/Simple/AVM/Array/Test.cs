using System;

class X
{
    static void Test1()
    {
        Avm.Array arr = new Avm.Array();
        arr.push(10);
        arr.push(20);
        arr.push(30);
        int v = (int)arr[0];
        Console.WriteLine(v);
        v = (int)arr[1];
        Console.WriteLine(v);
        v = (int)arr[2];
        Console.WriteLine(v);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
