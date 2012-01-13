using System;
using Avm;
using T = System.UInt16;

class X
{
    static void Main()
    {
        Vector<T> v = new Vector<T>();
        v.push(10, 20, 30);
        for (int i = 0; i < (int)v.length; ++i)
            Console.WriteLine(v[i]);
        Console.WriteLine(v.pop());
        Console.WriteLine(v.shift());
        Console.WriteLine(v.length);
        Console.WriteLine(v[0]);
        Console.WriteLine("<%END%>");
    }
}