using System;

namespace gtest389
{

    enum MyEnum : byte
    {
        A = 1,
        B = 2,
        Z = 255
    }

    class C
    {
        public static void Main()
        {
            try
            {
                MyEnum? e = MyEnum.A;
                byte? b = 255;
                MyEnum? res = e + b;
                Console.WriteLine(res != 0);

                e = null;
                b = 255;
                res = e + b;
                Console.WriteLine(res != null);

                MyEnum e2 = MyEnum.A;
                byte b2 = 1;
                MyEnum res2 = e2 + b2;
                Console.WriteLine(res2 != MyEnum.B);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Unexpected exception: " + exc);
            }
            Console.WriteLine("<%END%>");
        }
    }


}
