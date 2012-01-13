using System;

class A<T>
{
    public T Convert<U>(U u)
    {
        return default(T);
    }
}

class B
{
    public T Convert<T>(T t)
    {
        return default(T);
    }
}

class Test
{
    static void Test1()
    {
        A<int> a = new A<int>();
        int v = a.Convert<double>(0);
        Console.WriteLine(v);
    }

    static void Test2()
    {
        B b = new B();
        int v = b.Convert<int>(42);
        Console.WriteLine(v);
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}