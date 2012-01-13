using System;

class X
{
    static void f(string s)
    {
        Console.WriteLine(s);
        s = "aaa";
        Console.WriteLine(s);
    }

    static void Main()
    {
        string s = "bbb";
        f(s);
        f(s);
        Console.WriteLine("<%END%>");
    }
}