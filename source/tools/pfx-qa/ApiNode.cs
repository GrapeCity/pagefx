using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace DataDynamics.PageFX
{
    internal class ApiNode : StatNode
    {
        public List<TestNode> Tests = new List<TestNode>();

        public void AddTest(TestNode test)
        {
            if (!Tests.Contains(test))
                Tests.Add(test);
        }

        public void AddTests(IEnumerable<TestNode> set)
        {
            foreach (var test in set)
                AddTest(test);
        }

        public virtual bool HasCommonStatusIcon
        {
            get 
            {
                switch (NodeKind)
                {
                    case NodeKind.Assembly:
                        return false;
                }
                return Stats.Any(s => s.Total != 0);
            }
        }

        protected override void WriteLabelEx(XmlWriter writer)
        {
            base.WriteLabelEx(writer);

            if (NodeKind == NodeKind.Method)
                Html.BR(writer);

            foreach (var stat in Stats)
                stat.Write(writer, true, true, false);
        }

        protected override void WriteStatusImage(XmlWriter writer)
        {
            if (HasCommonStatusIcon)
            {
                Images.WriteStatusImage(writer, Success);
            }
        }

        public override void WriteMyContent(string dir)
        {
            ContentUrl = "content/" + GlobalID + ".html";
            ++GlobalID;

            string path = Path.Combine(dir, ContentUrl);
            string content = GetContent();

            Utils.WriteTemplate(path, "HtmlTemplate.xml", "CONTENT", content);
        }

        public override bool HasContent
        {
            get { return Tests.Count > 0; }
        }

        public string GetContent()
        {
            var xws = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                OmitXmlDeclaration = true
            };

            var xml = new StringWriter();
            using (var writer = XmlWriter.Create(xml, xws))
                WriteContent(writer);

            return xml.ToString();
        }

        private void WriteContent(XmlWriter writer)
        {
            writer.WriteStartElement("div");
            writer.WriteAttributeString("class", "fixed");
            writer.WriteAttributeString("id", "content");
            string name = Name;
            if (!string.IsNullOrEmpty(name))
                Html.H2(writer, Name);
            WriteTestTable(writer);
            writer.WriteEndElement();
        }

        private void WriteTestTable(XmlWriter writer)
        {
            Tests.Sort((x, y) => string.Compare(x.FullName, y.FullName));

            writer.WriteStartElement("table");
            //writer.WriteAttributeString("style", "height: 100%");

            writer.WriteStartElement("tr");
            Html.TH(writer, "TestName");
            Html.TH(writer, "FP10");
            Html.TH(writer, "AVM");
            writer.WriteEndElement(); //</tr>

            foreach (var test in Tests)
            {
                writer.WriteStartElement("tr");

                WriteTestCells(writer, test);

                writer.WriteEndElement(); //</tr>
            }
            writer.WriteEndElement();
        }

        private static void WriteTestCells(XmlWriter writer, TestNode test)
        {
            writer.WriteStartElement("td");
            writer.WriteStartElement("span");
            writer.WriteAttributeString("class", "imgmiddle");
            Images.WriteStatusImage(writer, test.Success);
            writer.WriteString(test.FullName);
            writer.WriteEndElement();
            writer.WriteEndElement();

            foreach (var stat in test.Stats)
            {
                writer.WriteStartElement("td");
                writer.WriteAttributeString("class", "imgmiddle");
                stat.Write(writer, false, false, true);
                writer.WriteEndElement();
            }
        }

        public void UpdateStat(MethodNode node)
        {
            UpdateStats(node);
            
            if (NodeKind == NodeKind.Class)
                AddTests(node.Tests);
        }
    }
}