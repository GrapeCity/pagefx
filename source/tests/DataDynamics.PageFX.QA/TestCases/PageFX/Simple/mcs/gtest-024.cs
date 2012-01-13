using System;

namespace gtest24
{
    class Foo<T>
    {
        public Foo()
        { }

        public void Hello(T t)
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
        static void Test(Foo<int> foo)
        {
            foo.Hello(4);
        }

        static void Test1()
        {
            Console.WriteLine("----Test1");
            try
            {
                Foo<int> foo = new Foo<int>();
                Test(foo);
            }
            catch (Exception e)
            {
                Console.WriteLine("error " + e.GetType());
            }
        }
        
        static void Main()
        {
            Test1();
            Console.WriteLine("<%END%>");
        }
    }
}
