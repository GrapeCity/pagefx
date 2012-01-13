

using System;
using System.Reflection;

namespace CosntructorInfoTest
{
    
    class A
    {
        private int m_i;

        public void ViewI()
        {
            Console.WriteLine("m_i: {0}", m_i);
        }

        public A()
        {
            Console.WriteLine("A()");
            m_i = 42;
        }

        public A(int val)
        {
            Console.WriteLine("A(int val)");
            m_i = val;
        }
    }

    class Test
    {
        

        static void InvokeConstructorWithObject(ConstructorInfo ctor, A a, object[] args)
        {
            object b = null;
            try
            {
                if (ctor != null)
                {
                    b = ctor.Invoke(a, args);
                    if (b != null)
                    {
                        var bb = b as A;
                        if (bb != null)
                        {
                            bb.ViewI();
                        }
                        Console.WriteLine(b.GetType());

                    }
                    else
                    {
                        Console.WriteLine("b is null");
                    }
                }
                else
                {
                    Console.WriteLine("ctor is null");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        static void InvokeConstructorWithoutObject(ConstructorInfo ctor, object[] args)
        {
            object b = null;
            try
            {
                if (ctor != null)
                {
                    b = ctor.Invoke(args);
                    if (b != null)
                    {
                        var bb = b as A;
                        if (bb != null)
                        {
                            bb.ViewI();
                        }
                        Console.WriteLine(b.GetType());

                    }
                    else
                    {
                        Console.WriteLine("b is null");
                    }
                }
                else
                {
                    Console.WriteLine("ctor is null");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        static void Test1()
        {
            Console.WriteLine("----Test1");
            
            ConstructorInfo ctor = null;
            A a = null;

            try
            {
                a = new A(32);
                a.ViewI();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
            

            try
            {
                Console.WriteLine("---- ctor with parameter");
                ctor = typeof(A).GetConstructor(new[] { typeof(Int32) });
                InvokeConstructorWithoutObject(ctor, new object[] { 5 });
                Console.WriteLine("---- ctor without parameter");
                ctor = typeof(A).GetConstructor(Type.EmptyTypes);
                InvokeConstructorWithoutObject(ctor, null);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        static void Test2()
        {
            Console.WriteLine("----Test2");

            ConstructorInfo ctor = null;
            A a = null;

            try
            {
                a = new A(32);
                a.ViewI();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }


            try
            {
                Console.WriteLine("---- ctor with parameter");
                ctor = typeof(A).GetConstructor(new[] { typeof(Int32) });
                InvokeConstructorWithObject(ctor, a, new object[] { 5 });
                Console.WriteLine("---- ctor without parameter");
                ctor = typeof(A).GetConstructor(Type.EmptyTypes);
                InvokeConstructorWithObject(ctor, a, null);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        static void Main()
        {
            Test1();
            Test2();
            Console.WriteLine("<%END%>");
        }
    }
}