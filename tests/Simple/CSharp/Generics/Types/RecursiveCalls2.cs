using System;

class A<T>
{
    public T Value;

    public static A<A<T>> S1;

    public A(T value)
    {
        Value = value;

        if (Test.Counter > 0)
        {
            --Test.Counter;
            S1 = new A<A<T>>(this);
            Test.Arr[Test.Index] = S1;
            ++Test.Index;
        }
    }

    public override string ToString()
    {
        return string.Format("A<{0}>", Value);
    }
}

class Test
{
    public static int Counter = 100;
    public static int Index = 0;
    public static object[] Arr = new object[Counter];
    
    static void Test1()
    {
        try
        {
            Console.WriteLine("--- Test1");
            A<int> a = new A<int>(10);
            Console.WriteLine(a.Value);
            for (int i = 0; i < Arr.Length; ++i)
                Console.WriteLine(Arr[i]);
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