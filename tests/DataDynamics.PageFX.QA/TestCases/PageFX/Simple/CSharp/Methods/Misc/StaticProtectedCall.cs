using System;

class A
{
    static protected string str;
    protected string m_str;

    static protected void foo()
    {
        Console.WriteLine("A::foo()");
    }
}

class B : A
{
    public void f()
    {
        foo();
        Console.WriteLine("B::f");

        str = "aaa";
        Console.WriteLine(str);

        m_str = "bbb";
        Console.WriteLine(m_str);
    }
}

class StaticProtectedCallTest
{
    static void Main()
    {
        B obj = new B();
        obj.f();
        Console.WriteLine("<%END%>");
    }
}