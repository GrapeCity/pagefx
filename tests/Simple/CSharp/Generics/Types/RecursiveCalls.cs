using System;

class A<T>
{
    public T Value;

    public A(T value)
    {
        Value = value;

        if (Test.Counter > 0)
        {
            --Test.Counter;
            Test.S1 = new A<int>(Test.Counter);
        }
    }
}

class Test
{
    public static int Counter = 100;
    public static A<int> S1;

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        try
        {
            A<int> a = new A<int>(10);
            Console.WriteLine(a.Value);
            Console.WriteLine(Test.S1.Value);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}