using System;

delegate void Func();

class A
{
    public void f()
    {
        Console.WriteLine("A::f()");
    }
}

class Delegate2
{
    public static void Main()
    {
        A a = new A();
        Func f = a.f;
        f();
        Console.WriteLine("<%END%>");
    }
}