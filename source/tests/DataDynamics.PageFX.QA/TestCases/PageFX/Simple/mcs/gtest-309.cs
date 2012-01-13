using System;

class Test<A, B>
{
    public void Foo<V, W>(Test<A, W> x, Test<V, B> y)
    { }
}

class X
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        Test<float, int> test = new Test<float, int>();
        test.Foo(test, test);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
