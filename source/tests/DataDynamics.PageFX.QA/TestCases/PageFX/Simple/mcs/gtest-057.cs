using System;

interface IHello<T>
{
    void Print(T t);
}

interface Foo
{
    IHello<U> Test<U>();
}

class Hello<T> : IHello<T>, Foo
{
    public void Print(T t)
    {
        Console.WriteLine("Hello: {0}", t);
    }

    public IHello<U> Test<U>()
    {
        return new Hello<U>();
    }
}

class X
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        try
        {
            Hello<int> hello = new Hello<int>();
            hello.Print(5);
            hello.Test<float>().Print(3.14F);

            IHello<string> foo = hello.Test<string>();
            foo.Print("World");
        }
        catch (Exception e)
        {
            Console.WriteLine("error" + e);
        }
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
