using System;

namespace gtest372
{

    public class TestClass<T> where T : class
    {
        public bool Check(T x, T y) { return x == y; }
    }

    public class C
    {
    }

    public class TestClass2<T> where T : C
    {
        public bool Check(T x, T y) { return x == y; }
    }

    public class X
    {
        static void Test1()
        {
            Console.WriteLine("--- Test1");
            TestClass<object> a = new TestClass<object>();
            Console.WriteLine(a.Check(10, 10));
        }

        static void Test2()
        {
            Console.WriteLine("--- Test2");
            C c1 = new C();
            C c2 = new C();
            TestClass2<C> a = new TestClass2<C>();
            Console.WriteLine(a.Check(c1, c2));
            Console.WriteLine(a.Check(c1, c1));
        }

        static void Main()
        {
            Test1();
            Test2();
            Console.WriteLine("<%END%>");
        }
    }
}