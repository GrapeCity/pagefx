using System;

namespace Overriding2
{
    class A<T>
    {
        public virtual T Foo(T arg)
        {
            return arg;
        }
    }

    class B : A<int>
    {
        public override int Foo(int arg)
        {
            return base.Foo(arg) + 10;
        }
    }

    class C : A<int>
    {
        public new int Foo(int arg)
        {
            return base.Foo(arg) + 20;
        }
    }

    class X
    {
        static void Test1()
        {
            Console.WriteLine("----Test1");
            C c = new C();
            Console.WriteLine(c.Foo(1));
        }

        static void Test2()
        {
            Console.WriteLine("----Test2");
            C c = new C();
            Console.WriteLine((c as A<int>).Foo(2));
        }

        static void Test3()
        {
            Console.WriteLine("----Test3");
            B b = new B();
            Console.WriteLine(b.Foo(3));
        }

        static void Main()
        {
            //Test1();
            Test2();
            //Test3();
            Console.WriteLine("<%END%>");
        }
    }
}