// Compiler options: -langversion:default
using System;

namespace partial01
{
    namespace Foo
    {
        public class Hello
        {
            public static int World = 8;
        }
    }

    namespace Bar
    {
        public class Hello
        {
            public static int World = 9;
        }
    }

    namespace X
    {
        using Foo;

        public partial class Test
        {
            public static int FooWorld()
            {
                return Hello.World;
            }
        }
    }

    namespace X
    {
        using Bar;

        public partial class Test
        {
            public static int BarWorld()
            {
                return Hello.World;
            }
        }
    }

    class Y
    {
        
        static void Main()
        {
            Console.WriteLine(Test1());
            Console.WriteLine("<%END%>");
        }

        private static int Test1()
        {
            Console.WriteLine("----Test1");
            if (X.Test.FooWorld() != 8)
                return 1;
            if (X.Test.BarWorld() != 9)
                return 2;
            return 0;
        }
    }
}
