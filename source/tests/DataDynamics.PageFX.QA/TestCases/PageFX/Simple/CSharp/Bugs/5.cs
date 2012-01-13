using System;
using System.Collections.Generic;

class A<T>
{
    public IEqualityComparer<T> comparer;

    public A(IEqualityComparer<T> comparer)
    {
        this.comparer = comparer ?? EqualityComparer<T>.Default;
    }
}

internal class Test
{
    static void Test1()
    {
        Console.WriteLine("--- Test1");

        var eq1 = EqualityComparer<string>.Default;
        var eq2 = EqualityComparer<int>.Default;

        Console.WriteLine(ReferenceEquals(eq1, eq2));

        Console.WriteLine(eq1.Equals("aaa", "bbb"));
        Console.WriteLine(eq2.Equals(10, 20));
    }

    static A<T> CreateA<T>(IEqualityComparer<T> comparer)
    {
        var a = new A<T>(comparer ?? EqualityComparer<T>.Default);
        return a;
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        var a = CreateA<string>(null);
        Console.WriteLine(a.comparer.Equals("aaa", "bbb"));
    }

    static void TestIEquatableString(IEquatable<string> e)
    {
        Console.WriteLine(e.Equals("aaa"));
        Console.WriteLine(e.Equals("bbb"));
        Console.WriteLine(e.Equals(null));
    }

    static void Test3()
    {
        Console.WriteLine("--- Test3");
        TestIEquatableString("aaa");
        TestIEquatableString("bbb");
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Console.WriteLine("<%END%>");
    }
}