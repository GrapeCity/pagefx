using System;

namespace gtest378
{
    class Test
    {
        static object Foo(int? i)
        {
            return (object)i;
        }

        static object FooG<T>(T? i) where T : struct
        {
            return (object)i;
        }

        static void Test1()
        {
            Console.WriteLine("--- Test1");
            object o = Foo(null);
            Console.WriteLine(o != null);
        }

        static void Test2()
        {
            Console.WriteLine("--- Test2");
            object o = FooG<bool>(null);
            Console.WriteLine(o != null);
        }

        static void Test3()
        {
            Console.WriteLine("--- Test3");
            object o = Foo(null);
            if (o != null)
                Console.WriteLine("error");
            else
                Console.WriteLine("ok");
        }

        public static void Main()
        {
            Test1();
            Test2();
            Test3();
            Console.WriteLine("<%END%>");
        }
    }
}
