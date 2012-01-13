using System;

class A
{
    public void f()
    {
        Console.WriteLine("A.f()");
    }
}

class B : A
{
    public void f(int a)
    {
        Console.WriteLine("B.f(int)");
    }
}

class Test2
{
    static void Main()
    {
        B obj = new B();
        obj.f();  
        obj.f(0);
        Console.WriteLine("<%END%>");
    }
}