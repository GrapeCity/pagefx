using System;

class A
{
    public static int N;
    public static B B = new B();
    public static C C = new C();

    static A()
    {
        Console.WriteLine("A::begin");
        Console.WriteLine(N);
        N = 10;
        Console.WriteLine("A::end");
    }

    public static void Foo()
    {
        Console.WriteLine("A::Foo");
    }
}

class B : A
{
    public static C C2;

    static B()
    {
        Console.WriteLine("B::begin");
        C2 = new C();
        Console.WriteLine(N);
        N = 20;
        Console.WriteLine(A.B != null);
        Console.WriteLine(A.C != null);
        Console.WriteLine("B::end");
    }
}

class C : A
{
    static C()
    {
        Console.WriteLine("C::begin");
        Console.WriteLine(N);
        N = 30;
        Console.WriteLine(B.C2 != null);
        Console.WriteLine("C::end");
    }
}

class Test
{
    static void Main()
    {
        Console.WriteLine("Hello!");
        Console.WriteLine(10);
        //A.Foo();
        new B();
        Console.WriteLine("<%END%>");
    }
}