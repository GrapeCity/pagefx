

using System;
using System.Reflection;

namespace PropertyInfoTest
{
    class B 
    { 
        public override string ToString()
        {
            return "B+";
        }
    }
    class A
    {
        public int MyInt { get; set; }
        public double MyFloat { get; set; }
        public B b { get; set; } 
        public void Foo(int a, double d, char c)
        {
            Console.WriteLine("A::Foo(int, float, char, B)");
        }
    }

    class Test
    {
        static void Test1()
        {
            Console.WriteLine("----Test1");

            var a = new A();
            a.Foo(1, 2.0, 'a');
            a.b = new B();
            a.MyInt = 5;
            a.MyFloat = 2.0;

            var props = typeof(A).GetProperties();
            foreach (var prop in props)
            {
                Console.WriteLine(prop);
            }
        }


        static void Test2()
        {
            Console.WriteLine("----Test2");

            var a = new A();
            a.Foo(1, 2.0, 'a');

            var props = typeof(A).GetProperties();
            foreach (var prop in props)
            {
                Console.WriteLine(prop);
            }
        }

        static void Test3()
        {
            Console.WriteLine("----Test3(getter test)");

            var a = new A();
            a.Foo(1, 2.0, 'a');
            a.Foo(1, 2.0, 'a');
            a.b = new B();
            a.MyInt = 5;
            a.MyFloat = 2.0;

            int qa = a.MyInt;
            double qb = a.MyFloat;
            var qc = a.b;
            Console.WriteLine("qa:{0}, qb:{1}", qa, qb);

            GetAndViewProps(a);
        }

        private static void GetAndViewProps(A a)
        {
            var props = typeof(A).GetProperties();
            foreach (var prop in props)
            {
                try
                {
                    var getter = prop.GetGetMethod();
                    var val = getter.Invoke(a, null);
                    Console.WriteLine("value {0}", val);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc);
                }
                Console.WriteLine(prop);
            }
        }


        static void Test4()
        {
            Console.WriteLine("----Test4(setter test)");

            var a = new A();
            a.Foo(1, 2.0, 'a');
            a.Foo(1, 2.0, 'a');
            a.b = new B();
            a.MyInt = 5;
            a.MyFloat = 2.0;

            int qa = a.MyInt;
            double qb = a.MyFloat;
            var qc = a.b;
            Console.WriteLine("qa:{0}, qb:{1}", qa, qb);


            SetAndViewProp(a, "MyInt", 5);
            SetAndViewProp(a, "MyFloat", 2.71828);
            SetAndViewProp(a, "b", new B());
            GetAndViewProps(a);
        }



        private static void SetAndViewProp(A a, string propName, object propVal)
        {
            var prop = typeof(A).GetProperty(propName);

            try
            {
                var setter = prop.GetSetMethod();
                setter.Invoke(a, new [] {propVal});
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
            Test3();
            Test4();
            Console.WriteLine("<%END%>");
        }
    }
}