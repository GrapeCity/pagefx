using System;

interface IAction
{
    void Do();
}

class A1 : IAction
{
    #region IAction Members
    public void Do()
    {
        Console.WriteLine("action1");
    }
    #endregion
}

class A2 : IAction
{
    #region IAction Members
    public void Do()
    {
        Console.WriteLine("action2");
    }
    #endregion
}

class Virtual1
{
    static void Main()
    {
        IAction a = new A1();
        a.Do();
        a = new A2();
        a.Do();
        Console.WriteLine("<%END%>");
    }
}