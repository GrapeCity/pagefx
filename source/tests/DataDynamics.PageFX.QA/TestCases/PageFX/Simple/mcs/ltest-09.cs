using System;
using System.Collections.Generic;

namespace ltest09
{
    delegate TD Func<TD>();
    delegate TR Func<TA, TR>(TA arg);

    public class C
    {
        static IEnumerable<T> Test<T>(T t)
        {
            return null;
        }

        static IEnumerable<T> Test<T>(Func<T> f)
        {
            return null;
        }

        static IEnumerable<T> Test2<T>(Func<T, T> f)
        {
            return null;
        }

        static void Test1()
        {
            Console.WriteLine("----Test1");
            IEnumerable<int> ie = Test(1);
            IEnumerable<string> se;
            se = Test(() => "a");
            se = Test(delegate() { return "a"; });
            se = Test2((string s) => "s");
        }

        public static void Main()
        {
            Test1();
            Console.WriteLine("<%END%>");
        }
    }
}

