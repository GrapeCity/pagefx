using System;

namespace DataDynamics.PageFX.Common.Utilities
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class FormatAttribute : Attribute
    {
        public string Value;

        public FormatAttribute(string format)
        {
            Value = format;
        }
    }
}