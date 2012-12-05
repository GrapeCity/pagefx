using System;

class A
{
    public string Name
    {
        get { return GetName(); }
    }

    protected internal virtual string GetName()
    {
        return "A";
    }
}

class B : A
{
    protected override string GetName()
    {
        return "B";
    }
}

class X
{
    static void Test1()
    {
        B b = new B();
        Console.WriteLine(doc.Name);
    }

    static void Main()
    {
        Console.WriteLine("Hello!");
        Test1();
        Console.WriteLine("<%END%>");
    }
}