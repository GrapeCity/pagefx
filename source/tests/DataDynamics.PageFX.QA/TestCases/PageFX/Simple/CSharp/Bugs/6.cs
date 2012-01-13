using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

internal class Test
{
    static void Test1()
    {
        try
        {
            Foo(null);
        }
        catch (ArgumentNullException)
        {
            Console.WriteLine("ok");
        }
    }

    static void Foo(IEnumerable<string> source)
    {
        var m = source.Max<string, string>((Func<string, string>)(x => "test"));
        Console.WriteLine(m);
    }

    static void Test2()
    {
        Foo(new[] { "aaa" });
    }

    static void Main()
    {
        Console.WriteLine("Hello!");
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}