using System.Collections.Generic;

namespace DataDynamics.PageFX
{
    public class TestSuite
    {
        private TestSuite(string name, string fullname)
        {
            _name = name;
            _fullname = fullname;
        }

        public string Name
        {
            get { return _name; }
        }
        private readonly string _name;

        public string FullName
        {
            get { return _fullname; }
        }
        private readonly string _fullname;

        public int TotalFailed;
        public int TotalPassed;
        public int TotalCount;

        public bool IsRoot
        {
            get { return _parent == null; }
        }

        public TestSuite Parent
        {
            get { return _parent; }
        }
        private TestSuite _parent;

        public TestCaseCollection Cases
        {
            get { return _cases; }
        }
        private readonly TestCaseCollection _cases = new TestCaseCollection();

        public void Sort()
        {
            _cases.Sort((x, y) => string.Compare(x.Name, y.Name, true));
            _childSuites.Sort((x, y) => string.Compare(x.Name, y.Name, true));
        }

        public IEnumerable<TestSuite> ChildSuites
        {
            get { return _childSuites; }
        }
        private readonly HashList<string, TestSuite> _childSuites = new HashList<string, TestSuite>(t => t.Name);

        public void Add(TestSuite ts)
        {
            ts._parent = this;
            _childSuites.Add(ts);
        }

        public TestSuite FindTestSuite(string name)
        {
            return _childSuites[name];
        }

        public void Reset()
        {
            TotalFailed = 0;
            TotalPassed = 0;
        }

        public int Total
        {
            get { return TotalFailed + TotalPassed; }
        }

        public double Percentage
        {
            get { return TotalPassed / (double)Total; }
        }

        public override string ToString()
        {
            return _name;
        }

        public static TestSuite Root
        {
            get { return _root ?? (_root = new TestSuite("", "")); }
        }
        private static TestSuite _root;

        public static void Register(TestCase tc)
        {
            var names = tc.FullName.Split('.');

            int n = names.Length - 1;
            if (tc.IsSimple && !tc.IsNUnit)
                n -= 1;

            var parent = Root;
            string fullname = "";
            for (int i = 0; i < n; ++i)
            {
                ++parent.TotalCount;
                string name = names[i];
                if (fullname.Length > 0)
                    fullname += ".";
                fullname += name;
                var ts = parent.FindTestSuite(name);
                if (ts == null)
                {
                    ts = new TestSuite(name, fullname);
                    parent.Add(ts);
                }
                parent = ts;
            }

            ++parent.TotalCount;
            tc.Suite = parent;
            parent.Cases.Add(tc);
        }

        public static void Register(IEnumerable<TestCase> cases)
        {
            if (cases == null) return;
            foreach (var tc in cases)
                Register(tc);
        }
    }
}