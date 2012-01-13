using System;

namespace PageFX.Tests
{
    class A
    {
        public virtual void f()
        {
            Console.WriteLine("A::f()");
        }
    }

    class B : A
    {
        public override void f()
        {
            base.f();
            Console.WriteLine("B::f()");
        }
    }

    class Program
    {
        static void Main()
        {
            B b = new B();
            b.f();
            Console.WriteLine("<%END%>");
        }
    }
}