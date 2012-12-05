using System;

class A<T>
{
    public static T GetDefault()
    {
        return default(T);
    }
}

class B<T>
{
    public static T GetDefault()
    {
        return A<T>.GetDefault();
    }
}


class A<T1, T2>
{
    public static T1 GetDefault()
    {
        return A<T1>.GetDefault();
    }

    public static T2 GetDefault2()
    {
        return A<T2>.GetDefault();
    }
}

class A<T1, T2, T3>
{
    public static T1 GetDefault()
    {
        return A<T1>.GetDefault();
    }

    public static T2 GetDefault2()
    {
        return A<T2>.GetDefault();
    }

    public static T3 GetDefault3()
    {
        return A<T3>.GetDefault();
    }
}

class Test
{
    static void Main()
    {
        Console.WriteLine(A<bool>.GetDefault());
        Console.WriteLine(B<bool>.GetDefault());
        Console.WriteLine(B<int>.GetDefault());

        Console.WriteLine(A<bool, int>.GetDefault());
        Console.WriteLine(A<bool, int>.GetDefault2());

        Console.WriteLine(A<bool, int, string>.GetDefault());
        Console.WriteLine(A<bool, int, string>.GetDefault2());
        Console.WriteLine(A<bool, int, string>.GetDefault3());
        Console.WriteLine("<%END%>");
    }
}