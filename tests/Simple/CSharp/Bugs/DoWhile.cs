using System;

internal class DoWhileTest
{

    class A
    {
        public void Foo(int par)
        {
            int i = par;
            if (i < 200)
                return;
            do
            {
                if (i > 324)
                    continue;
                i--;
            } while (i > 200);  
        }
    }
    static void Test1(int param)
    {
        Console.WriteLine("--- Test1");
        var a = new A();
        a.Foo(250);
    }

    static void Main()
    {
        Test1(457);
        Console.WriteLine("<%END%>");
    }
}