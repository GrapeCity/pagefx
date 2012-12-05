using System;

class Test
{
    class B<T>
    {}

    class G<T>: B<T>
    { 
    }

    
    class A<T>
    {
        private T firstProp;
        public T FirstProp
        {
            get
            {
                return firstProp;
            }
            set
            {
                if (value is int)
                {
                    firstProp = value;
                }
                else
                {
                    throw new NotSupportedException("only integer type is appropriate");
                }
            }
        }

        private A<T> _instance;

        public A<T> Instance
        {
            get
            {
                return (_instance);
            }
            set
            {
                if (_instance != value)
                    _instance = value;
                else
                    throw new NotSupportedException("you can't assign twice");
            }
        }

        private B<T> _binstance;
        public B<T> BInstance
        {
            get
            {
                return (_binstance);
            }
            set
            {
                if (_binstance != value)
                    _binstance = value;
                else
                    throw new NotSupportedException("you can't assign twice");
            }
        }
    }


    static void Test1()
    {
        Console.WriteLine("--- Test1");
        A<int> a = new A<int>();
        a.FirstProp = 42;
        Console.WriteLine("a.FirstProp = {0}", a.FirstProp);
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        A<double> a = new A<double>();
        try
        {
            a.FirstProp = 2.718281828459045235360287;
        }
        catch (Exception e)
        {
            Console.WriteLine("exception {0}({1})", e.GetType(), e.Message);
        }
    }

    static void Test3()
    {
        Console.WriteLine("--- Test3");
        A<double> a = new A<double>();
        
        // first time
        try
        {
            a.Instance = a;
            Console.WriteLine("ok");
        }
        catch (Exception)
        {
            Console.WriteLine("error");
        }
        
        // second time
        try
        {
            a.Instance = a;
            Console.WriteLine("error");
        }
        catch (Exception e)
        {
            Console.WriteLine("exception {0}({1})", e.GetType(), e.Message);
        }

        // third time
        try
        {
            a.Instance = new A<double>();
            Console.WriteLine("ok");
        }
        catch (Exception e)
        {
            Console.WriteLine("exception {0}({1})", e.GetType(), e.Message);
        }
    }

    static void Test4()
    {
        Console.WriteLine("--- Test4");
        A<string> a = new A<string>();
        B<string> b = new B<string>();
        
        // first time
        try
        {
            a.BInstance = b;
            Console.WriteLine("ok");
        }
        catch (Exception)
        {
            Console.WriteLine("error");
        }

        // second time
        try
        {
            a.BInstance = new G<string>();
            Console.WriteLine("error");
        }
        catch (Exception e)
        {
            Console.WriteLine("exception {0}({1})", e.GetType(), e.Message);
        }
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Test4();
        Console.WriteLine("<%END%>");
    }
}