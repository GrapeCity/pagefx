using System;


class A<T>
{
    public static T GetDefault()
    {
        return default(T);
    }

    public T GetDefaultNonStatic()
    {
        return default(T);
    }
}

class B<T> : A<T>
{
    public T GetDefaultNonStaticB()
    {
        return default(T);
    }
}

class C<T> : B<T>
{ 
}

class A<T1, T2> : A<T1>
{
    public static T2 GetDefault2()
    {
        return A<T2>.GetDefault();
    }
}

class A<T1, T2, T3> : A<T1, T2>
{
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
        
        B<bool> b = new B<bool>();
        Console.WriteLine(b.GetDefaultNonStatic());
        Console.WriteLine(b.GetDefaultNonStaticB());

        C<bool> c = new C<bool>();
        Console.WriteLine(c.GetDefaultNonStaticB());


        Console.WriteLine(A<bool, int>.GetDefault());
        Console.WriteLine(A<bool, int>.GetDefault2());

        Console.WriteLine(A<bool, int, string>.GetDefault());
        Console.WriteLine(A<bool, int, string>.GetDefault2());
        Console.WriteLine(A<bool, int, string>.GetDefault3());
        Console.WriteLine("<%END%>");
    }
}