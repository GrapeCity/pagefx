// Important test: Type inference

using System;

class Test<A, B>
{
    public void Foo<U>(U u)
    { }

    public void Foo<V>(V[] v, V w)
    { }

    public void Hello<V, W>(V v, W w, Test<V, W> x)
    { }

    public void ArrayMethod<V>(params V[] args)
    { }
}

class X
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        try
        {
            Test<float, int> test = new Test<float, int>();
            test.Foo("Hello World");
            test.Foo(new long[] { 3, 4, 5 }, 9L);
            test.Hello(3.14F, 9, test);
            test.ArrayMethod(3.14F, (float)9 / 3);
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

