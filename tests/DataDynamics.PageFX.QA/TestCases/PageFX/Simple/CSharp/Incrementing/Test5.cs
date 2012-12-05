using System;

class A
{
    private int _value;

    public int this[int i]
    {
        get { return _value; }
        set { _value = value; }
    }
}

class Test5
{
    static void f(int i)
    {
        Console.WriteLine(i);
    }

    static void Main()
    {
        A obj = new A();
        f(obj[0]++);
        f(++obj[0]);
        f(obj[0]--);
        f(--obj[0]);
        Console.WriteLine("<%END%>");
    }
}