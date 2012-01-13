using System;

class A
{
    public int value;
}

class Test
{
    static void Main()
    {
        A obj = new A();
        Type type = obj.GetType();
        Console.WriteLine(type.FullName);
        Console.WriteLine("<%END%>");
    }
}