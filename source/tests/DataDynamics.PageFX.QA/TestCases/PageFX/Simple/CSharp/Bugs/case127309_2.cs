using System;
using System.Collections;

class A
{
    public virtual IEnumerable Nums
    {
        get
        {
            yield return 1;
            yield return 2;
        }
    }
}

class B : A
{
    public override IEnumerable Nums
    {
        get
        {
            yield return base.Nums;
            yield return 3;
        }
    }
}

class X
{
    static void Test1()
    {
        Console.WriteLine("--Test1");
        A a = new B();
        IEnumerable e = a.Nums;
        e = Unwind(e);
        foreach (var o in e)
            Console.WriteLine(o);
    }

    static IEnumerable Unwind(IEnumerable set)
    {
        foreach (var o in set)
        {
            var e = o as IEnumerable;
            if (e != null)
            {
                foreach (var o2 in Unwind(e))
                    yield return o2;
                continue;
            }
            yield return o;
        }
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}