using System;

class X
{

    interface I
    {
        int Answer();
    }
    
    private class B : I
    {
        public int Answer()
        {
            return (42);
        }
    }

    private class C : B {}

    private class D<TArg>
    {
        public TArg value;
    }

    private class A
    {
        public void Foo<T, U>()
            where T : class, new()
        {
            T t1 = new T();
            U[] u = new U[10];
        }

        public int Bar<U>(U u)
            where U : I
        {
            return u.Answer();
        }

        public void Baz<T>()
            where T : B, new()
        {
            Bar<T>(new T());
        }

        public D<T> Qaz<T>(T t)
        {
            D<T> d = new D<T>();
            d.value = t;
            return d;
        }
    }
    
    static void Test1()
    {
        Console.WriteLine("----Test2");
        A a = new A();
        a.Foo<A, int>();
    }
    
    static void Test2()
    {
        Console.WriteLine("----Test2");
        A a = new A();
        a.Baz<C>();
    }

    static void Test3()
    {
        Console.WriteLine("----Test3");
        A a = new A();
        Console.WriteLine(a.Qaz<int>(42).value); 
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Console.WriteLine("<%END%>");
    }
}