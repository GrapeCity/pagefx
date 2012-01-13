using System;
using System.Collections.Generic;
using System.Text;

namespace TestParametrAttribute{


    class AAttribute : Attribute
    {
        public string msg;
    }

    class BAttribute : Attribute
    {
        public int[] arr;
    }

    //class MBase
    //{
    //    public virtual void Foo([A(msg="Hi, this a AAtribute")]int a)
    //    {
    //        Console.WriteLine("MBase");
    //    }
    //}

    class ClassD
    {
        public ClassD() { }

        public ClassD(string m)
        {
            msg = m;
        }

        public string msg = "Hi, this is just a few moments.";
    }

    class MDer //: MBase
    {
        public MDer() {}
        
        public MDer(ClassD d)
        {
            Console.WriteLine(d.msg);
        }

        public static void Foo([B(arr = new int[] {1, 2, 3, 4})]int a)
        {
            Console.WriteLine("MDer");
        }
        
        public void Bar([B(arr = new int[] { 1, 2, 3, 4 })]double super)
        {
            Console.WriteLine("MDer" + super);
        }

        public void Qaz()
        {
            Console.WriteLine("Mder:Qaz");
        }
    }

    class ParametrAttribute
    {
        static void Test1()
        {
            Console.WriteLine("----Test1");
            var b = new MDer();
            //b.Foo(5);
            MDer.Foo(5);

            var method = b.GetType().GetMethod("Foo");
            if (method == null)
            {
                Console.WriteLine("Method is null");
                return;
            }
            
            var parameters = method.GetParameters();
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    if (param != null)
                        Console.WriteLine(param);
                    else
                        Console.WriteLine("null");
                }
            }
            else
            {
                Console.WriteLine("Parameters is null");
            }

            Console.WriteLine("Invoke:");

            method.Invoke(b, (object[])(new object[] {(object)(5)}));
        }

        static void Test2()
        {
            Console.WriteLine("----Test2");
            var b = new MDer();
            b.Bar(5.5);
            

            var method = b.GetType().GetMethod("Bar");
            if (method == null)
            {
                Console.WriteLine("Method is null");
                return;
            }

            var parameters = method.GetParameters();
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    if (param != null)
                        Console.WriteLine(param);
                    else
                        Console.WriteLine("null");
                }
            }
            else
            {
                Console.WriteLine("Parameters is null");
            }

            Console.WriteLine("Invoke:");

            method.Invoke(b, (new object[] { (2.718281) }));
        }


        static void Test3()
        {
            Console.WriteLine("----Test3");
            ClassD d = new ClassD();
            ClassD d2 = new ClassD("Not just a D");
            var b = new MDer(d);
            var b2 = new MDer();
            
            
            var ctor = b.GetType().GetConstructor(new Type[] {typeof(ClassD)});

            if (ctor == null)
            {
                Console.WriteLine("Method is null");
                return;
            }

        TODO:
            var parameters = ctor.GetParameters();
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    if (param != null)
                        Console.WriteLine(param);
                    else
                        Console.WriteLine("null");
                }
            }
            else
            {
                Console.WriteLine("Parameters is null");
            }

            Console.WriteLine("Invoke:");

            ctor.Invoke(new object[] { (new ClassD("ClassD: hello, this a D test")) });
            //ctor.Invoke(new object[] { (new ClassD("ClassD: hello, this a D test")) });
        }

        static void Main()
        {
            Test1();
            Test2();
            Test3();
            Console.WriteLine("<%END%>");
        }
    }
}
