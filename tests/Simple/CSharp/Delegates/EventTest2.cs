using System;

class X
{
    public event EventHandler Hook;

    public void Raise()
    {
        if (Hook == null)
            throw new Exception("error");

        Hook(this, EventArgs.Empty);
    }

    static void foo(object sender, EventArgs args)
    {
        Console.WriteLine("Hook invoked");
    }

    static void Main()
    {
        X x = new X();
        x.Hook += foo;
        x.Raise();
        Console.WriteLine("<%END%>");
    }
}