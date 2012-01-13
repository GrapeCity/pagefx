using System;

namespace gtest25
{
    class Foo
    {
        public Foo()
        { }

        public void Hello<T>(T t)
        {
            // We're boxing the type parameter `T' to an object here.
            Whatever(t);
        }

        public void Whatever(object o)
        {
            System.Console.WriteLine(o.GetType());
        }
    }

    class X
    {
        static void Test(Foo foo)
        {
            foo.Hello<int>(531);
        }

        static void Test1()
        {
            Console.WriteLine("----Test1");
            try
            {
                Foo foo = new Foo();
                Test(foo);
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
