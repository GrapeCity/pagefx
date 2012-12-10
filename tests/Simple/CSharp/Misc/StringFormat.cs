using System;

class StringFormatTest
{
    class E
    {
        int x;
        public E(int _x)
        {
            x = _x;
        }

        public override string ToString()
        {
            return x.ToString();
        }
    }

    class B
    { 
        int a;
        long b;
        E e;

        public B(int _a, int _b, int _e)
        {
            a = _a;
            b = _b;
            e = new E(_e);
        }

        public override string ToString()
        {
            return string.Format("a: {0}, b:{1}, e:{2}", a, b, e);
        }
    }

    class Foo
    {
        int x1;
        long x2;
        ulong x3;
        float x4;
        double x5;
        
        private B b1;
        private B b2;
        private B b3;
        private B b4;

        public Foo(int value1, int value2)
        {
            x1 = value1;
            x2 = x1 + value1;
            x3 = (ulong)(x2 + value1);
        }
    }

    static void Test1()
    {
        Console.WriteLine("----Test1");
        long lx = 123456789012345667;
        int ix = 123456789;
        float fx = (float)0.1234;
        int count = Environment.TickCount;
        for (int i = 0; i < 10000000000; i++)
        {
            Console.WriteLine();
        }
        
        Console.WriteLine(Environment.TickCount - count);


        count = Environment.TickCount;
        for (int i = 0; i < 10000000000; i++)
        {
            Console.WriteLine("lx.ToString(): " + i.ToString());
        }
        Console.WriteLine(Environment.TickCount - count);
    }

    public static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}