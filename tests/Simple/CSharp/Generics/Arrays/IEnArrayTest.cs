using System;
using System.Collections;
using System.Collections.Generic;

namespace IEnArrayTest
{

    class A<T>
    {
        public IEnumerable<T> Foo(object arg)
        {
            return (arg as IEnumerable<T>);
        }
    }

    class Program
    {
        static void Test1()
        {
            Console.WriteLine("----Test1");
            try
            {
                A<int> a = new A<int>();
                foreach (object i in a.Foo(new int[] {1, 2, 3, 4, 5}))
                {
                    Console.WriteLine("i = {0}", i);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());
            }
        }

        static void Test2()
        {
            Console.WriteLine("----Test2");
            try
            {
                A<char> a = new A<char>();
                foreach (char i in a.Foo("Hello World!"))
                {
                    Console.WriteLine("i = {0}", i);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());
            }
        }

        static void Main()
        {
            Test1();
            Test2();
            Console.WriteLine("<%END%>");
        }
    }
}
