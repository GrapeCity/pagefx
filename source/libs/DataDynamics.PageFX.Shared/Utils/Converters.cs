using System;

namespace DataDynamics
{
    public interface IConverter<TInput, TOutput>
    {
        TOutput Convert(TInput input);
    }

    public class IdentityConverter<T> : IConverter<T, T>
    {
        public static readonly IdentityConverter<T> Instance = new IdentityConverter<T>();

        public T Convert(T input)
        {
            return input;
        }
    }

    public class DelegateConverter<TInput, TOutput> : IConverter<TInput, TOutput>
    {
        readonly Converter<TInput, TOutput> _converter;

        public DelegateConverter(Converter<TInput, TOutput> converter)
        {
            if (converter == null)
                throw new ArgumentNullException("converter");
            _converter = converter;
        }

        public TOutput Convert(TInput input)
        {
            return _converter(input);
        }
    }
}