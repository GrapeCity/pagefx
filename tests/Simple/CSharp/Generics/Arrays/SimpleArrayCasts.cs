using System;

interface I
{
    void f();
}

internal struct S
{
    private int x;
    private int y;

    public override string ToString()
    {
        return string.Format("S{{{0}, {1}}}", x, y);
    }
}

class B<T>
{
    private T value;

    public override string ToString()
    {
        return string.Format("B<{0}>", value);
    }
}

class A<T>
{
    public T t;

    public I[] CastToI()
    {
        return (t as I[]);
    }
    
    public S[] CastToS()
    {
        return (t as S[]);
    }
    
    public int[] CastToInt()
    {
        return (t as int[]);
    }

    public uint[] CastToUInt()
    {
        return (t as uint[]);
    }

    public float[] CastToSingle()
    {
        return (t as float[]);
    }

    public double[] CastToDouble()
    {
        return (t as double[]);
    }

    public object[] CastToObj()
    {
        
        return (t as object[]);
    }
    
    public B<T>[] CastToB()
    {
        return (t as B<T>[]);
    }
    
    public override string ToString()
    {
        return string.Format("a<{0}>", t);
    }
}


struct S2 : I 
{
    #region Implementation of I

    public void f()
    {
        throw new System.NotImplementedException();
    }

    #endregion
}

class C<T> : A<T>
{
    public C(T val)
    {
        base.t = val;
    }

    public A<T>[] CastCToA(C<T>[] c)
    {
        return (c as A<T>[]);
    }

    public override string ToString()
    {
        return string.Format("C<{0}>", base.t);
    }
}

class Test
{
    static void Test1()
    {
    }

    static void Test2()
    {
        A<I> a = new A<I>();
        Console.WriteLine("\n----CastToI------\n" + (a.CastToI() == null));
        Console.WriteLine("\n----CastToS------\n" + (a.CastToS() == null));
        Console.WriteLine("\n----CastToInt----\n" + (a.CastToInt() == null));
        Console.WriteLine("\n----CastToUInt---\n" + (a.CastToUInt() == null));
        Console.WriteLine("\n----CastToObj----\n" + (a.CastToObj() == null));
        Console.WriteLine("\n----CastToB------\n" + (a.CastToB() == null));
        C<C<int>> c = new C<C<int>>(new C<int>(7));
        Console.WriteLine("\n----CastCToA------\n" + (c.CastCToA(new C<C<int>>[5]) == null));
        Console.WriteLine(c);
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}