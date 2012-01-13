using System;

namespace tmx
{
    enum AttrType
    {
        String,

        [Attr("x")]
        [Attr("y")]
        [Attr("width")]
        [Attr("height")]
        Number,

        [Attr("left")]
        [Attr("right")]
        [Attr("top")]
        [Attr("bottom")]
        [Attr("horizontalCenter")]
        [Attr("verticalCenter")]
        [Attr("baseline")]
        Constraint,

        [Attr("backgroundColor")]
        [Attr("color")]
        Color,

        [Attr("fontWeight")]
        [Attr("horizontalAlign")]
        [Attr("verticalAlign")]
        StyleString,

        [Attr("fontSize")]
        StyleNumber,
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class AttrAttribute : Attribute
    {
        public string Name;

        public AttrAttribute(string name)
        {
            Name = name;
        }
    }
}