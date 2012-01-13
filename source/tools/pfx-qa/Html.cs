using System.Xml;

namespace DataDynamics.PageFX
{
    internal static class Html
    {
        public static void H(XmlWriter writer, string title, int level)
        {
            writer.WriteElementString("h" + level, title);
        }

        public static void H1(XmlWriter writer, string title)
        {
            H(writer, title, 1);
        }

        public static void H2(XmlWriter writer, string title)
        {
            H(writer, title, 2);
        }

        public static void H3(XmlWriter writer, string title)
        {
            H(writer, title, 3);
        }

        public static void TH(XmlWriter writer, string title)
        {
            writer.WriteElementString("th", title);
        }

        public static void TD(XmlWriter writer, string title)
        {
            writer.WriteElementString("td", title);
        }

        //public void 

        public static void LinkCss(XmlWriter writer, string href)
        {
            writer.WriteStartElement("link");
            writer.WriteAttributeString("rel", "stylesheet");
            writer.WriteAttributeString("type", "text/css");
            writer.WriteAttributeString("href", href);
            writer.WriteEndElement();
        }

        public static void LinkJS(XmlWriter writer, string src)
        {
            //<script src="../lib/jquery.js" type="text/javascript"></script>
            writer.WriteStartElement("script");
            writer.WriteAttributeString("type", "text/javascript");
            writer.WriteAttributeString("src", src);
            writer.WriteEndElement();
        }

        private static string ToString(VAlign va)
        {
            switch (va)
            {
                case VAlign.Middle:
                    return "middle";
            }
            return "baseline";
        }

        public static void IMG(XmlWriter writer, string src, string alt)
        {
            writer.WriteStartElement("img");
            writer.WriteAttributeString("src", "images/" + src);

            if (!string.IsNullOrEmpty(alt))
            {
                writer.WriteAttributeString("alt", alt);
                writer.WriteAttributeString("title", alt);
            }

            writer.WriteEndElement();
        }

        public static void IMG(XmlWriter writer, string src)
        {
            IMG(writer, src, null);
        }

        public static void WritePlaceHolder(XmlWriter writer)
        {
            writer.WriteStartElement("ul");
            //writer.WriteAttributeString("style", "display: none;");

            writer.WriteStartElement("li");
            //writer.WriteAttributeString("class", "last");

            writer.WriteStartElement("span");
            writer.WriteAttributeString("class", "placeholder");
            writer.WriteRaw("&nbsp;");
            writer.WriteEndElement(); //span

            writer.WriteEndElement(); //li
            writer.WriteEndElement(); //ul
        }

        //public static void WriteHitArea(XmlWriter writer, bool expandable)
        //{
        //    //<div class="hitarea closed-hitarea collapsable-hitarea"/>
        //    //<div class="hitarea closed-hitarea expandable-hitarea"/>
        //    writer.WriteStartElement("div");
        //    if (expandable)
        //        writer.WriteAttributeString("class", "hitarea closed-hitarea expandable-hitarea");
        //    else
        //        writer.WriteAttributeString("class", "hitarea closed-hitarea collapsable-hitarea");
        //    writer.WriteEndElement();
        //}

        public static void BR(XmlWriter writer)
        {
            writer.WriteStartElement("br");
            writer.WriteEndElement();
        }
    }

    internal enum VAlign
    {
        Baseline,
        Middle,
    }
}