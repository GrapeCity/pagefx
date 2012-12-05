using System;

internal class Test
{
    static void Foo(object obj)
    {
        Console.WriteLine(obj.GetType() == typeof(int));
    }

    static void Main()
    {
        int? v = 10;
        Foo(v);
        Console.WriteLine("<%END%>");
    }
}