using System.Collections.Generic;
using System.Linq;

namespace DataDynamics
{
    #region interface IFilter
    public interface IFilter<T>
    {
        /// <summary>
        /// Determine if a particular object passes the filter criteria.
        /// </summary>
        /// <param name="obj">The object to which the filter is applied</param>
        /// <returns></returns>
        bool Pass(T obj);
    }
    #endregion

    #region class TrueFilter
    public class TrueFilter<T> : IFilter<T>
    {
        private TrueFilter()
        {
        }

        public static readonly IFilter<T> Instance = new TrueFilter<T>();

        public bool Pass(T obj)
        {
            return true;
        }
    }
    #endregion

    #region class FalseFilter
    public class FalseFilter<T> : IFilter<T>
    {
        private FalseFilter()
        {
        }

        public static readonly IFilter<T> Instance = new FalseFilter<T>();

        public bool Pass(T obj)
        {
            return false;
        }
    }
    #endregion

    #region class AndFilter
    public class AndFilter<T> : IFilter<T>
    {
        readonly List<IFilter<T>> _filters;

        public AndFilter()
        {
            _filters = new List<IFilter<T>>();
        }

        public AndFilter(IFilter<T> filter)
        {
            _filters = new List<IFilter<T>> {filter};
        }

        public AndFilter(IEnumerable<IFilter<T>> filters)
        {
            _filters = new List<IFilter<T>>(filters);
        }

        public void Add(IFilter<T> filter)
        {
            _filters.Add(filter);
        }

        public void Add(IEnumerable<IFilter<T>> filters)
        {
            _filters.AddRange(filters);
        }

        public bool Pass(T obj)
        {
        	return _filters.All(filter => filter.Pass(obj));
        }
    }
    #endregion

    #region class OrFilter
    public class OrFilter<T> : IFilter<T>
    {
        readonly List<IFilter<T>> _filters;

        public OrFilter()
        {
            _filters = new List<IFilter<T>>();
        }

        public OrFilter(IFilter<T> filter)
        {
            _filters = new List<IFilter<T>> {filter};
        }

        public OrFilter(IEnumerable<IFilter<T>> filters)
        {
            _filters = new List<IFilter<T>>(filters);
        }

        public void Add(IFilter<T> filter)
        {
            _filters.Add(filter);
        }

        public void Add(IEnumerable<IFilter<T>> filters)
        {
            _filters.AddRange(filters);
        }

        public bool Pass(T obj)
        {
        	return _filters.Any(filter => filter.Pass(obj));
        }
    }
    #endregion

    #region class NotFilter
    public class NotFilter<T> : IFilter<T>
    {
        readonly IFilter<T> _filter;

        public NotFilter(IFilter<T> filter)
        {
            _filter = filter;
        }

        public bool Pass(T obj)
        {
            return !_filter.Pass(obj);
        }
    }
    #endregion

    #region class EqualsFilter
    public class EqualsFilter<T> : IFilter<T>
    {
        readonly T _value;

        public EqualsFilter(T value)
        {
            _value = value;
        }

        public bool Pass(T obj)
        {
            return Equals(obj, _value);
        }
    }
    #endregion
}