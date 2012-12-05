using System;

namespace gtest407
{
    struct MyColor
    {
        int v;

        public MyColor(int v)
        {
            this.v = v;
        }

        public static bool operator ==(MyColor left, MyColor right)
        {
            return left.v == right.v;
        }

        public static bool operator !=(MyColor left, MyColor right)
        {
            return left.v != right.v;
        }
    }

    public class NullableColorTests
    {
        public static void Main()
        {
            MyColor? col = null;
            bool b = col == new MyColor(3);
            Console.WriteLine(b);
            

            b = col != new MyColor(3);
            Console.WriteLine(!b);
            Console.WriteLine("<%END%>");
        }
    }
}