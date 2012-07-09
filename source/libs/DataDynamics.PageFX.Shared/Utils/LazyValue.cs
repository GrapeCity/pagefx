using System;

namespace DataDynamics
{
    public class LazyValue<T> where T : class
    {
		private T _value;
		private readonly Func<T> _eval;

        public LazyValue(Func<T> eval)
        {
            if (eval == null)
                throw new ArgumentNullException("eval");
            _eval = eval;
        }

        public T Value
        {
            get
            {
                if (_value == null)
                {
                    _value = _eval();
                    if (_value == null)
                        throw new InvalidOperationException("Evaluated value cannot be null");
                }
                return _value;
            }
        }
    }
}