using System;

abstract class Action
{
    public abstract void Do();
}

class A1 : Action
{
    public override void Do()
    {
        Console.WriteLine("action1");
    }
}

class A2 : Action
{
    public override void Do()
    {
        Console.WriteLine("action2");
    }
}

class Virtual2
{
    static void Main()
    {
        Action a = new A1();
        a.Do();
        a = new A2();
        a.Do();
        Console.WriteLine("<%END%>");
    }
}