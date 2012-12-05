using System;

class A<T>
{
    public static T GetDefault()
    {
        return default(T);
    }
}

class A<T1, T2>
{
    public static T1 GetDefault()
    {
        return default(T1);
    }

    public static T2 GetDefault2()
    {
        return default(T2);
    }
}

class A<T1, T2, T3>
{
    public static T1 GetDefault()
    {
        return default(T1);
    }

    public static T2 GetDefault2()
    {
        return default(T2);
    }

    public static T3 GetDefault3()
    {
        return default(T3);
    }
}

class Test
{
    static void Main()
    {
        Console.WriteLine(A<bool>.GetDefault());

        Console.WriteLine(A<bool, int>.GetDefault());
        Console.WriteLine(A<bool, int>.GetDefault2());

        Console.WriteLine(A<bool, int, string>.GetDefault());
        Console.WriteLine(A<bool, int, string>.GetDefault2());
        Console.WriteLine(A<bool, int, string>.GetDefault3());
        Console.WriteLine("<%END%>");
    }
}