using System;

namespace RecursiveClasLimit
{

    class A<T>
    {
        public A()
        {
            if (X.ACounter > 0)
            {
                --X.ACounter;
                B<A<T>> b = new B<A<T>>();
            }
            return;
        }
    }

    class B<T>
    {
        public B()
        {
            if (X.BCounter > 0)
            {
                --X.BCounter;
                A<B<T>> a = new A<B<T>>();
            }
            return;
        }
    }


    class X
    {
        public static int ACounter = 50;
        public static int BCounter = 50;

        static void Test1()
        {
            Console.WriteLine("----Test1");
            try
            {
                B<int> b = new B<int>();
                Console.WriteLine("ok");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());
            }
        }

        static void Main()
        {
            Test1();
            Console.WriteLine("<%END%>");
        }

    }


}