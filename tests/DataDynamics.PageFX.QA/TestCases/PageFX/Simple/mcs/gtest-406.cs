using System;
using System.Collections.Generic;

namespace gtest406
{
    delegate R Func<T,R>(T arg);

    class Test<T>
    {
        public void Foo<TOutput>(IEnumerable<T> t, Func<T, TOutput> converter)
        {
            Console.WriteLine(typeof(TOutput));
            foreach (T item in t)
            {
                Console.WriteLine(converter(item));
            }
        }
    }

    public class C<A, B>
        where A : class
        where B : class
    {
        public C(IEnumerable<B> t)
        {
            new Test<B>().Foo<A>(t, 
                delegate(B b)
                {
                    Console.WriteLine(b);
                    return b as A;
                });
        }
    }

    class M
    {
        static void Test1()
        {
            Console.WriteLine("----0");
            new C<object, object>((object)(new object[0]) as IEnumerable<object>);
            Console.WriteLine("----1");
            new C<object, object>((object)(new string[] { "1", "2", "3" }) as IEnumerable<object>);
            Console.WriteLine("----2");
            new C<object, string>((object)(new string[] { "1", "2", "3" }) as IEnumerable<string>);
            Console.WriteLine("----3");
            new C<string, string>((object)(new string[] { "1", "2", "3" }) as IEnumerable<string>);
        }

        public static void Main()
        {
            Test1();
            Console.WriteLine("<%END%>");
        }
    }

}