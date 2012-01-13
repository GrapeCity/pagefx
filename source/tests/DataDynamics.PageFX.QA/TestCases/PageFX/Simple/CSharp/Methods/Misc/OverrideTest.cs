using System;

class A
{
    public virtual int Foo(int v)
    {
        Console.WriteLine("A::Foo");
        return v + 1;
    }
}

class B : A
{
    public override int Foo(int v)
    {
        Console.WriteLine("B::Foo");
        return base.Foo(v) + 2;
    }
}

class C : A
{
    public new int Foo(int v)
    {
        Console.WriteLine("C::Foo");
        return base.Foo(v) + 3;
    }
}

class D : C
{
    public new int Foo(int v)
    {
        Console.WriteLine("D::Foo");
        return base.Foo(v) + 4;
    }
}

class Test
{
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        A a = new B();
        Console.WriteLine(a.Foo(1));
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        A a = new C();
        Console.WriteLine(a.Foo(2));
    }

    static void Test3()
    {
        Console.WriteLine("--- Test3");
        C c = new C();
        Console.WriteLine(c.Foo(3));
    }

    static void Test4()
    {
        Console.WriteLine("--- Test4");
        C c = new D();
        Console.WriteLine(c.Foo(4));
    }

    static void Test5()
    {
        Console.WriteLine("--- Test5");
        D d = new D();
        Console.WriteLine(d.Foo(5));
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Test4();
        Test5();
        Console.WriteLine("<%END%>");
    }
}