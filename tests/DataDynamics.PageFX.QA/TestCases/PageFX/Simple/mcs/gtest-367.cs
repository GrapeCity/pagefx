using System;

class Foo { }

class Repro
{
    public void Bar<TFoo>(TFoo foo) where TFoo : Repro
    {
        Baz(foo, Gazonk);
    }

    static void Baz<T>(T t, Action<T> a)
    {
        a(t);
    }

    static void Gazonk(Repro f)
    {
        f.PrintOk();
    }

    private void PrintOk()
    {
        Console.WriteLine("OK");
    }

    static void Test1()
    {
        Console.WriteLine("----Test1");
        Repro r = new Repro();
        r.Bar(r);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
