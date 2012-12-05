using System;

namespace N
{
    public class TestG
    {
        public static void Foo<T>()
        {
            Console.WriteLine(typeof(T));
        }
    }
}

class NonGeneric
{
    public override string ToString()
    {
        return "NonGeneric";
    }
}

class Generic<T>
{
    public override string ToString()
    {
        return typeof(T).FullName;
    }
}

class m
{
    public global::NonGeneric compiles_fine(global::NonGeneric i, out global::NonGeneric o)
    {
        o = new global::NonGeneric();
        return new global::NonGeneric();
    }

    public global::Generic<int> does_not_compile(global::Generic<int> i, out global::Generic<int> o)
    {
        o = new global::Generic<int>();
        return new global::Generic<int>();
    }

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        m m = new m();
        NonGeneric ng;
        ng = m.compiles_fine(null, out ng);
        Console.WriteLine(ng);
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        m m = new m();
        Generic<int> g;
        g = m.does_not_compile(null, out g);
        Console.WriteLine(g);
    }

    public static void Main()
    {
        Test1();
        Test2();
        global::N.TestG.Foo<int>();
        Console.WriteLine("<%END%>");
    }
}