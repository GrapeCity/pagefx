using System;

namespace NS1
{
    interface I
    {
        void f();
    }
}

namespace NS2
{
    interface I
    {
        void f();
    }
}

class A : NS1.I, NS2.I
{
    #region I Members
    void NS1.I.f()
    {
        Console.WriteLine("NS1.I.f()");
    }
    #endregion

    #region I Members
    void NS2.I.f()
    {
        Console.WriteLine("NS2.I.f()");
    }
    #endregion
}

class Program
{
    static void Main()
    {
        A obj = new A();
        ((NS1.I)obj).f();
        ((NS2.I)obj).f();
        Console.WriteLine("<%END%>");
    }
}