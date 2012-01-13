using System;

namespace SpecOverriding
{
    class A<T1, T2>
    {
        public virtual int Foo<T>(T1 v1, T2 v2)
        {
            Console.WriteLine("A::Foo(T1, T2)");
            Console.WriteLine(typeof(T1));
            Console.WriteLine(typeof(T2));
            return 0;
        }

        public virtual int Foo<T>(int v1, T v2)
        {
            Console.WriteLine("A::Foo(int, T)");
            Console.WriteLine(typeof(T));
            return v1 + 1;
        }

        //public virtual int Foo<T>(T v1, int v2)
        //{
        //    Console.WriteLine("A::Foo(T, int)");
        //    return v2 + 1;
        //}

        public virtual int Foo(int v1, int v2)
        {
            Console.WriteLine("A::Foo(int, int)");
            return v1 + v2;
        }
    }

    class A1 : A<int, int>
    {
        public override int Foo(int v1, int v2)
        {
            Console.WriteLine("A1::Foo(int, int)");
            return base.Foo(v1, v2) + 2;
        }

        public override int Foo<T>(int v1, T v2)
        {
            Console.WriteLine("A1::Foo(int, T)");
            Console.WriteLine(typeof(T));
            return base.Foo<T>(v1, v2) + 3;
        }
    }

    class Test
    {
        static void Test1()
        {
            Console.WriteLine("--- Test1");
            A<int, int> a = new A1();
            Console.WriteLine(a.Foo(1, 1));
        }

        static void Test2()
        {
            Console.WriteLine("--- Test2");
            A<int, int> a = new A1();
            Console.WriteLine(a.Foo<int>(2, 2));
            Console.WriteLine(a.Foo<string>(3, 3));
        }

        static void Main()
        {
            Test1();
            Test2();
            Console.WriteLine("<%END%>");
        }
    }
}