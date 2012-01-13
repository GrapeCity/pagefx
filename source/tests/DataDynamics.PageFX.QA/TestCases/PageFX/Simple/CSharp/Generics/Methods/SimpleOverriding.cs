using System;

class Test
{
    abstract class B<T>
    {
        public abstract T Foo<T>();
        public abstract T Fct(T t);
    }

    class D1 : B<string> 
    {
        public override T Foo<T>() 
        {
            return default(T);
        }

        public override string Fct(string t)
        {
            return "hello";
        }
    }

    class D2<T> : B<T>
    {
        public override T1 Foo<T1>()
        {
            return default(T1);
        }

        public override T Fct(T t)
        {
            return default(T);
        }
    }


    class D3<T, U> : B<U>
    {
        #region Overrides of B<U>

        public override T1 Foo<T1>()
        {
            return default(T1);
        }

        public override U Fct(U t)
        {
            return default(U);
        }

        #endregion
    }

    class B
    {
        public virtual void Fct() { }
    }

    class D<T> : B where T : new()
    {
        public override void Fct()
        {
            T t = new T();
        }
    }

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        D1 d = new D1();
        Console.WriteLine(d.Foo<string>());
        Console.WriteLine(d.Fct("ok"));
    }
    
    static void Test2()
    {
        Console.WriteLine("--- Test2");
        D2<int> d = new D2<int>();
        Console.WriteLine(d.Fct(42));
    }

    static void Test3()
    {
        Console.WriteLine("--- Test3");
        D3<int, double> d = new D3<int, double>();
        Console.WriteLine(d.Fct(2.718281828459045235360287));
    }

    static void Test4()
    {
        Console.WriteLine("--- Test4");
        D<float> d = new D<float>();

        try
        {
            d.Fct();
        }
        catch (Exception)
        {
            Console.WriteLine("d.Fct()");
        }
    }

    static void Main ()
    {
        Test1();
        Test2();
        Test3();
        Test4();
        Console.WriteLine("<%END%>");
    }
}
