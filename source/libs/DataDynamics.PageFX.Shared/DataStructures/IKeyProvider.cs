namespace System.Collections.Generic
{
    public interface IKeyProvider<TKey, TValue>
    {
        TKey KeyOf(TValue value);
    }

    public delegate TKey KeyFunction<TKey, TValue>(TValue value);

    public sealed class FunctorKeyProvider<TKey, TValue> : IKeyProvider<TKey, TValue>
    {
        private readonly KeyFunction<TKey, TValue> _f;

        public FunctorKeyProvider(KeyFunction<TKey, TValue> f)
        {
            _f = f;
        }

        #region IKeyProvider<TKey,TValue> Members
        public TKey KeyOf(TValue value)
        {
            return _f(value);
        }
        #endregion
    }
}