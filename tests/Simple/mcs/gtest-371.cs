// Compiler options: -t:library

using System;

class X
{
    public class Test<A, B>
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

    static void Test1()
    {
        Console.WriteLine("----Test1");
        Test<int, double> t = new Test<int, double>();
        t.Foo(5);
        int[] a = {1, 2, 3, 4};
        t.Foo(a, 42);
        t.Hello<int, double>(5, 42, t);
        Console.WriteLine("ok");
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}