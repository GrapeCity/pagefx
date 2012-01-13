using System;

delegate void SimpleHandler();

class A
{
    public event SimpleHandler X;

    public void Foo()
    {
        Console.WriteLine("A::Foo Before");
        if (X != null)
            X();
        Console.WriteLine("A::Foo After");
    }
}

class Program
{
    static void f1()
    {
        Console.WriteLine("f1");
    }

    static void f2()
    {
        Console.WriteLine("f2");
    }

    static void Main()
    {
        A obj = new A();
        obj.X += f1;
        obj.X += f2;
        obj.Foo();
        Console.WriteLine("<%END%>");
    }
}