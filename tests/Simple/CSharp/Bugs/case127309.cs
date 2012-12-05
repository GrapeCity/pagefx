using System;
using System.Collections;

class A
{
    public virtual IEnumerable Get()
    {
        yield return 1;
        yield return 2;
    }
}

class B : A
{
    public override IEnumerable Get()
    {
       yield return base.Get();
       yield return 3;
    }
}

class C : B
{
    public override IEnumerable Get()
    {
        yield return base.Get();
        yield return 4;
    }
}

class X
{
    static void Test1()
    {
        Console.WriteLine("--Test1");
        A a = new C();
        IEnumerable e = a.Get();
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