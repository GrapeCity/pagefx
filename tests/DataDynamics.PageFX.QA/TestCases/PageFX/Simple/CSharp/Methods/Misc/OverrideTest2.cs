using System;

class A
{
    public virtual int Foo(int v)
    {
        Console.WriteLine("A::Foo");
        return v;
    }
}

class B : A
{
}

class C : B
{
    public override int Foo(int v)
    {
        Console.WriteLine("C::Foo");
        return base.Foo(v) + 2;
    }
}

class Test
{
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        A a = new C();
        Console.WriteLine(a.Foo(1));
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}