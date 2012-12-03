using System.Xml;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX
{
    internal class TestNode : StatNode
    {
        public string FullName;
        public IMethod Method;

        public override NodeKind NodeKind
        {
            get { return NodeKind.Test; }
        }
        
        public override string Class
        {
            get
            {
                if (IsSuite) return "test-suite";
                return "test-case";
            }
        }

        public override string Image
        {
            get
            {
                if (Success) return Images.Passed;
                return Images.Failed;
            }
        }

        public bool IsSuite
        {
            get { return Kids.Count > 0; }
        }

        protected override void WriteLabelEx(XmlWriter writer)
        {
            base.WriteLabelEx(writer);

            bool isSuite = IsSuite;
            foreach (var stat in Stats)
                stat.Write(writer, true, isSuite, true);
        }

        public override string FormatUrl(int id)
        {
            return "test." + FullName + "." + id;
        }
    }
}