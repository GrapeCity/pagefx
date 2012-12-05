// Tests anonymous types
using System;
using System.Collections;

namespace anontype01
{
    public class Test
    {
        static int Test1()
        {
            var v = new { Foo = "Bar", Baz = 42 };
            if (v.Foo != "Bar")
                return 1;
            if (v.Baz != 42)
                return 2;
            return 0;
        }

        static void Main()
        {
            Console.WriteLine(Test1());
            Console.WriteLine("<%END%>");
        }
    }
}
