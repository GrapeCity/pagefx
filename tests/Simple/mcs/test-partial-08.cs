// The standard says this doesn't have to have the 'sealed' modifier
using System;

namespace partial08
{
    public partial class Foo
    {
        public string myId;
    }

    public sealed partial class Foo
    {
        public string Id { get { return myId; } }
    }

    public class PartialAbstractCompilationError
    {
        public static void Main()
        {
            Console.WriteLine(Test1());
            Console.WriteLine("<%END%>");
        }

        private static int Test1()
        {
            Console.WriteLine("----Test1");
            if (typeof(Foo).IsAbstract || !typeof(Foo).IsSealed)
                return 1;

            System.Console.WriteLine("OK");
            return 0;
        }
    }
}
