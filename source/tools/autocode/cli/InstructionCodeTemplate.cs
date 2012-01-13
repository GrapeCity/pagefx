using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Xml;

namespace DataDynamics.PageFX.CLI.IL
{
    [Template("cli.InstructionCode.txt", "..\\..\\..\\..\\libs\\DataDynamics.PageFX.CLI\\IL\\InstructionCode.cs")]
    public class InstructionCodeTemplate : ITextTemplateContext
    {
        private readonly List<OpCode> _codes;
        private readonly List<string> _names;
        private readonly List<string> _desc;
        private int _index;

        private static string[] GetXmlSummary(string s)
        {
            var list = new List<string>();
            foreach (var line in Str.GetLines(s))
            {
                list.Add(XmlHelper.EntifyString(line));
            }
            return list.ToArray();
        }

        public InstructionCodeTemplate()
        {
            string mscorlibPath = typeof(int).Assembly.Location;
            string mscorlibXml = Path.ChangeExtension(mscorlibPath, "xml");

            var doc = new XmlDocument();
            doc.Load(mscorlibXml);

            _codes = new List<OpCode>();
            _names = new List<string>();
            _desc = new List<string>();

            var type = typeof(OpCodes);
            var list = type.GetFields();
            foreach (var fi in list)
            {
                if (!fi.IsStatic) continue;
                var c = (OpCode)fi.GetValue(null);
                _codes.Add(c);
                _names.Add(fi.Name);

                string docXPath =
                    string.Format("/doc/members/member[@name='F:{0}.{1}']/summary", type.FullName, fi.Name);
                var sum = doc.SelectSingleNode(docXPath) as XmlElement;
                if (sum != null)
                {
                    using (var writer = new StringWriter())
                    {
                        bool eol = false;
                        foreach (var s in GetXmlSummary(sum.InnerText))
                        {
                            if (eol)
                            {
                                writer.WriteLine();
                                writer.Write("\t\t/// {0}", s);
                            }
                            else
                            {
                                writer.Write("/// {0}", s);
                            }
                            eol = true;
                        }
                        _desc.Add(writer.ToString());
                    }
                }
                else
                {
                    _desc.Add(string.Format("// 0x{0:X}", (ushort)c.Value));
                }
            }
            _index = -1;
        }

        #region ITextTemplateContext Members
        public string Eval(string statement, string var)
        {
            if (statement == "for")
            {
                if (var == "i")
                {
                    ++_index;
                    if (_index < _codes.Count)
                        return "1";
                }
                return null;
            }
            if (var == "desc")
                return _desc[_index];
            if (var == "name")
                return _names[_index];
            if (var == "value")
                return string.Format("{0}", (int)_codes[_index].Value);
            return null;
        }
        #endregion
    }
}