using System;

namespace DataDynamics
{
    public class LazyValue<T> where T : class
    {
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
                if (_type == null)
                {
                    _type = _eval();
                    if (_type == null)
                        throw new InvalidOperationException("Evaluated value cannot be null");
                }
                return _type;
            }
        }
        private T _type;
        private readonly Func<T> _eval;
    }
}