using System;

class A
{
    public int Value;
}

class B
{
    public A Value
    {
        get { return _value; }
    }
    private readonly A _value = new A();
}

class Test3
{
    static void f(int i)
    {
        Console.WriteLine(i);
    }

    static void Main()
    {
        B b = new B();
        f(b.Value.Value);
        f(b.Value.Value++);
        f(++b.Value.Value);
        f(b.Value.Value);
        f(b.Value.Value--);
        f(--b.Value.Value);
        f(b.Value.Value);
        Console.WriteLine("<%END%>");
    }
}