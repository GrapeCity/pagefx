using System;

internal delegate void Func();

class AnonymousDelegate1
{
    static void foo(Func f)
    {
        f();
    }

    static void Main()
    {
        int i = 10;

        Console.WriteLine(i);

        Func f = delegate
                {
                    i += 10;
                    Console.WriteLine(i);
                    i += 10;
                    Console.WriteLine(i);
                };

        foo(f);
        Console.WriteLine(i);
        foo(f);

        i += 10;
        Console.WriteLine(i);
        Console.WriteLine("<%END%>");
    }
}