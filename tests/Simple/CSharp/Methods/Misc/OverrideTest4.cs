using System;

interface I
{
    int Foo(int v);
}

class A : I
{
    public virtual int Foo(int v)
    {
        Console.WriteLine("A::Foo");
        return v + 1;
    }
}

class B : A
{
	private int BaseFoo(int v)
	{
		return base.Foo(v);
	}

    public override int Foo(int v)
    {
        Console.WriteLine("B::Foo");
        return BaseFoo(v) + 2;
    }
}

class C : B
{
    private int BaseFoo(int v)
	{
		return base.Foo(v);
	}

    public override int Foo(int v)
    {
        Console.WriteLine("C::Foo");
        return BaseFoo(v) + 3;
    }
}

class D : C
{
    private int BaseFoo(int v)
	{
		return base.Foo(v);
	}

    public override int Foo(int v)
    {
        Console.WriteLine("D::Foo");
        return BaseFoo(v) + 4;
    }
}

class Test
{
    static void Main()
    {
        var o = new D();
        Console.WriteLine(o.Foo(5));
        Console.WriteLine("<%END%>");
    }
}