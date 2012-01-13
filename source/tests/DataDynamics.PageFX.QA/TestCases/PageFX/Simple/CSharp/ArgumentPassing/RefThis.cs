using System;

struct A
{
    public int value;

    static void f2(ref A obj)
    {
        A a = new A();
        a.value = obj.value + 10;
        obj = a;
    }

    public void f()
    {
        Console.WriteLine(value);
        f2(ref this);
        Console.WriteLine(value);
    }
}

class X
{
    static void Main()
    {
        Console.WriteLine("Hello!");
        A obj = new A();
        obj.f();
        Console.WriteLine("<%END%>");
    }
}