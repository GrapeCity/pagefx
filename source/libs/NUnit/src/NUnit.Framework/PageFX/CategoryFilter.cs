using System.Collections.Generic;

namespace DataDynamics.PageFX.NUnit
{
    class CategoryFilter
    {
        static readonly List<IFilter<string>> _exclude = new List<IFilter<string>>();
        static readonly List<IFilter<string>> _include = new List<IFilter<string>>();

        public static void Clear()
        {
            _include.Clear();
            _exclude.Clear();
        }

        static IFilter<string> Parse(string exp)
        {
            return new CategoryExpression(exp).Filter;
        }

        public static void Exclude(string exp)
        {
            if (string.IsNullOrEmpty(exp)) return;
            _exclude.Add(Parse(exp));
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
            _include.Add(Parse(exp));
        }

        public static void Include(params string[] exps)
        {
            if (exps == null) return;
            foreach (var exp in exps)
                Include(exp);
        }

        public static bool IsIncluded(ICollection<string> cats)
        {
            if (_include.Count > 0)
            {
                if (cats == null || cats.Count == 0)
                    return false;
                foreach (var f in _include)
                {
                    foreach (var cat in cats)
                        if (f.Pass(cat))
                            return true;
                }
                return false;
            }
            if (_exclude.Count > 0)
            {
                if (cats != null && cats.Count > 0)
                {
                    foreach (var f in _exclude)
                    {
                        foreach (var cat in cats)
                            if (f.Pass(cat))
                                return false;
                    }
                }
            }
            return true;
        }
    }
}