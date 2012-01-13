using System;

class A
{
    public static int value;

    static A()
    {
        Console.WriteLine("A::.cctor()");
    }

    public static void f()
    {
        Console.WriteLine("A::f()");
    }
}

class StaticCtor2
{
    static void Main()
    {
        A.value = 10;
        Console.WriteLine(A.value);
        A.f();
        Console.WriteLine("<%END%>");
    }
}