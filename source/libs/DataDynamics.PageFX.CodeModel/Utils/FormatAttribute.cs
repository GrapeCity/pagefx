using System;

namespace DataDynamics
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