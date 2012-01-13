using System;

namespace gtest395
{
    public class RuleBuilder<T> where T : class 
    {
        public void Foo<U>(T t)
        {
            Console.WriteLine(typeof(U) == typeof(T));
        }
    }

    public interface IDynamicObject
    {
        RuleBuilder<T> GetRule<T>() where T : class;
    }

    public class RubyMethod : IDynamicObject
    {
        RuleBuilder<T> IDynamicObject.GetRule<T>() /* where T : class */ {
            return new RuleBuilder<T>();
        }
    }

    public class T
    {
        static void Test1()
        {
            Console.WriteLine("--- Test1");
            IDynamicObject obj = new RubyMethod();
            RuleBuilder<T> rb = obj.GetRule<T>();
            rb.Foo<T>(new T());
            rb.Foo<int>(new T());
        }

        static void Main()
        {
            Test1();
            Console.WriteLine("<%END%>");
        }
    }
}
