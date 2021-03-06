﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.NUnit
{
	using Filter = Func<string, bool>;

    internal sealed class CategoryFilter
    {
		private static readonly List<Filter> ExcludeFilters = new List<Filter>();
		private static readonly List<Filter> IncludeFilters = new List<Filter>();

        public static void Clear()
        {
            IncludeFilters.Clear();
            ExcludeFilters.Clear();
        }

		private static Filter Parse(string exp)
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

        public static bool IsIncluded(ICollection<string> categories)
        {
            if (IncludeFilters.Count > 0)
            {
                if (categories == null || categories.Count == 0)
                    return false;
            	return (from f in IncludeFilters
						from category in categories
						where f(category)
						select f).Any();
            }
            if (ExcludeFilters.Count > 0)
            {
                if (categories != null && categories.Count > 0)
                {
                	return !(from f in ExcludeFilters
							 from category in categories
							 where f(category)
							 select f).Any();
                }
            }
            return true;
        }
    }
}