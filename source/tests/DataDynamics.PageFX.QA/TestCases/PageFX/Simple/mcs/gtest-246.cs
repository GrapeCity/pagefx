using System;

namespace TestCase
{
    interface ITest
    {
    }

    class CTest : ITest
    {
        static void Test1()
        {
            Console.WriteLine("----Test1");
            CGenericTest<ITest, CTest> c = new CGenericTest<ITest, CTest>();
            c.Foo().Bar();
        }

        static void Main()
        {
            Test1();
            Console.WriteLine("<%END%>");
        }

        public void Bar()
        {
            Console.WriteLine("Bar");
        }
    }

    class CGenericTest<T, V>
        where T : ITest
        where V : CTest, T, new()
    {
        public V Foo()
        {
            V TestObject = new V();
            TestObject.Bar();
            return TestObject;
        }
    }
}
