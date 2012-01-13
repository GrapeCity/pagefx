using System;

class Test
{
    static void foo(int value)
    {
        Console.WriteLine(value);
    }

    static void foo(string s)
    {
        Console.WriteLine(s);
    }

    static string getstr1()
    {
        return "aaa";
    }

    static void Main()
    {
        string msg = "Hello World!!!";
        foo(msg);

        int i = 0;
        foo(i);
        for (i = 1; i <= 5; ++i)
            foo(i);

        msg = getstr1();
        foo(msg);
        Console.WriteLine("<%END%>");
    }
}