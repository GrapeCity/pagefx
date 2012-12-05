using System;

class A
{
    public int get_Value;
}

class B : A
{
    public int Value
    {
        get { return 100; }
    }
}

class Test
{
    static void Main()
    {
        Console.WriteLine("Hello!");
        B b = new B();
        Console.WriteLine(b.Value);
        Console.WriteLine("<%END%>");
    }
}