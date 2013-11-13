using System;
using DataDynamics.PageFX.Flash.Swf.Actions;
using DataDynamics.PageFX.Flash.Swf.Tags;

namespace DataDynamics.PageFX.Flash.Swf
{
    public sealed class SwfVersionAttribute : Attribute
    {
        public SwfVersionAttribute(int version)
        {
            Version = version;
        }

	    public int Version { get; private set; }
    }

    public sealed class SwfTagCategoryAttribute : Attribute
    {
        public SwfTagCategoryAttribute(SwfTagCategory category)
        {
            Category = category;
        }

	    public SwfTagCategory Category { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SwfTagAttribute : Attribute
    {
        public SwfTagAttribute(SwfTagCode code)
        {
            Code = code;   
        }

	    public SwfTagCode Code { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SwfActionAttribute : Attribute
    {
        public SwfActionAttribute(SwfActionCode code)
        {
            Code = code;
        }

	    public SwfActionCode Code { get; private set; }
    }
}