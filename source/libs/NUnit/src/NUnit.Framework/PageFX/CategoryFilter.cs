using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.NUnit
{
    internal class CategoryFilter
    {
        private static readonly List<IFilter<string>> ExcludeFilters = new List<IFilter<string>>();
        private static readonly List<IFilter<string>> IncludeFilters = new List<IFilter<string>>();

        public static void Clear()
        {
            IncludeFilters.Clear();
            ExcludeFilters.Clear();
        }

        static IFilter<string> Parse(string exp)
        {
            return new CategoryExpression(exp).Filter;
        }

        public static void Exclude(string exp)
        {
            if (string.IsNullOrEmpty(exp)) return;
            ExcludeFilters.Add(Parse(exp));
        }

        public static void Exclude(params string[] exps)
        {
            if (exps == null) return;
            foreach (var exp in exps)
                Exclude(exp);
        }

        public static void Include(string exp)
        {
            if (string.IsNullOrEmpty(exp)) return;
            IncludeFilters.Add(Parse(exp));
        }

        public static void Include(params string[] exps)
        {
            if (exps == null) return;
            foreach (var exp in exps)
                Include(exp);
        }

        public static bool IsIncluded(ICollection<string> cats)
        {
            if (IncludeFilters.Count > 0)
            {
                if (cats == null || cats.Count == 0)
                    return false;
            	return (from filter in IncludeFilters
						from cat in cats
						where filter.Pass(cat)
						select filter).Any();
            }
            if (ExcludeFilters.Count > 0)
            {
                if (cats != null && cats.Count > 0)
                {
                	return !(from filter in ExcludeFilters
							 from cat in cats
							 where filter.Pass(cat)
							 select filter).Any();
                }
            }
            return true;
        }
    }
}