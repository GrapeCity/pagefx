using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace DataDynamics.PageFX
{
    internal class TreeNode
    {
        #region Properties
        public string Name = "";

        public virtual NodeKind NodeKind
        {
            get { return NodeKind.Unknown; }
        }

        public virtual string ID
        {
            get { return ""; }
        }

        public virtual string Class
        {
            get { return ""; }
        }

        public virtual string Image
        {
            get { return ""; }
        }

        public TreeNode Parent;

        public List<TreeNode> Kids
        {
            get { return _kids; }
        }
        private readonly List<TreeNode> _kids = new List<TreeNode>();

        public bool HasChildren
        {
            get { return _kids.Count > 0; }
        }

        public TreeNode this[string name]
        {
            get { return _kids.Find(k => k.Name == name); }
        }

        public void Add(TreeNode kid)
        {
            if (kid == null)
                throw new ArgumentNullException("kid");
            if (kid.Parent != null)
                throw new InvalidOperationException();
            kid.Parent = this;
            _kids.Add(kid);
        }

        public void Sort()
        {
            _kids.Sort((x, y) => string.Compare(x.Name, y.Name));
            foreach (var kid in _kids)
                kid.Sort();
        }

        private bool IsClosed
        {
            get 
            {
                if (Parent == null) return false;
                //if (Parent.Parent == null) return false;

                if (NodeKind == NodeKind.Assembly
                    && Parent.Kids[0] == this)
                    return false;

                return true;
            }
        }
        #endregion

        #region Write
        public virtual void Write(XmlWriter writer)
        {
            writer.WriteStartElement("li");

            WriteAttrs(writer);

            WriteContentUrl(writer);

            WriteLabel(writer);

            WriteKids(writer);

            writer.WriteEndElement();
        }

        protected virtual void WriteAttrs(XmlWriter writer)
        {
            string s = ID;
            if (!string.IsNullOrEmpty(s))
                writer.WriteAttributeString("id", s);

            string klass = Class;
            if (IsClosed || ChildHtml)
            {
                if (!string.IsNullOrEmpty(klass))
                    klass += " ";
                klass += "closed";
            }

            if (!string.IsNullOrEmpty(klass))
                writer.WriteAttributeString("class", klass);
        }

        public virtual void WriteLabel(XmlWriter writer)
        {
            writer.WriteStartElement("span");
            
            WriteStatusImage(writer);

            Html.IMG(writer, Image, null);
            writer.WriteString(" " + Name);
            
            WriteLabelEx(writer);

            writer.WriteEndElement(); //span
        }

        protected virtual void WriteStatusImage(XmlWriter writer)
        {
        }

        protected virtual void WriteLabelEx(XmlWriter writer)
        {
        }

        public static bool ChildHtml;

        public void WriteKids(XmlWriter writer)
        {
            if (!HasChildren) return;

            writer.WriteStartElement("ul");
            
            foreach (var kid in _kids)
            {
                if (kid.HasChildren)
                {
                    writer.WriteStartElement("li");
                    kid.WriteAttrs(writer);
                    
                    string url = kid.Url;
                    if (!string.IsNullOrEmpty(url))
                    {
                        writer.WriteAttributeString("child-loaded", "false");
                        writer.WriteAttributeString("child-url", url);
                    }

                    kid.WriteContentUrl(writer);
                    
                    kid.WriteLabel(writer);

                    Html.WritePlaceHolder(writer);

                    writer.WriteEndElement();
                }
                else
                {
                    kid.Write(writer);
                    
                }
            }
            writer.WriteEndElement();
        }

        public void WriteContentUrl(XmlWriter writer)
        {
            string url = ContentUrl;
            if (string.IsNullOrEmpty(url)) return;
            writer.WriteAttributeString("content-loaded", "false");
            writer.WriteAttributeString("content-url", url);
        }
        #endregion

        #region Urls
        public string Url = "";
        public string ContentUrl = "";

        protected virtual bool UseNameForUrl
        {
            get { return false; }
        }

        protected virtual string UrlPrefix
        {
            get { return ""; }
        }

        public virtual string FormatUrl(int id)
        {
            string url = UrlPrefix;
            if (!string.IsNullOrEmpty(url) && url[url.Length - 1] != '.')
                url += ".";
            if (UseNameForUrl)
                url += Name;
            else
                url += "node" + id;
            return url;
        }
        #endregion

        #region WriteTo / WriteContent
        public static void WriteTo(IEnumerable<TreeNode> set, string dir)
        {
            foreach (var node in set)
                WriteTo(node, dir);
        }

        public static int GlobalID = 1;

        public static void WriteTo(TreeNode node, string dir)
        {
            WriteTo(node.Kids, dir);

            if (node.Parent == null) return;
            if (node.Parent.Parent == null) return;
            if (!node.HasChildren) return;

            var xws = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                OmitXmlDeclaration = true
            };

            string url = "tree/" + node.FormatUrl(GlobalID) + ".htm";
            string path = Path.Combine(dir, url);
            node.Url = url;

            ++GlobalID;

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            ChildHtml = true;
            using (var writer = XmlWriter.Create(path, xws))
                node.WriteKids(writer);

            ChildHtml = false;
        }

        public virtual void WriteContent(string dir)
        {
            if (HasContent)
                WriteMyContent(dir);

            foreach (var kid in _kids)
                kid.WriteContent(dir);
        }

        public virtual bool HasContent
        {
            get { return false; }
        }

        public virtual void WriteMyContent(string dir)
        {
        }
        #endregion

        #region ToXml
        public string ToXml(string path)
        {
            return ToXml("tree", path);
        }

        public string ToXml(string id, string path)
        {
            Sort();

            string dir = Path.GetDirectoryName(path);
            WriteContent(dir);
            WriteTo(Kids, dir);

            var xws = new XmlWriterSettings
                          {
                              Indent = true,
                              IndentChars = "  ",
                              OmitXmlDeclaration = true
                          };

            var xml = new StringWriter();
            using (var writer = XmlWriter.Create(xml, xws))
            {
                writer.WriteStartElement("ul");
                writer.WriteAttributeString("id", id);
                //writer.WriteAttributeString("class", "treeview-famfamfam");

                foreach (var node in Kids)
                    node.Write(writer);

                writer.WriteEndElement(); //</ul>
            }

            return xml.ToString();
        }
        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
}