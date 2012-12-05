using System;

class A
{
    public int Value;
}

class A<T> where T : new()
{
    public T Value;

    public static T Create()
    {
        return new T();
    }

    public static A<T> CreateInstance()
    {
        return new A<T>();
    }

    public override string ToString()
    {
        return string.Format("A<{0}>", Value);
    }
}

class Test
{
    static void Test1()
    {
        Console.WriteLine(A<int>.Create());
    }

    static void Test2()
    {
        Console.WriteLine(A<A>.Create().Value);
    }
    
    static void Test3()
    {
        Console.WriteLine(A<A>.CreateInstance());
    }


    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Console.WriteLine("<%END%>");
    }
}