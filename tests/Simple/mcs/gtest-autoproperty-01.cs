
// Tests automatic properties
using System;

namespace autoprop01
{
    public class Test
    {
        private class A
        {
            public string B { get; set; }
        }

        public string Foo { get; set; }
        public int Answer { get; private set; }

        public Test()
        {
            Answer = 42;
        }

        static void Main()
        {
            Console.WriteLine(Test1());
            Console.WriteLine("<%END%>");
        }

        private static int Test1()
        {
            Test t = new Test();
            t.Foo = "Bar";
            if (t.Foo != "Bar")
                return 1;

            if (t.Answer != 42)
                return 2;

            A a = new A();
            a.B = "C";
            if (a.B != "C")
                return 3;

            return 0;
        }
    }
}
