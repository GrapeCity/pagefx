using System;

namespace X
{
    class A
    {
        public class B
        {
            public class C
            {
                public class D
                {
                    public void Foo()
                    {
                        Console.WriteLine("Hello!");
                    }
                }
            }
        }
    }
}

class Test
{
    static void Main()
    {
        var obj = new X.A.B.C.D();
        obj.Foo();
        Console.WriteLine("<%END%>");
    }
}