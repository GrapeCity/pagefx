using System;
using System.Collections;

class X
{
    static void Test1()
    {
        Hashtable h = new Hashtable();
        h['a'] = 1;
        h['b'] = 2;
        object[] o = new object[2];
        h.CopyTo(o, 0);
    }

    static void Test2()
    {
        try
        {
            Hashtable h = new Hashtable();
            h["blue"] = 1;
            h["green"] = 2;
            h["red"] = 3;
            Char[] o = new Char[3];
            h.CopyTo(o, 0);
        }
        catch (InvalidCastException)
        {
            Console.WriteLine("ok");
            return;
        }
        Console.WriteLine("invalid cast error not thrown");
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}