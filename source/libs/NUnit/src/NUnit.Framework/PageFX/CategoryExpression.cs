using System;
using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.NUnit
{
	using Filter = Func<string, bool>;

    //see http://www.nunit.org/index.php?p=consoleCommandLine&r=2.4.8
	
    /// <summary>
    /// CategoryExpression parses strings representing boolean
    /// combinations of categories according to the following
    /// grammar:
    ///   CategoryName ::= string not containing any of ',', '&', '+', '-'
    ///   CategoryFilter ::= CategoryName | CategoryFilter ',' CategoryName
    ///   CategoryPrimitive ::= CategoryFilter | '-' CategoryPrimitive
    ///   CategoryTerm ::= CategoryPrimitive | CategoryTerm '&' CategoryPrimitive
    /// </summary>
    internal sealed class CategoryExpression
    {
        private static readonly char[] Operators = new [] { ',', ';', '-', '|', '+', '(', ')' };

        readonly string _text;
        int _next;
        string _token;

        private Filter _filter;

        public CategoryExpression(string text)
        {
            _text = text;
            _next = 0;
        }

        public Filter Filter
        {
            get
            {
            	return _filter ?? (_filter = GetToken() == null
            	                             	? True()
            	                             	: GetExpression());
            }
        }

		private static Filter True()
		{
			return s => true;
		}

        private Filter GetExpression()
        {
	        var terms = GetTerms().ToArray();
	        return s => terms.Any(f => f(s));
        }

		private IEnumerable<Filter> GetTerms()
		{
			var term = GetTerm();
			if (_token != "|")
			{
				yield return term;
				yield break;
			}

			while (_token == "|")
			{
				GetToken();
				yield return GetTerm();
			}
		}

		private Filter GetTerm()
		{
			var prims = GetPrims().ToArray();
			return s => prims.All(f => f(s));
        }

		private IEnumerable<Filter> GetPrims()
		{
			var prim = GetPrimitive();
			if (_token != "+" && _token != "-")
			{
				yield return prim;
				yield break;
			}

			while (_token == "+" || _token == "-")
			{
				string tok = _token;
				GetToken();
				prim = GetPrimitive();
				yield return tok == "-" ? Not(prim) : prim;
			}
		}

		private Filter GetPrimitive()
        {
            if (_token == "-")
            {
                GetToken();
                return Not(GetPrimitive());
            }
            if (_token == "(")
            {
                GetToken();
                var expr = GetExpression();
                GetToken(); // Skip ')'
                return expr;
            }

            return GetCategoryFilter();
        }

		private static Filter Not(Filter filter)
		{
			return s => !filter(s);
		}

		private Filter GetCategoryFilter()
		{
			var names = GetCategoryNames().ToArray();
			return s => names.Contains(s);
		}

		private IEnumerable<string> GetCategoryNames()
		{
			yield return _token;

			while (GetToken() == "," || _token == ";")
				yield return GetToken();
		}

        public string GetToken()
        {
            SkipWhiteSpace();

            if (EndOfText())
                _token = null;
            else if (NextIsOperator())
                _token = _text.Substring(_next++, 1);
            else
            {
                int index2 = _text.IndexOfAny(Operators, _next);
                if (index2 < 0) index2 = _text.Length;

                _token = _text.Substring(_next, index2 - _next).TrimEnd();
                _next = index2;
            }

            return _token;
        }

        private void SkipWhiteSpace()
        {
            while (_next < _text.Length && Char.IsWhiteSpace(_text[_next]))
                ++_next;
        }

		private bool EndOfText()
        {
            return _next >= _text.Length;
        }

		private bool NextIsOperator()
        {
        	return Operators.Any(op => op == _text[_next]);
        }
    }
}