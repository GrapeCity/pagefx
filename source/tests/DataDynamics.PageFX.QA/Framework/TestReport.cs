using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace DataDynamics.PageFX
{
    public class TestReport
    {
        private class Item
        {
            public string Name;
            public string Mode;
            public string Error;

            public bool Passed
            {
                get { return string.IsNullOrEmpty(Error); }
            }
        }
        private readonly List<Item> _items = new List<Item>();

        public string Title
        {
            get
            {
                if (string.IsNullOrEmpty(_title))
                    return string.Format("QA Report {0}", DateTime.Now);
                return _title;
            }
            set { _title = value; }
        }
        private string _title;

        public void Add(string name, string mode, string error)
        {
        	_items.Add(new Item
        	           	{
        	           		Name = name,
        	           		Mode = mode,
        	           		Error = error
        	           	});
        }

        public void Add(TestCase testCase, string mode, string error)
        {
            Add(testCase.FullName, mode, error);
        }

        public bool HasErrors
        {
            get
            {
                foreach (var item in _items)
                {
                    if (!item.Passed)
                        return true;
                }
                return false;
            }
        }

        #region Export
        private static bool IsXml(string format)
        {
            return string.IsNullOrEmpty(format) 
                || string.Compare(format, "xml", true) == 0;
        }

        public void Export(TextWriter output, string format)
        {
            if (IsXml(format))
            {
            	var xws = new XmlWriterSettings {Indent = true, IndentChars = "\t"};
            	using (var writer = XmlWriter.Create(output, xws))
                {
                    writer.WriteStartDocument();
                    writer.WriteProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"html.xslt\"");
                    writer.WriteStartElement("report");
                    writer.WriteAttributeString("title", Title);
                    foreach (var item in _items)
                    {
                        writer.WriteStartElement("item");
                        writer.WriteAttributeString("mode", item.Mode);
                        writer.WriteAttributeString("passed", item.Passed ? "1" : "0");
                        writer.WriteAttributeString("name", item.Name);
                        if (!item.Passed)
                        {
                            writer.WriteValue(item.Error);
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            else if (string.Compare(format, "wiki", true) == 0)
            {
                output.WriteLine("||*Test Case*||*Mode*||*Error*||");
                foreach (var item in _items)
                {
                    output.WriteLine("||{0}||{1}||{2}||", item.Name, item.Mode, item.Error);
                }
            }
            else if (string.Compare(format, "text", true) == 0
                     || string.Compare(format, "txt", true) == 0)
            {
                output.WriteLine("{0,10}{1,10}{2}", "Test Case", "Mode", "Error");
                foreach (var item in _items)
                {
                    output.WriteLine("{0,10}{1,10}{2}", item.Name, item.Mode, item.Error);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void Export(string subpath, string format)
        {
            string path = QA.GetReportPath(subpath);
            string dir = Path.GetDirectoryName(path);
            Directory.CreateDirectory(dir);
            using (var writer = new StreamWriter(path))
            {
                Export(writer, format);
            }
            if (IsXml(format))
            {
                path = Path.Combine(dir, "html.xslt");
                var rs = ResourceHelper.GetStream(typeof(TestReport), "xslt.html.xslt");
                Stream2.SaveStream(rs, path);
            }
        }

        public string ToText()
        {
            using (var writer = new StringWriter())
            {
                Export(writer, "text");
                return writer.ToString();
            }
        }
        #endregion

        #region Show
        public void Show()
        {
            string path = "c:\\QA\\report.xml";
            Export(path, "xml");
            QA.ShowBrowser(Title, path, true);
        }
        #endregion
    }
}