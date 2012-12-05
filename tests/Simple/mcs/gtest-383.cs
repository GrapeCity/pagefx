using System;

namespace gtest383
{

    struct MyTypeA
    {
        short b;

        public MyTypeA(short b)
        {
            this.b = b;
        }

        public static MyTypeA operator +(MyTypeA a, MyTypeA b)
        {
            throw new NotSupportedException();
        }

        public static bool operator ==(MyTypeA a, MyTypeA b)
        {
            return true;
        }

        public static bool operator !=(MyTypeA a, MyTypeA b)
        {
            throw new NotSupportedException();
        }

        public static bool operator >(MyTypeA a, MyTypeA b)
        {
            throw new NotSupportedException();
        }

        public static bool operator <(MyTypeA a, MyTypeA b)
        {
            throw new NotSupportedException();
        }
    }

    struct MyTypeB
    {
        short b;

        public MyTypeB(short b)
        {
            this.b = b;
        }

        public static MyTypeB operator +(MyTypeB a, MyTypeB b)
        {
            return a;
        }

        public static bool operator ==(MyTypeB a, MyTypeB b)
        {
            return true;
        }

        public static bool operator !=(MyTypeB a, MyTypeB b)
        {
            return false;
        }

        public static bool operator >(MyTypeB a, MyTypeB b)
        {
            return false;
        }

        public static bool operator <(MyTypeB a, MyTypeB b)
        {
            return true;
        }

        public static MyTypeB operator &(MyTypeB a, MyTypeB b)
        {
            return a;
        }
    }

    class C
    {
        static void Main()
        {
            MyTypeA? mt = null;
            mt = null + mt;

            MyTypeA? mt2 = null;
            mt2 = mt2 + null;
            bool b = mt2 > null;
            bool x = mt2 == null;

            MyTypeB? bt = null;
            bt = bt + bt;
            Console.WriteLine(bt != null);
            
            MyTypeB? b2 = null;
            bool bb = b2 == b2;
            Console.WriteLine(!bb);
            
            MyTypeB? b3 = null;
            b3 = b3 & b3;
            Console.WriteLine("<%END%>");
        }
    }
}

