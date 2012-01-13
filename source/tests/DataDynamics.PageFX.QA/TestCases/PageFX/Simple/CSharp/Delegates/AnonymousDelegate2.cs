using System;

internal delegate void Func();

class AnonymousDelegate2
{
    static void foo(Func f)
    {
        f();
    }

    static Func GetFunc()
    {
        int i = 10;
        return delegate
        {
            i += 10;
            Console.WriteLine(i);
            i += 10;
            Console.WriteLine(i);
        };
    }

    static void Main()
    {
        int i = 10;

        Console.WriteLine(i);

        Func f = GetFunc();

        foo(f);
        Console.WriteLine(i);
        foo(f);

        i += 10;
        Console.WriteLine(i);
        Console.WriteLine("<%END%>");
    }
}