using System;

interface IFoo<T>
{
    T this[int index]
    {
        get;
        set;
    }
}

public class FooCollection<T> : IFoo<T>
{
    T IFoo<T>.this[int index]
    {
        get
        {
            return default(T);
        }
        set
        {
        }
    }
}

class X
{
    static void Test1()
    {
        IFoo<int> foo = new FooCollection<int>();
        int a = foo[3];
        Console.WriteLine(a);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
