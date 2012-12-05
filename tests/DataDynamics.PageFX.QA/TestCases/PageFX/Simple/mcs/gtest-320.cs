using System;

namespace Gtest320
{
    public class Foo<K>
    { }

    partial class B
    { }

    partial class B : Foo<B.C>
    {
        public class C
        {
            public void PrintOk()
            {
                Console.WriteLine("OK");
            }
                
        }

    }

    class X
    {
        static void Test1()
        {
            Console.WriteLine("----Test1");
            B b = new B();
            B.C c = new B.C();
            c.PrintOk();
        }

        static void Main()
        {
            Test1();
            Console.WriteLine("<%END%>");
        }
    }

}
