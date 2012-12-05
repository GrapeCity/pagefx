using System;

class A<T>
{
    public T V;
}

class A<T1, T2>
{
    public T1 V1;
    public T2 V2;
}

class A<T1, T2, T3>
{
    public T1 V1;
    public T2 V2;
    public T3 V3;
}

class Test
{
    static void Test1()
    {
        A<int> a = new A<int>();
        Console.WriteLine(a.GetType() == typeof(A<>));
        Console.WriteLine(a.GetType() == typeof(A<,>));
        Console.WriteLine(a.GetType() == typeof(A<,,>));
    }

    static void Test2()
    {
        A<int, int> a = new A<int, int>();
        Console.WriteLine(a.GetType() == typeof(A<>));
        Console.WriteLine(a.GetType() == typeof(A<,>));
        Console.WriteLine(a.GetType() == typeof(A<,,>));
        //not supported in C# 2.0
        //Console.WriteLine(a.GetType() == typeof(A<int,>));
    }

    static void Test3()
    {
        A<int, int, int> a = new A<int, int, int>();
        Console.WriteLine(a.GetType() == typeof(A<>));
        Console.WriteLine(a.GetType() == typeof(A<,>));
        Console.WriteLine(a.GetType() == typeof(A<,,>));
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Console.WriteLine("<%END%>");
    }
}