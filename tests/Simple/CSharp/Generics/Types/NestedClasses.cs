using System;

public class A<T>
{
    public A()
    {
        Console.WriteLine("A");
        Console.WriteLine("T = " + typeof(T));
    }

    public A(T value)
    {
        aValue = value;
        Console.WriteLine("A");
        Console.WriteLine("T = " + typeof(T));
    }

    private T aValue;

    public override string ToString()
    {
        return string.Format("A<{0}>", aValue);
    }

    public class B<T2> : A<T2>
    {
        public B(T2 value)
            : base(value)
        {
            bValue = value;
            Console.WriteLine("B");
            Console.WriteLine("T = " + typeof(T));
            Console.WriteLine("T2 = " + typeof(T2));
        }

        private T2 bValue;

        public B()
        {
            Console.WriteLine("B");
            Console.WriteLine("T = " + typeof(T));
            Console.WriteLine("T2 = " + typeof(T2));
        }
        
        public override string ToString()
        {
            return string.Format("B<{0}>", bValue);
        }

        public class C<T3> : B<T3>
        {
            public C(T3 value)
                : base(value)
            {
                cValue = value;
                Console.WriteLine("C");
                Console.WriteLine("T = " + typeof(T));
                Console.WriteLine("T2 = " + typeof(T2));
                Console.WriteLine("T3 = " + typeof(T3));
            }

            public T3 cValue;

            public C()
            {
                base.aValue = default(T3);
                base.bValue = default(T3);
            }
            
            public override string ToString()
            {
                return string.Format("C<{0}>", cValue);
            }
        }

        public class D<T2> : A<T2>
        {
            public D(T2 value)
                : base(value)
            {
                dValue = value;
                Console.WriteLine("D");
                Console.WriteLine("T = " + typeof(T));
                Console.WriteLine("T2 = " + typeof(T2));
            }

            private T2 dValue;
            
            public override string ToString()
            {
                return string.Format("D<{0}>", dValue);
            }
        }

        public class E<T> : C<T>
        {
            public T eValue;

            public E(T value)
                : base(value)
            {
                eValue = value;
                eInstance = new E<T>();
                eInstance.aValue = value;
                eInstance.bValue = value;
                eInstance.cValue = value;
                eInstance.eValue = value;
                Console.WriteLine("E");
                Console.WriteLine("T = " + typeof(T));
                Console.WriteLine("T2 = " + typeof(T2));
            }
            
            public E()
            {
            }

            public E<T> eInstance;

            public override string ToString()
            {
                return string.Format("E<{0},{1},{2},{3}>", aValue, bValue, cValue, eValue);
            }
        }
    }
}

class Test
{
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        A<double>.B<float>.E<double> a = new A<double>.B<float>.E<double>(2.718281828459045235360287);
        Console.WriteLine(a.eInstance.ToString());
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}