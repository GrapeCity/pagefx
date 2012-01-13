using System;

class Test
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        A<int> a = new A<int>(new A<int>.B(D), 3);
        a.Run();
        Console.WriteLine("ok");
    }

    static void Main(string[] args)
    {
        Test1();
        Console.WriteLine("<%END%>");
    }

    public static void D(int y)
    {
        System.Console.WriteLine("Hello " + 3);
    }
}

class A<T>
{
    public delegate void B(T t);

    protected B _b;
    protected T _value;

    public A(B b, T value)
    {
        _b = b;
        _value = value;
    }
    public void Run()
    {
        _b(_value);
    }
}

