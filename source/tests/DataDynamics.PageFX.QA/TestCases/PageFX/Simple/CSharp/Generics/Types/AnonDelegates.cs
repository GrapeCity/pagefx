using System;

delegate TResult Func<TResult>(TResult value);

delegate TResult Func<TArg, TResult>(TArg arg) where TArg: TResult;

class Test
{
    class A<T>
    {
        private T val;
        
        public A(T value)
        {
            val = value;
        }

        public T Foo()
        {
            Func<T, T> f = delegate(T a) { return a; };
            return f(val);
        }
    }

    class A<T1, T2> where T1: T2
    {
        public T2 Foo(T1 t1)
        {
            Func<T1, T2> f = delegate(T1 temp) { return default(T1); };
            return f(t1);
        }
    }

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        Func<int, int> f = delegate(int a) { return a + 10; };
        Console.WriteLine(f(42));
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        A<double> a = new A<double>(2.718281828459045235360287);
        Console.WriteLine(a.Foo());
    }

    static void Test3()
    {
        Console.WriteLine("--- Test3");
        A<int, int> a = new A<int, int>();
        Console.WriteLine(a.Foo(42));
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Console.WriteLine("<%END%>");
    }
}