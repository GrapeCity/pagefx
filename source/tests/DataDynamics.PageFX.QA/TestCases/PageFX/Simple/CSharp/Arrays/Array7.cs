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
        Console.WriteLine("A1::Do()");
    }
    #endregion
}

class A2 : IAction
{
    #region IAction Members
    public void Do()
    {
        Console.WriteLine("A2::Do()");
    }
    #endregion
}

class A3 : IAction
{
    #region IAction Members
    public void Do()
    {
        Console.WriteLine("A3::Do()");
    }
    #endregion
}

class Array7
{
    static void Main()
    {
        IAction[] arr = new IAction[] { new A1(), new A2(), new A3() };
        for (int i = 0; i < arr.Length; ++i)
            arr[i].Do();
        Console.WriteLine("<%END%>");
    }
}