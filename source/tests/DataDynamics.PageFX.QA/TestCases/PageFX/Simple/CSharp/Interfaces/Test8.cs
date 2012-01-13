using System;

interface I
{
    void f();
}

class A : I
{
    #region I Members
    void I.f()
    {
        Console.WriteLine("A::f()");
    }
    #endregion
}

class B : A, I
{
    #region I Members
    void I.f()
    {
        Console.WriteLine("B::f()");
    }
    #endregion
}

class Program
{
    static void Test(I i)
    {
        i.f();
    }

    static void Main()
    {
        Test(new A());
        Test(new B());
        Console.WriteLine("<%END%>");
    }
}
