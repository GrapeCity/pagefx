namespace DataDynamics
{
    public struct Pair<F, S>
    {
        public F First;
        public S Second;

        public Pair(F f, S s)
        {
            First = f;
            Second = s;
        }
    }
}