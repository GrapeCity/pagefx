using System;

delegate void Foo1();
delegate void Foo2();

delegate R Func<T,R>(T arg);
delegate R Func<T1, T2, R>(T1 arg1, T2 arg2);
delegate R Func<T1, T2, T3, R>(T1 arg1, T2 arg2, T3 arg3);

interface I
{
}

class A
{
    public override string ToString()
    {
        return "A";
    }
}

class B : A, I
{
    public override string ToString()
    {
        return "B";
    }
}

class C : I
{
    public override string ToString()
    {
        return "C";
    }
}

namespace PageFX
{
    using T = Foo1;

    class Test
    {
        static void TestAs(object obj)
        {
            try
            {
                var f = obj as T;
                Console.WriteLine(f != null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());
            }
        }

        static void TestIs(object obj)
        {
            try
            {
                Console.WriteLine(obj is T);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());
            }
        }

        static void TestCast(object obj)
        {
            try
            {
                var f = (T)obj;
                Console.WriteLine(f != null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());
            }
        }

        static void TestCore(object obj)
        {
            TestAs(obj);
            TestIs(obj);
            TestCast(obj);
        }

        static void FromNull()
        {
            Console.WriteLine("--- FromNull");
            TestCore(null);
        }

        static void FromObject()
        {
            Console.WriteLine("--- FromObject");
            TestCore(new object());
        }

        static void FromString()
        {
            Console.WriteLine("--- FromString");
            TestCore("aaa");
        }

        static void FromFoo1()
        {
            Console.WriteLine("--- FromFoo1");
            TestCore((Foo1)(() => Console.WriteLine("Foo1")));
        }

        static void FromFoo2()
        {
            Console.WriteLine("--- FromFoo2");
            TestCore((Foo2)(() => Console.WriteLine("Foo2")));
        }

        static void FromFunc_A_A()
        {
            Console.WriteLine("--- FromFunc_A_A");
            TestCore((Func<A,A>)(x => x));
        }

        static void FromFunc_A_B()
        {
            Console.WriteLine("--- FromFunc_A_B");
            TestCore((Func<A, B>)(x => x as B));
        }

        static void FromFunc_A_C()
        {
            Console.WriteLine("--- FromFunc_A_C");
            TestCore((Func<A, C>)(x => null));
        }

        static void FromFunc_A_I()
        {
            Console.WriteLine("--- FromFunc_A_I");
            TestCore((Func<A, I>)(x => x as I));
        }

        static void FromFunc_B_A()
        {
            Console.WriteLine("--- FromFunc_B_A");
            TestCore((Func<B, A>)(x => x));
        }

        static void FromFunc_B_B()
        {
            Console.WriteLine("--- FromFunc_B_B");
            TestCore((Func<B, B>)(x => x));
        }

        static void FromFunc_B_C()
        {
            Console.WriteLine("--- FromFunc_B_C");
            TestCore((Func<B, C>)(x => null));
        }

        static void FromFunc_B_I()
        {
            Console.WriteLine("--- FromFunc_B_I");
            TestCore((Func<B, I>)(x => x as I));
        }

        static void FromFunc_C_A()
        {
            Console.WriteLine("--- FromFunc_C_A");
            TestCore((Func<C, A>)(x => null));
        }

        static void FromFunc_C_B()
        {
            Console.WriteLine("--- FromFunc_C_B");
            TestCore((Func<C, B>)(x => null));
        }

        static void FromFunc_C_C()
        {
            Console.WriteLine("--- FromFunc_C_C");
            TestCore((Func<C, C>)(x => x));
        }

        static void FromFunc_C_I()
        {
            Console.WriteLine("--- FromFunc_C_I");
            TestCore((Func<C, I>)(x => null));
        }

        static void FromFunc_I_A()
        {
            Console.WriteLine("--- FromFunc_I_A");
            TestCore((Func<I, A>)(x => null));
        }

        static void FromFunc_I_B()
        {
            Console.WriteLine("--- FromFunc_I_B");
            TestCore((Func<I, B>)(x => null));
        }

        static void FromFunc_I_C()
        {
            Console.WriteLine("--- FromFunc_I_C");
            TestCore((Func<I, C>)(x => null));
        }

        static void FromFunc_I_I()
        {
            Console.WriteLine("--- FromFunc_I_I");
            TestCore((Func<I, I>)(x => x));
        }

        static void FromAction_A()
        {
            Console.WriteLine("--- FromAction_A");
            TestCore((Action<A>)(x => Console.WriteLine("Hello")));
        }

        static void FromAction_B()
        {
            Console.WriteLine("--- FromAction_B");
            TestCore((Action<B>)(x => Console.WriteLine("Hello")));
        }

        static void FromAction_C()
        {
            Console.WriteLine("--- FromAction_C");
            TestCore((Action<C>)(x => Console.WriteLine("Hello")));
        }

        static void FromAction_I()
        {
            Console.WriteLine("--- FromAction_I");
            TestCore((Action<I>)(x => Console.WriteLine("Hello")));
        }

        static void Main()
        {
            FromNull();
            FromObject();
            FromString();

            FromFoo1();
            FromFoo2();

            FromFunc_A_A();
            FromFunc_A_B();
            FromFunc_A_C();
            FromFunc_A_I();

            FromFunc_B_A();
            FromFunc_B_B();
            FromFunc_B_C();
            FromFunc_B_I();

            FromFunc_C_A();
            FromFunc_C_B();
            FromFunc_C_C();
            FromFunc_C_I();

            FromFunc_I_A();
            FromFunc_I_B();
            FromFunc_I_C();
            FromFunc_I_I();

            FromAction_A();
            FromAction_B();
            FromAction_C();
            FromAction_I();

            Console.WriteLine("<%END%>");
        }
    }
}