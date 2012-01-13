using System;

class A
{
    public int Value;
}

class Test2
{
    static void f(int i)
    {
        Console.WriteLine(i);
    }

    static void Main()
    {
        A a = new A();
        f(a.Value);
        f(a.Value++);
        f(++a.Value);
        f(a.Value);
        f(a.Value--);
        f(--a.Value);
        f(a.Value);
        Console.WriteLine("<%END%>");
    }
}