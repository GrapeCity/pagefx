using System;
using TestNamespace;

namespace TestNamespace
{
    class A
    {
        public void f()
        {
            Console.WriteLine("A::f()");
        }
    }
}

namespace DD
{
    class Test
    {
        static void Main()
        {
            A obj = new A();
            obj.f();
            Console.WriteLine("<%END%>");
        }
    }
}