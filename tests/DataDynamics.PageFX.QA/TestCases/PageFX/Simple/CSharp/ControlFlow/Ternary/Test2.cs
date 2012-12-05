using System;
using System.Collections;

class A
{
    public IComparer comparer;

    public void Test1()
    {
        IComparer cmp = comparer == null ? Comparer.Default : comparer;
        Console.WriteLine(cmp.Compare("a", "a"));
    }

    public void Test2()
    {
        IComparer cmp = comparer ?? Comparer.Default;
        Console.WriteLine(cmp.Compare("a", "a"));
    }
}

class Program
{
    static void Main()
    {
        A a = new A();
        a.Test1();
        a.Test2();
        Console.WriteLine("<%END%>");
    }
}