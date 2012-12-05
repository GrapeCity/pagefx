using System;

namespace gtest392
{
    public class DieSubrangeType
    {
        public int? UpperBound
        {
            get { return _ub; }
            private set { _ub = value; }
        }
        private int? _ub;

        public DieSubrangeType()
        {
            UpperBound = 1;
        }
    }

    class X
    {
        static void Main()
        {
            DieSubrangeType subrange = new DieSubrangeType();
            Console.WriteLine(subrange.UpperBound != null);
            Console.WriteLine((int)subrange.UpperBound);
            Console.WriteLine((int)subrange.UpperBound - 1);
            Console.WriteLine("<%END%>");
        }
    }
}
