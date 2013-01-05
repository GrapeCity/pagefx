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
    protected internal override string GetName()
    {
        return "B";
    }
}

class X
{
    static void Test1()
    {
        var o = new B();
        Console.WriteLine(o.Name);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}