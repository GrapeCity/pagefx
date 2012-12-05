using System;
using System.Collections.Generic;

public class A<T>
{
    private IEqualityComparer<T> comparer;

    public void Init(IEqualityComparer<T> comparer)
    {
        this.comparer = comparer ?? EqualityComparer<T>.Default;
    }
}

interface CI
{
}

interface I1 : CI
{
}

interface I2 : CI
{
}

interface I3 : I2
{
}

enum E { A, B, C, D }

class B
{
    public E Value
    {
        get { return E.C; }
    }
}

internal class Test
{
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        var a = new A<int>();
        a.Init(null);
    }

    static I1 GetI1()
    {
        return null;
    }

    static I3 GetI3()
    {
        return null;
    }

    static void TestCI(int n)
    {
        CI i = n == 0 ? (CI)GetI1() : (CI)GetI3();
        Console.WriteLine(i == null);
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        TestCI(0);
        TestCI(1);
    }

    static void Foo(B b)
    {
        E e = b != null ? b.Value : 0;
        Console.WriteLine(e);
    }

    static E V1 = E.D;

    static void Bar(B b)
    {
        E e = b != null ? b.Value : V1;
        Console.WriteLine(e);
    }

    static void Test3()
    {
        Console.WriteLine("--- Test3");
        Foo(new B());
        Foo(null);
        Bar(new B());
        Bar(null);
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Console.WriteLine("<%END%>");
    }
}