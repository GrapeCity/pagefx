using System;

interface I
{
    void f();
}

class A : I
{
    #region I Members
    public void f()
    {
        Console.WriteLine("A::f()");
    }
    #endregion
}

class B : I
{
    #region I Members
    public void f()
    {
        Console.WriteLine("B::f()");
    }
    #endregion
}

class Program
{
    static void TestEquals(I a, I b)
    {
        Console.WriteLine(Equals(a, b));
    }

    static void Main()
    {
        A a = new A();
        B b = new B();
        TestEquals(a, b);
        TestEquals(a, a);
        Console.WriteLine("<%END%>");
    }
}