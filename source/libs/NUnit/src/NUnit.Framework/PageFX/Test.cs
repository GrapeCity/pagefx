using System;
using System.Collections.Generic;
using System.Xml;

namespace DataDynamics.PageFX.NUnit
{
    public interface IStat
    {
        bool Success { get; set; }

        double Time { get; set; }

        int Asserts { get; set; }
    }

    class Test : IStat
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string AssemblyPath { get; set; }

        public string SuiteName { get; set; }

        public string SuiteDescription { get; set; }

        public bool Executed { get; set; }

        public bool Success { get; set; }

        public double Time { get; set; }

        public int Asserts { get; set; }

        public int ExitCode;

        public string Output;

        public string StackTrace;

#if !AVM
        public string MainCode;
#endif

        public TestSuite Suite { get; set; }

        public readonly List<string> Categories = new List<string>();

        #region Write
        public void Write(XmlWriter writer)
        {
            writer.WriteStartElement("test-case");
            writer.WriteAttributeString("name", Name);
            if (!string.IsNullOrEmpty(Description))
                writer.WriteAttributeString("description", Description);
            writer.WriteAttributeString("executed", XmlConvert.ToString(Executed));
            Report.WriteAttrs(writer, this);

            if (Categories.Count > 0)
            {
                writer.WriteStartElement("categories");
                foreach (var name in Categories)
                {
                    writer.WriteStartElement("category");
                    writer.WriteAttributeString("name", name);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            if (!string.IsNullOrEmpty(Output))
            {
                if (Success)
                {
                    //writer.WriteStartElement("output");
                    //writer.WriteCData(Output);
                    //writer.WriteEndElement();
                }
                else
                {
                    writer.WriteStartElement("failure");
                    writer.WriteStartElement("message");
                    writer.WriteCData(Output);
                    writer.WriteEndElement();
                    if (!string.IsNullOrEmpty(StackTrace))
                    {
                        writer.WriteStartElement("stack-trace");
                        writer.WriteCData(StackTrace);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();    
                }
            }

            writer.WriteEndElement();
        }
        #endregion

        public override string ToString()
        {
            return Name;
        }

#if AVM
        internal Native.Function Func;
#endif

        public void Run()
        {
            int start = Environment.TickCount;
#if AVM
            Func.call(null, this);
#endif
            int t = Environment.TickCount - start;
            Time = t / 1000.0;
        }
    }
}