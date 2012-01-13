using System;

class A
{
    public int value;
}

class B : A
{
    public new int value()
    {
        return 10;
    }

    public new int value(int a)
    {
        return a + 10;
    }
}

class Test
{
    static void Main()
    {
        Console.WriteLine("Hello!");
        B b = new B();
        Console.WriteLine(b.value());
        Console.WriteLine(b.value(10));
        Console.WriteLine("<%END%>");
    }
}