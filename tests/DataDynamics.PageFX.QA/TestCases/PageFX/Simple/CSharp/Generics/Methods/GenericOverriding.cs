using System;

namespace GenericOverriding
{
    class  A
    {
        public virtual T1 Foo<T1>(T1 arg)
        {
            return arg;
        }

        public virtual T1 Foo<T1>(int arg)
        {
            return default(T1);
        }
    }

    class B : A
    {
        public int Foo(int arg)
        {
            return base.Foo(arg) + arg;
        }
    }

    class X
    {
        static void Test1()
        {
            Console.WriteLine("----Test1");
            B b = new B();
            Console.WriteLine(b.Foo(1));
        }

        static void Test2()
        {
            Console.WriteLine("----Test2");
            B b = new B();
            Console.WriteLine((b as A).Foo(2));
        }

        static void Test3()
        {
            Console.WriteLine("----Test3");
            B b = new B();
            Console.WriteLine((b as A).Foo(3));
        }

        static void Main()
        {
            Test1();
            Test2();
            Test3();
            Console.WriteLine("<%END%>");
        }
    }
}