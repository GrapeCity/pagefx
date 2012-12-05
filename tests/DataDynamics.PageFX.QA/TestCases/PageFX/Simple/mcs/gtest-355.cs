using System;

class A
{
    public virtual bool Foo(string s)
    {
        return true;
    }

    public virtual string Foo<T>(string s)
    {
        return "A::Foo<T>(string)";
    }
}

class B : A
{
    public bool Goo(string s)
    {
        return Foo(s);
    }

    public override bool Foo(string s)
    {
        return false;
    }

    public override string Foo<T>(string s)
    {
        return "B::Foo<T>(string)";
    }
}

class Test
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        B b = new B();
        Console.WriteLine(b.Foo("false"));
        Console.WriteLine(b.Foo<int>("string"));
        Console.WriteLine(b.Goo("ok"));
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}