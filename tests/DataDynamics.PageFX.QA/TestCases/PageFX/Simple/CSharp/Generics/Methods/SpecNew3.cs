using System;

interface I1
{
    int Foo(int v1, int v2);
}

interface I2
{
    int Foo<T>(int v1, T v2);
}

class A<T1, T2> : I1, I2
{
    public virtual int Foo<T>(T1 v1, T2 v2)
    {
        Console.WriteLine("A::Foo<T>(T1, T2)");
        Console.WriteLine(typeof(T1));
        Console.WriteLine(typeof(T2));
        Console.WriteLine(v1);
        Console.WriteLine(v2);
        return 0;
    }

    public virtual int Foo<T>(int v1, T v2)
    {
        Console.WriteLine("A::Foo<T>(int, T)");
        Console.WriteLine(typeof(T));
        Console.WriteLine(v1);
        Console.WriteLine(v2);
        return v1 + 1;
    }

    public virtual int Foo(int v1, int v2)
    {
        Console.WriteLine("A::Foo(int, int)");
        return v1 + v2;
    }

    int I1.Foo(int v1, int v2)
    {
        Console.WriteLine("A::I1.Foo(int, int)");
        return v1 * v2;
    }

    int I2.Foo<T>(int v1, T v2)
    {
        Console.WriteLine("A::I2.Foo<T>(int, T)");
        Console.WriteLine(typeof(T));
        Console.WriteLine(v1);
        Console.WriteLine(v2);
        return v1 * 10;
    }
}

class A1 : A<int, int>, I1, I2
{
    public new int Foo(int v1, int v2)
    {
        Console.WriteLine("A1::Foo(int, int)");
        return base.Foo(v1, v2) + 2;
    }

    public new int Foo<T>(int v1, T v2)
    {
        Console.WriteLine("A1::Foo<T>(int, T)");
        Console.WriteLine(typeof(T));
        Console.WriteLine(v1);
        Console.WriteLine(v2);
        return base.Foo<T>(v1, v2) + 3;
    }
}

class Test
{
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        A1 a = new A1();
        Console.WriteLine(((I1)a).Foo(1, 1));
        Console.WriteLine(((A<int, int>)a).Foo(2, 2));
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        A1 a = new A1();
        Console.WriteLine(((I2)a).Foo<int>(2, 2));
        Console.WriteLine(((I2)a).Foo<string>(3, "aaa"));
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}