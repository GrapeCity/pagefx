// A non-generic type may have a closed constructed type as its parent

using System;

namespace gtest22
{
    class Foo<T>
    {
        public void Hello()
        { }

        public void World(T t)
        {
            Hello();
        }
    }

    class Bar : Foo<int>
    {
        public void Test()
        {
            Hello();
            World(4);
        }
    }

    class X
    {
        static void Test1()
        {
            Console.WriteLine("----Test1");
            try
            {
                Bar bar = new Bar();
                bar.Test();
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
