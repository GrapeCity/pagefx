using System;

class Test
{
    class B
    {
        public virtual void Foo<T>()
        {
            Console.WriteLine("B.Foo<T>");
        }
    }

    abstract class C
    {
        public abstract void Bar<T>();
    }

    class B1 : B
    {
        public override void Foo<T>()
        {
            base.Foo<T>();
            Console.WriteLine("B1.Foo");
        }
    }

    class C1 : C
    {
        public override void Bar<T>()
        {
            Console.WriteLine("C1.Bar");
        }
    }

    static void Test1()
    {
        Console.WriteLine("----Test1");
        B1 b = new B1();
        b.Foo<int>();
    }
    
    static void Test2()
    {
        Console.WriteLine("----Test2");
        C1 c = new C1();
        c.Bar<double>();
    }


    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}