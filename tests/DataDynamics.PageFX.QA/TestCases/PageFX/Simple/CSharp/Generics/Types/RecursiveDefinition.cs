using System;

public interface ISuper<T>
{
    INumber GetNumber();
}

public interface IOther<T>
{
    T GetOtherT();
}

public interface INumber
{
    int ToInt32();
}

public class IntNumber : INumber
{
    public int Value;

    public IntNumber()
    {
    }
    
    public IntNumber(int value)
    {
        Value = value;
    }

    public int ToInt32()
    {
        return Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

public class G<T1, T2> : ISuper<T1>, IOther<C1<T2>>
    where T1 : class, INumber, new()
    where T2 : class, INumber, new()
{
    public T1 V1;
    public T2 V2;

    public void Test(object value)
    {
        T1 v = new T1();
        v.ToInt32();
    }

    public int TryAs(object value)
    {
        T1 v = value as T1;
        if (v == null)
            throw new NotImplementedException();
        return v.ToInt32();
    }

    public bool IsT1(object value)
    {
        return value is T1;
    }

    public int TryCast(object value)
    {
        return ((T1)value).ToInt32();
    }
    
    public INumber GetNumber()
    {
        int v = V1.ToInt32() + V2.ToInt32();
        return new IntNumber(v);
    }
    
    
    public C1<T2> GetOtherT()
    {
        C1<T2> v = new C1<T2>();
        v.V2 = new IntNumber(100);
        return v;
    }
}

public class C1<T> : G<T, IntNumber>
    where T : class, INumber, new()
{
}


public class C2<T> : G<C2<T>, C2<T>>, INumber
    where T : class, INumber, new()
{
    public int ToInt32()
    {
        return V1.V1.V1.V1.V1.ToInt32();
    }
}

public class A<T>: INumber {
    public int ToInt32()
    {
        throw new System.NotImplementedException();
    }
}

class Test
{
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        try
        {
            G<C2<A<int>>, C2<A<double>>> g = new G<C2<A<int>>, C2<A<double>>>();
            Console.WriteLine(g.TryCast(0));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        try
        {
            G<C2<A<int>>, C2<A<double>>> g = new G<C2<A<int>>, C2<A<double>>>();
            Console.WriteLine(g.TryAs(new C2<A<int>>()));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}