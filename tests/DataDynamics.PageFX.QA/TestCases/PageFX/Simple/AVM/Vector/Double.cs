using System;
using Avm;
using T = System.Double;

class X
{
    static void Main()
    {
        Vector<T> v = new Vector<T>();
        v.push(-1, 0, 1);
        for (int i = 0; i < (int)v.length; ++i)
            Console.WriteLine(v[i]);
        Console.WriteLine(v.pop());
        Console.WriteLine(v.shift());
        Console.WriteLine(v.length);
        Console.WriteLine(v[0]);
        Console.WriteLine("<%END%>");
    }
}