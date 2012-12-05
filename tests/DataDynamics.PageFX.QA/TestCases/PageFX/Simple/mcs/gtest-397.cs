using System;

namespace gtest397
{
    struct Foo
    {
        public int Value;

        public Foo(int value)
        {
            this.Value = value;
        }

        public static Foo operator -(Foo? f)
        {
            if (f.HasValue)
                return new Foo(-f.Value.Value);

            return new Foo(42);
        }
    }

    struct Bar
    {
        public int Value;

        public Bar(int value)
        {
            this.Value = value;
        }

        public static Bar? operator -(Bar? b)
        {
            if (b.HasValue)
                return new Bar(-b.Value.Value);

            return b;
        }
    }

    class Test
    {

        static Foo NegateFoo(Foo f)
        {
            return -f;
        }

        static Foo NegateFooNullable(Foo? f)
        {
            return -f;
        }

        static Bar? NegateBarNullable(Bar? b)
        {
            return -b;
        }

        static Bar? NegateBar(Bar b)
        {
            return -b;
        }

        static void Main()
        {
            Console.WriteLine(NegateFooNullable(null).Value != 42);
            Console.WriteLine(NegateFoo(new Foo(2)).Value != -2);
            Console.WriteLine(NegateBarNullable(null) != null);
            Console.WriteLine(NegateBar(new Bar(2)).Value.Value != -2);
            Console.WriteLine("<%END%>");
        }
    }
}

