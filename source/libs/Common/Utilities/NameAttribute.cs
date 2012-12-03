using System;

namespace DataDynamics.PageFX.Common.Utilities
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class NameAttribute : Attribute
    {
        public string Name;

        public NameAttribute(string name)
        {
            Name = name;
        }
    }
}