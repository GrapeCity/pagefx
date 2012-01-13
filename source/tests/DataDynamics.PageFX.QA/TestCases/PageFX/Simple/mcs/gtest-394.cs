using System;

namespace gtest394
{
    public class Test
    {
        public delegate bool MemberFilter();

        public static void FindMembers(MemberFilter filter) 
        {
            Console.WriteLine(filter != null);
            if (filter != null)
                Console.WriteLine(filter());
        }

        public static void GetMethodGroup(MemberFilter filter)
        {
            FindMembers(filter ?? delegate()
            {
                return true;
            });
        }

        public static void Main()
        {
            GetMethodGroup(null);
            GetMethodGroup(delegate() { return true; });
            GetMethodGroup(delegate() { return false; });
            Console.WriteLine("<%END%>");
        }
    }
}
