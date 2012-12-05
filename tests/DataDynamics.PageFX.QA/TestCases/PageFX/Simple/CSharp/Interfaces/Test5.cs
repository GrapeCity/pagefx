using System;

interface I
{
    int Count { get; }
}

class A : I
{
    public int[] arr;

    public A()
    {
        arr = new int[] { 10, 20, 30 };
    }

    #region I Members
    int I.Count
    {
        get { return Length; }
    }
    #endregion

    public int Length
    {
        get { return arr.Length; }
    }
}

class Program
{
    static void Main()
    {
        A a = new A();
        Console.WriteLine(a.Length);
        Console.WriteLine(((I)a).Count);
        Console.WriteLine("<%END%>");
    }
}