using System;

namespace NS1
{
    interface I
    {
        void f();
    }
}

namespace NS2
{
    interface I
    {
        void f();
    }
}

class A : NS1.I, NS2.I
{
    public void f()
    {
        Console.WriteLine("A::f()");
    }
}

class Program
{
    static void f1(NS1.I i)
    {
        i.f();
    }

    static void f2(NS2.I i)
    {
        i.f();
    }

    static void Main()
    {
        A obj = new A();
        f1((NS1.I)obj);
        f2((NS2.I)obj);
        Console.WriteLine("<%END%>");
    }
}