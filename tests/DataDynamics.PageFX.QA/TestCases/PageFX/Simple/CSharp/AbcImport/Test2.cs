using System;
using Shapes;

class A : Shape
{
    public override void Draw()
    {
        Console.WriteLine("A::Draw");
        base.Draw();
    }
}

class Test2
{
    static void Test1()
    {
        try
        {
            Line line = new Line();
            line.Draw();
            A a = new A();
            a.Draw();
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.GetType());
        }
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}