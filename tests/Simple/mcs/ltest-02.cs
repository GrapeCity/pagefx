
//
// Lambda expression test overload resolution with parameterless arguments
//

using System;

namespace ltest02
{
    delegate string funcs(string s);
    delegate int funci(int i);

    class X
    {
        static void Foo(funci fi)
        {
            int res = fi(10);
            Console.WriteLine(res);
        }

        static void Foo(funcs fs)
        {
            string res = fs("hello");
            Console.WriteLine(res);
        }

        static void Test1()
        {
            Console.WriteLine("----Test1");
            Foo(x => x + "dingus");
        }

        static void Main()
        {
            Test1();
            Console.WriteLine("<%END%>");
        }
    }
}
