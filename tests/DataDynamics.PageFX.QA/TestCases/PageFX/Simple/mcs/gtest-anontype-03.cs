// Tests anonymous types initialized with object members
using System;
using System.Collections;

namespace anontype03
{
    public class MyClass
    {
        public string Foo = "Bar";
        public int Baz
        {
            get { return 42; }
        }
    }

    public class Test
    {
        static void Main()
        {
            Console.WriteLine(Test1());
            Console.WriteLine("<%END%>");
        }

        private static int Test1()
        {
            MyClass mc = new MyClass();
            var v = new { mc.Foo, mc.Baz };

            if (v.Foo != "Bar")
                return 1;
            if (v.Baz != 42)
                return 2;

            return 0;
        }
    }
}
