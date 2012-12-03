using System;

namespace DataDynamics.PageFX.Common.Utilities
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class StringAttribute : Attribute
    {
        public StringAttribute(string value)
        {
            Value = value;
        }

	    public string Value { get; private set; }
    }
}