using System;

namespace PageFX
{
    class Num : Avm.Object
    {
        public int value;

        public Num(int n)
        {
            value = n;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }

    class X
    {
        static void Main()
        {
            Num num = new Num(10);
            Console.WriteLine(num.ToString());
            Type type = num.GetType();
            Console.WriteLine(type.FullName);
            Console.WriteLine("<%END%>");
        }
    }
}