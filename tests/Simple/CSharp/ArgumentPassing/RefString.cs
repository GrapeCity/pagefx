using System;

class X
{
    static void f(ref string s)
    {
        Console.WriteLine(s);
        s = "aaa";
        Console.WriteLine(s);
    }

    static void Main()
    {
        string s = "bbb";
        f(ref s);
        f(ref s);
        Console.WriteLine("<%END%>");
    }
}