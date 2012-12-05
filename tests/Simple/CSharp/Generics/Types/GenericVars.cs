using System;

namespace X
{
    class A<T>
    {
        private T _privateVar;
        public T PublicVar;

        public A()
        {
            _privateVar = default(T);
            PublicVar = default(T);
        }

        public T GetPrivateVar()
        {
            return _privateVar;
        }

        public void Test1(T value)
        {
            T _privateVar = value;
            T PublicVar = value;
        }

        public void Test2(T value)
        {
            _privateVar = value;
            PublicVar = value;
        }

        public override string ToString()
        {
            return string.Format("A<{0}, {1}>", _privateVar, PublicVar);
        }
    }

    class B<T>
    {
        private static T _privateVar;
        public static T PublicVar;

        public static T GetPrivateVar()
        {
            return _privateVar;
        }

        public B()
        {
            _privateVar = default(T);
            PublicVar = default(T);
        }


        public B(T value)
        {
            _privateVar = value;
            PublicVar = value;
        }

        public void Test1(T value)
        {
            T _privateVar = value;
            T PublicVar = value;
        }

        public void Test2(T value)
        {
            _privateVar = value;
            PublicVar = value;
        }

        public override string ToString()
        {
            return string.Format("B<{0}, {1}>", _privateVar, PublicVar);
        }
    }

    class C<T> where T : class
    {
        private T _privateVar;
        public T PublicVar;

        public T GetPrivateVar()
        {
            return _privateVar;
        }

        public C()
        {
            _privateVar = default(T);
            PublicVar = default(T);
        }


        public C(T value)
        {
            _privateVar = value;
            PublicVar = value;
        }

        public void Test1(T value)
        {
            T _privateVar = value;
            T PublicVar = value;
        }

        public override string ToString()
        {
            return string.Format("C<{0}, {1}>", _privateVar, PublicVar);
        }
    }


    class Test
    {
        static void TestNonStaticPrimitive()
        {
            Console.WriteLine("----TestNonStaticPrimitive");
            A<int> a = new A<int>();
            a.Test1(5);
            Console.WriteLine("private = {0}, public = {1}", a.GetPrivateVar(), a.PublicVar);
            a.Test2(7);
            Console.WriteLine("private = {0}, public = {1}", a.GetPrivateVar(), a.PublicVar);
        }

        static void TestNonStatic()
        {
            Console.WriteLine("----TestNonStatic");
            C<B<int>> cb = new C<B<int>>(new B<int>(7));
            Console.WriteLine("private = {0}", cb.GetPrivateVar());
            cb.Test1(new B<int>(5));
            Console.WriteLine("private = {0}", cb.GetPrivateVar());

            C<A<int>> ca = new C<A<int>>(new A<int>());
            Console.WriteLine("private = {0}", ca.GetPrivateVar());
            ca.Test1(new A<int>());
            Console.WriteLine("private = {0}", ca.GetPrivateVar());

            C<C<A<int>>> cca = new C<C<A<int>>>(new C<A<int>>(new A<int>()));
            Console.WriteLine("private = {0}", cca.GetPrivateVar());
        }

        static void TestStatic()
        {
            Console.WriteLine("TestStatic");   
            B<int> b = new B<int>();
            b.Test1(5);
            Console.WriteLine("private = {0}, public = {1}", B<int>.GetPrivateVar(), B<int>.PublicVar);
            b.Test2(7);
            Console.WriteLine("private = {0}, public = {1}", B<int>.GetPrivateVar(), B<int>.PublicVar);
        }


        static void Main()
        {
            TestNonStaticPrimitive();
            TestNonStatic();
            TestStatic();
            Console.WriteLine("<%END%>");
        }
    }
}
