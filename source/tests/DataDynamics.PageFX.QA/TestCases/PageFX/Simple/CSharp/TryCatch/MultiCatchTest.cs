using System;

interface I
{
    void Throw();
}

class A : I
{
    public void Throw()
    {
        throw new NotImplementedException();
    }
}

class B : I
{
    public void Throw()
    {
        throw new InvalidCastException();
    }
}

class C : I
{
    public void Throw()
    {
        throw new NullReferenceException();
    }
}

class X
{
    static void foo(I obj)
    {
        try
        {
            obj.Throw();
        }
        catch (NullReferenceException e1)
        {
            Console.WriteLine(e1.GetType());
        }
        catch (InvalidCastException e2)
        {
            Console.WriteLine(e2.GetType());
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
        finally
        {
            Console.WriteLine("finally");
        }
    }

    static void Main()
    {
        foo(null);
        foo(new A());
        foo(new B());
        foo(new C());
        Console.WriteLine("<%END%>");
    }
}