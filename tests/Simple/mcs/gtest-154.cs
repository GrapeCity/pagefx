using System;

public delegate int T<X>(X x);

public class B
{
    public static T<X> M<X>()
    {
        return delegate(X x) { return 5; };
    }

    public static T<long> N()
    {
        return delegate(long x) { return 6; };
    }
}

public class D
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        T<double> f = B.M<double>();
        Console.WriteLine(f(5.0));
        T<long> g = B.N();
        Console.WriteLine(g(50));
    }

    public static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
