using System;

class A<T1, T2>
{
    public T1 V1;
    public T2 V2;

    public A()
    {
    }

    public A(T1 v1, T2 v2)
    {
        V1 = v1;
        V2 = v2;
    }
}

class Test
{
    
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        A<int, int> a = new A<int, int>(10, 20);
        Console.WriteLine(a.V1);
        Console.WriteLine(a.V2);
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        A<bool, string> a = new A<bool, string>(false, "aaa");
        Console.WriteLine(a.V1);
        Console.WriteLine(a.V2);
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}