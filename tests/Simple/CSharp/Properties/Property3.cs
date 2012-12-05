using System;

class A
{
    public virtual int Value
    {
        get
        {
            Console.WriteLine("A::Value");
            return 10;
        }
    }
}

class B : A
{
    public override int Value
    {
        get
        {
            Console.WriteLine("B::Value");
            return base.Value + 10;
        }
    }
}

class Property3
{
    static void Main()
    {
        A obj = new B();
        Console.WriteLine(obj.Value);
        Console.WriteLine("<%END%>");
    }
}