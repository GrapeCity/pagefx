namespace DataDynamics
{
    public static class Logic
    {
        public static bool Implies(bool a, bool b)
        {
            return a ? b : true;
        }

        public static bool Bijection(bool a, bool b)
        {
            return Implies(a, b) && Implies(b, a);
        }
    }
}