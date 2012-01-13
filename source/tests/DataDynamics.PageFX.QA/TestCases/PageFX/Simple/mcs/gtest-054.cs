//
// Conversions involving type parameters (26.7.4).
// This is a compilation-only test since some of the explict
// conversions would trigger an InvalidCastException.
//

using System;

namespace gtest54
{
    interface Foo
    {
        void Hello();
    }

    class A
    { }

    class B : A, Foo
    {
        public void Hello()
        {
            Console.WriteLine("Hello");
        }

        public static implicit operator C(B b)
        {
            return new C();
        }
    }

    class C
    {
        public static explicit operator B(C c)
        {
            return new B();
        }
    }

    class Test
    {
        static T Simple<T>(T t)
        {
            object o = t;
            t = (T)o;
            Foo foo = (Foo)t;
            t = (T)foo;
            return t;
        }

        static T Interface<T>(T t)
            where T : Foo
        {
            Foo foo = t;
            return (T)foo;
        }

        static T Class<T>(T t)
            where T : B
        {
            B b = t;
            A a = t;
            Foo foo = t;
            t = (T)b;
            t = (T)a;
            t = (T)foo;
            C c = t;
            t = (T)c;
            return t;
        }

        static T[] Array<T>(T[] t)
        {
            object o = t;
            Array a = t;
            t = (T[])o;
            t = (T[])a;
            return t;
        }


        static void Test1()
        {
            Console.WriteLine("----Test1");
            try
            {
                Console.WriteLine("ok");
                int[] a = { 1, 2, 3, 4 };
                Console.WriteLine(Array(a));
            }
            catch (Exception e)
            {
                Console.WriteLine("error " + e.GetType());
            }
        }

        static void Test2()
        {
            Console.WriteLine("----Test2");
            try
            {
                Console.WriteLine("ok");
                Class(new B());
            }
            catch (Exception e)
            {
                Console.WriteLine("error " + e.GetType());
            }
        }

        static void Test3()
        {
            Console.WriteLine("----Test3");
            try
            {
                Console.WriteLine("ok");
                Console.WriteLine(Simple(5));
            }
            catch (Exception e)
            {
                Console.WriteLine("error " + e.GetType());
            }
        }

        static void Test4()
        {
            Console.WriteLine("----Test3");
            try
            {
                Console.WriteLine("ok");
                Console.WriteLine(Interface(new B()));
            }
            catch (Exception e)
            {
                Console.WriteLine("error " + e.GetType());
            }
        }
        static void Main()
        {
            Test1();
            Console.WriteLine("<%END%>");
        }
    }
}
