// Compiler options: -langversion:default

using System;

namespace partial03
{
    public partial class Test
    {
        public readonly Foo TheFoo;

        public Test()
        {
            this.TheFoo = new Foo();
        }

        public partial interface IFoo
        {
            int Hello(Test foo);
        }

        public int TestFoo()
        {
            return TheFoo.Hello(this);
        }
    }

    public partial class Test
    {
        public partial class Foo : IFoo
        {
            int IFoo.Hello(Test test)
            {
                return 2;
            }

            public int Hello(Test test)
            {
                return 1;
            }
        }

        public int TestIFoo(IFoo foo)
        {
            return foo.Hello(this);
        }
    }

    class X
    {
        static void Main()
        {
            Console.WriteLine(Test1());
            Console.WriteLine("<%END%>");
        }

        private static int Test1()
        {
            Console.WriteLine("----Test1");
            Test test = new Test();
            if (test.TestFoo() != 1)
                return 1;
            if (test.TestIFoo(test.TheFoo) != 2)
                return 2;
            return 0;
        }
    }
}
