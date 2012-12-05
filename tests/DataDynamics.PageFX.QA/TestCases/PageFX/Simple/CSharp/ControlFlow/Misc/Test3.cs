using System;

class Test3
{
    static void Write(string s)
    {
        if (s == null || s.Length == 0) return;
        Console.WriteLine(s);
    }

    static void Main()
    {
        Write(null);
        Write("");
        Write("aaa");
        Console.WriteLine("<%END%>");
    }
}