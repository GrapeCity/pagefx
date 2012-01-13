using System;

class A
{
    public int Value;
    public string Name;

    public A()
    {
        Console.WriteLine("A()");
    }

    public A(int value)
    {
        Console.WriteLine("A(int value)");
        Value = value;
    }

    public A(string name)
        : this(0)
    {
        Console.WriteLine("A(string name)");
        Name = name;
    }

    public A(string name, int value)
        : this(name)
    {
        Console.WriteLine("A(string name, int value)");
        Value = value;
    }
}

class B : A
{
    public B() : base("aaa")
    {
    }
}

class CtorTest
{
    static void f(A obj)
    {
        Console.WriteLine(obj.Value);
        Console.WriteLine(obj.Name);
    }

    static void Test1()
    {
        f(new A());
        f(new A(100));
        f(new A("name"));
        f(new A("name", 100));
    }

    static void Test2()
    {
        f(new B());
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}