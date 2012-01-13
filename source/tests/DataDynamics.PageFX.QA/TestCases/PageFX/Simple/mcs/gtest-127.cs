using System;

public class A<T>
{
    public delegate void Changed(A<T> a);

    protected event Changed _changed;

    public void Register(Changed changed)
    {
        _changed += changed;
        _changed(this);
    }
}

class Test
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        A<int> a = new A<int>();
        a.Register(new A<int>.Changed(Del));
        Console.WriteLine("ok");
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }

    public static void Del(A<int> a)
    {
        System.Console.WriteLine("Solved");
    }
}
