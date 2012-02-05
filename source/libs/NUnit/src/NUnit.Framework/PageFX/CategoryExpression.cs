using System;

namespace DataDynamics.PageFX.NUnit
{
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
    class CategoryExpression
    {
        static readonly char[] ops = new [] { ',', ';', '-', '|', '+', '(', ')' };

        readonly string _text;
        int _next;
        string _token;

        IFilter<string> _filter;

        public CategoryExpression(string text)
        {
            _text = text;
            _next = 0;
        }

        public IFilter<string> Filter
        {
            get
            {
            	return _filter ?? (_filter = GetToken() == null
            	                             	? TrueFilter<string>.Instance
            	                             	: GetExpression());
            }
        }

        IFilter<string> GetExpression()
        {
            var term = GetTerm();
            if (_token != "|")
                return term;

            var filter = new OrFilter<string>(term);
            
            while (_token == "|")
            {
                GetToken();
                filter.Add(GetTerm());
            }

            return filter;
        }

        IFilter<string> GetTerm()
        {
            IFilter<string> prim = GetPrimitive();
            if (_token != "+" && _token != "-")
                return prim;

            var filter = new AndFilter<string>(prim);
            
            while (_token == "+" || _token == "-")
            {
                string tok = _token;
                GetToken();
                prim = GetPrimitive();
                filter.Add(tok == "-" ? new NotFilter<string>(prim) : prim);
            }

            return filter;
        }

        IFilter<string> GetPrimitive()
        {
            if (_token == "-")
            {
                GetToken();
                return new NotFilter<string>(GetPrimitive());
            }
            if (_token == "(")
            {
                GetToken();
                IFilter<string> expr = GetExpression();
                GetToken(); // Skip ')'
                return expr;
            }

            return GetCategoryFilter();
        }

        IFilter<string> GetCategoryFilter()
        {
            var filter = new OrFilter<string>();
            filter.Add(new EqualsFilter<string>(_token));

            while (GetToken() == "," || _token == ";")
                filter.Add(new EqualsFilter<string>(GetToken()));

            return filter;
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
                int index2 = _text.IndexOfAny(ops, _next);
                if (index2 < 0) index2 = _text.Length;

                _token = _text.Substring(_next, index2 - _next).TrimEnd();
                _next = index2;
            }

            return _token;
        }

        void SkipWhiteSpace()
        {
            while (_next < _text.Length && Char.IsWhiteSpace(_text[_next]))
                ++_next;
        }

        bool EndOfText()
        {
            return _next >= _text.Length;
        }

        bool NextIsOperator()
        {
            foreach (char op in ops)
                if (op == _text[_next])
                    return true;

            return false;
        }
    }
}