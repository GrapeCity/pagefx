namespace DataDynamics
{
    public class Indent
    {
        public Indent(string one)
        {
            _one = one;
        }

        public Indent()
            : this("\t")
        {
        }

        private readonly string _one;

        public string Value
        {
            get { return _value; }
        }
        private string _value = "";

        public static implicit operator string(Indent i)
        {
            return i._value;
        }

        public static Indent operator ++(Indent i)
        {
            i._value += i._one;
            return i;
        }

        public static Indent operator --(Indent i)
        {
            int n = i._value.Length - i._one.Length;
            i._value = n > 0 ? i._value.Substring(0, n) : "";
            return i;
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            var i = obj as Indent;
            if (i != null)
            {
                return i._value == _value;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value;
        }
    }
}