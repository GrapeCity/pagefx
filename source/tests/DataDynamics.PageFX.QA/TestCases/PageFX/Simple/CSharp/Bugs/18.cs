using System;
using System.Collections;
using System.Collections.Generic;

class X
{
    static void Test1()
    {
        Console.WriteLine("--Test1");
        string text = "Textbox1";
        string what = "text";
        StringComparison sc = StringComparison.CurrentCultureIgnoreCase;
        int i = text.IndexOf(what, 0, sc);
        Console.WriteLine(i);
        i = text.IndexOf(what, i + what.Length, sc);
        Console.WriteLine(i);
    }

    static void Test2()
    {
        Console.WriteLine("--Test2");
        string text = "Textbox1";
        string what = "text";
        StringComparison sc = StringComparison.CurrentCultureIgnoreCase;
        int i = text.LastIndexOf(what, text.Length - 1, sc);
        Console.WriteLine(i);
        i = text.LastIndexOf(what, 0, sc);
        Console.WriteLine(i);
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}