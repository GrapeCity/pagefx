using System;

class C<T>
{
    public Type Test()
    {
        T[,] a = new T[0, 0];
        return a.GetType();
    }
}

class M
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        C<string> c1 = new C<string>();
        C<bool> c2 = new C<bool>();
        if (c1.Test() != typeof(string[,]))
            throw new InvalidCastException();
        if (c2.Test() != typeof(bool[,]))
            throw new InvalidCastException();
    }

    public static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
