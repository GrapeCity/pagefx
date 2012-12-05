// Test access to class fields outside the generic type declaration.

using System;

namespace gtest26
{
    class Foo<T>
    {
        public T Hello;

        public Foo()
        { }
    }

    class X
    {
        static void Test1()
        {
            Console.WriteLine("----Test1");
            try
            {
                Foo<int> foo = new Foo<int>();
                foo.Hello = 9;
                Console.WriteLine("ok");
            }
            catch (Exception e)
            {
                Console.WriteLine("error" + e.GetType());
            }
        }

        static void Main()
        {
            Test1();
            Console.WriteLine("<%END%>");
        }
    }
}