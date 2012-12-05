using System.Collections.Generic;
using System;

namespace gtest405
{
    public struct S<T>
    {
        public static Comparison<S<T>> hh = C1;

        public static int C1(S<T> x, S<T> y)
        {
            hh = C2;
            return 1;
        }

        public static int C2(S<T> x, S<T> y)
        {
            hh = C1;
            return 0;
        }
    }

    class C
    {
        public static void Test1()
        {
            Console.WriteLine("--- Test1");
            S<int>.hh(new S<int>(), new S<int>());
            S<string>.hh(new S<string>(), new S<string>());
            S<int>.hh(new S<int>(), new S<int>());
            S<string>.hh(new S<string>(), new S<string>());
        }

        public static void Main()
        {
            Test1();
            Console.WriteLine("<%END%>");
        }
    }

}
