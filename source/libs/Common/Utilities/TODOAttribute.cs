using System;

namespace DataDynamics.PageFX.Common.Utilities
{
    public class TODOAttribute : Attribute
    {
        public TODOAttribute()
        {
        }

        public TODOAttribute(string desc)
        {
            _desc = desc;
        }

        public string Description
        {
            get { return _desc; }
        }
        private readonly string _desc;
    }
}