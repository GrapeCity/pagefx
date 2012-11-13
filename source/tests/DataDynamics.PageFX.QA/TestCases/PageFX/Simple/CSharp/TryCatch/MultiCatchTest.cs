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
        catch (NullReferenceException)
        {
			Console.WriteLine("NullReferenceException");
        }
        catch (InvalidCastException)
        {
			Console.WriteLine("InvalidCastException");
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