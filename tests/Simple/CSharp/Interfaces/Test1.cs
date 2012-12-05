using System;

namespace NS
{
    interface I1
    {
        void f();
    }

    interface I2
    {
        void f();
    }

    class A : I1, I2
    {
        #region I1 Members
        void I1.f()
        {
            Console.WriteLine("A::I1.f()");
        }
        #endregion

        #region I2 Members
        void I2.f()
        {
            Console.WriteLine("A::I2.f()");
        }
        #endregion
    }
}

class Program
{
    static void Main()
    {
        NS.A obj = new NS.A();
        ((NS.I1)obj).f();
        ((NS.I2)obj).f();
        Console.WriteLine("<%END%>");
    }
}