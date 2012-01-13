using System;
using System.Collections.Generic;

namespace DataDynamics
{
    public sealed class FunctorComparer<T> : IComparer<T>
    {
        private readonly Comparison<T> comparison;

        public FunctorComparer(Comparison<T> comparison)
        {
            this.comparison = comparison;
        }

        #region IComparer<T> Members
        public int Compare(T x, T y)
        {
            return comparison(x, y);
        }
        #endregion
    }
}