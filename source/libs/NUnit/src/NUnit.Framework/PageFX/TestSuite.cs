using System.Collections.Generic;
using System.Xml;

namespace DataDynamics.PageFX.NUnit
{
    class TestSuite : IStat
    {
        public TestSuite()
        {
            Success = true;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Success { get; set; }

        public double Time { get; set; }

        public int Asserts { get; set; }

        public List<TestSuite> Suites
        {
            get { return _suites; }
        }
        readonly List<TestSuite> _suites = new List<TestSuite>();

        public List<Test> Tests
        {
            get { return _tests; }
        }
        readonly List<Test> _tests = new List<Test>();

        public void Update(IStat stat)
        {
            if (!stat.Success)
                Success = false;
            Time += stat.Time;
            Asserts += stat.Asserts;
        }

        public void Write(XmlWriter writer)
        {
            writer.WriteStartElement("test-suite");
            writer.WriteAttributeString("name", Name);
            if (!string.IsNullOrEmpty(Description))
                writer.WriteAttributeString("description", Description);
            Report.WriteAttrs(writer, this);
            writer.WriteStartElement("results");
            foreach (var test in _tests)
                test.Write(writer);
            foreach (var suite in _suites)
                suite.Write(writer);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}