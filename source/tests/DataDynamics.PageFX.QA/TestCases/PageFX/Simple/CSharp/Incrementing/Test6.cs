using System;

class A
{
    private readonly int[] _arr = new int[] { 10, 20, 30 };

    public int this[int i]
    {
        get { return _arr[i]; }
        set { _arr[i] = value; }
    }
}

class Test6
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