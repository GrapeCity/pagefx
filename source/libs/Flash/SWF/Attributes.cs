using System;
using DataDynamics.PageFX.FlashLand.Swf.Actions;
using DataDynamics.PageFX.FlashLand.Swf.Tags;

namespace DataDynamics.PageFX.FlashLand.Swf
{
    public class SwfVersionAttribute : Attribute
    {
        public SwfVersionAttribute(int version)
        {
            _version = version;
        }

        public int Version
        {
            get { return _version; }
        }
        private readonly int _version;
    }

    public class SwfTagCategoryAttribute : Attribute
    {
        public SwfTagCategoryAttribute(SwfTagCategory category)
        {
            _category = category;
        }

        public SwfTagCategory Category
        {
            get { return _category; }
        }
        private readonly SwfTagCategory _category;
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class SwfTagAttribute : Attribute
    {
        public SwfTagAttribute(SwfTagCode code)
        {
            _code = code;   
        }

        public SwfTagCode Code
        {
            get { return _code; }
        }
        private readonly SwfTagCode _code;
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class SwfActionAttribute : Attribute
    {
        public SwfActionAttribute(SwfActionCode code)
        {
            _code = code;
        }

        public SwfActionCode Code
        {
            get { return _code; }
        }
        private readonly SwfActionCode _code;
    }
}