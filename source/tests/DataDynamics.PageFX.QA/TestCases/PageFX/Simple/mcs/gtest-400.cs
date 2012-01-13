using System;

namespace gtest400
{
    class Gen<T> where T : class
    {
        public static bool Foo(T t)
        {
            return t is Program;
        }
    }

    class Program
    {
        static bool Foo<T>()
        {
            object o = 1;
            return o is T;
        }

        static void Main()
        {
            Console.WriteLine(Foo<bool>());
            Console.WriteLine(!Foo<int>());

            Console.WriteLine(Gen<object>.Foo(null));
            Console.WriteLine(!Gen<Program>.Foo(new Program()));

            Console.WriteLine("<%END%>");
        }
    }
}

