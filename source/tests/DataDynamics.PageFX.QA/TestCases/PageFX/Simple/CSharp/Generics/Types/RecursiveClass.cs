using System;

namespace RecursiveClass
{

    class A<T>
    {
        public object Value;

        public A()
        {
            if (X.ACounter > 0)
            {
                --X.ACounter;
                Value = new B<A<T>>();
                Console.WriteLine(Value);
            }
            return;
        }

        public override string ToString()
        {
            return string.Format("A<{0}>", Value);
        }
    }

    class B<T>
    {
        public object Value;

        public B()
        {
            if (X.BCounter > 0)
            {
                --X.BCounter;
                Value = new A<B<T>>();
                Console.WriteLine(Value);
            }
            return;
        }

        public override string ToString()
        {
            return string.Format("B<{0}>", Value);
        }
    }


    class X
    {
        public static int ACounter = 10;
        public static int BCounter = 10;

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