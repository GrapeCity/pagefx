using System.Xml;

namespace DataDynamics.PageFX
{
    static class Images
    {
        public const string Passed = "success.png";
        public const string Failed = "failed.png";
        public const string Unexecuted = "unknown.png";

        public const string PassedSmall = Passed;
        public const string FailedSmall = Failed;

        public const string Flash = "fp.png";
        public const string AVM = "avmplus.png";
        public const string Time = "time.png";

        public const string Assembly = "assembly.png";
        public const string Namespace = "namespace.png";
        public const string Class = "class.png";
        public const string Interface = "interface.png";
        public const string Struct = "struct.png";
        public const string Delegate = "delegate.png";
        public const string Enum = "enum.png";
        public const string Method = "method.png";
        public const string Constructor = "constructor.png";
        public const string Operator = "operator.png";
        public const string NoTests = "notests.png";

        public static string GetSuccessImage(bool success)
        {
            return success ? Passed : Failed;
        }

        public static string Get(Runtime runtime)
        {
            switch (runtime)
            {
                case Runtime.FP10:
                    return Flash;
            }
            return AVM;
        }

        public static void WriteStatusImage(XmlWriter writer, bool success)
        {
            string src = success ? PassedSmall : FailedSmall;
            Html.IMG(writer, src);
        }
    }

    static class Xmlns
    {
        //public const string XHtml = "http://www.w3.org/1999/xhtml";
        //public const string XHtml = "";
    }
}