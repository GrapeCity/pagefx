using System;

namespace gtest376
{

    struct S<T> where T : struct
    {
        public static object Box(T? o)
        {
            if (o == null)
                return null;
            return (T)o;
        }
    }

    class C
    {
        private static void Print(object obj)
        {
            if (obj == null)
                Console.WriteLine("null");
            else
                Console.WriteLine(obj);
        }

        public static void Test1()
        {
            Console.WriteLine("--- Test1");
            int? v = null;
            Print(S<int>.Box(v));
        }

        public static void Test2()
        {
            Console.WriteLine("--- Test2");
            Print(S<int>.Box(10));
        }

        public static void Main()
        {
            Test1();
            Test2();
            Console.WriteLine("<%END%>");
        }
    }

}