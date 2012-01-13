using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DataDynamics.Tools
{
    class Method : Member
    {
        public string Returns;
        public List<Param> Params = new List<Param>();

        protected override void WriteBody(XmlWriter writer)
        {
            foreach (var param in Params)
                param.Write(writer);
            if (!string.IsNullOrEmpty(Returns))
                Utils.WriteSummary(writer, "returns", Returns);
        }

        protected override string NameSuffix
        {
            get
            {
                int n = Params.Count;
                if (n == 0) return "";

                var sb = new StringBuilder();
                sb.Append('(');
                for (int i = 0; i < n; ++i)
                {
                    if (i > 0) sb.Append(',');
                    var p = Params[i];
                    sb.Append(Utils.ConvertType(p.Type));
                }
                sb.Append(')');
                return sb.ToString();
            }
        }
    }

    class Param : Member
    {
        protected override bool WrapSummary
        {
            get { return false; }
        }

        protected override string XmlElementName
        {
            get { return "param"; }
        }
    }
}