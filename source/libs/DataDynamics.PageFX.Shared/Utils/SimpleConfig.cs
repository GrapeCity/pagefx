using System;
using System.Collections;
using System.Drawing;
using System.Xml;

namespace DataDynamics.PageFX
{
    public class SimpleConfig
    {
        readonly Hashtable _table = new Hashtable();

        public SimpleConfig()
        {
        }

        public SimpleConfig(string path)
        {
            Load(path);
        }

        public string this[string key, string defval]
        {
            get 
            { 
                var s = _table[key] as string;
                if (s == null) return defval;
                return s;
            }
        }

        public string this[string key]
        {
            get { return this[key, null]; }
            set { _table[key] = value; }
        }

        public bool GetBool(string key, bool defval)
        {
            string s = this[key, defval ? "true" : "false"];
            if (string.IsNullOrEmpty(s)) return defval;
            if (string.Compare(s, "true", StringComparison.OrdinalIgnoreCase) == 0) return true;
			if (string.Compare(s, "false", StringComparison.OrdinalIgnoreCase) == 0) return true;
            int v;
            if (int.TryParse(s, out v)) return v != 0;
            return defval;
        }

        public int GetInt32(string key, int defval, Predicate<int> validator)
        {
            string s = this[key, defval.ToString()];
            int v;
            if (!int.TryParse(s, out v)) return defval;
            if (validator != null && !validator(v)) return defval;
            return v;
        }

        public int GetInt32(string key, int defval)
        {
            return GetInt32(key, defval, null);
        }

        public int GetPositiveInt32(string key, int defval)
        {
            return GetInt32(key, defval, v => v > 0);
        }

        public Color GetColor(string key, Color defval)
        {
            string s = this[key, Hex.ToString(defval)];
            Color c = defval;
            if (ColorHelper.TryParse(s, ref c))
                return c;
            return defval;
        }

        #region IO
        string m_key;
        bool m_root;

        public void Load(string path)
        {
            using (var reader = XmlReader.Create(path))
                Load(reader);
        }

        public void Load(XmlReader reader)
        {
            m_key = "";
            m_root = true;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            bool empty = reader.IsEmptyElement;
                            string name = reader.Name;
                            IncKey(name);
                            ReadAttrs(reader);
                            if (empty) DecKey();
                        }
                        break;

                    case XmlNodeType.Text:
                        SetValue(reader.Value);
                        break;

                    case XmlNodeType.EndElement:
                        DecKey();
                        break;
                }
            }
        }

        void IncKey(string name)
        {
            if (m_root)
            {
                m_root = false;
                return;
            }
            m_key = m_key.Length > 0 ? m_key + "." + name : name;
        }

        void DecKey()
        {
            int i = m_key.LastIndexOf('.');
            if (i >= 0)
            {
                m_key = m_key.Substring(0, i);
            }
            else
            {
                m_key = "";
            }
        }

        void SetValue(string value)
        {
            this[m_key] = value;
        }

        static void ReadAttrs(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    //Console.WriteLine(" {0}=\"{1}\"", reader.Name, reader.Value);
                }
                // Move the reader back to the element node.
                reader.MoveToElement();
            }
        }

        public void Save(string path)
        {
            var doc = new XmlDocument();
            doc.Load(path);
            ChangeElements(doc.DocumentElement);
            var xws = new XmlWriterSettings {Indent = true, IndentChars = "  "};
            using (var writer = XmlWriter.Create(path, xws))
                doc.Save(writer);
        }

        void ChangeElements(XmlElement root)
        {
            foreach (string key in _table.Keys)
            {
                var e = Find(root, key);
                int cn = e.ChildNodes.Count;
                if (cn == 0)
                {
                    string value = (string)_table[key];
                    if (!string.IsNullOrEmpty(value))
                    {
                        e.AppendChild(root.OwnerDocument.CreateTextNode(value));
                    }
                }
                else if (cn == 1)
                {
                    var xt = e.ChildNodes[0];
                    if (xt != null)
                        xt.Value = (string)_table[key];
                }
            }
        }

        private static XmlElement Find(XmlElement parent, string key)
        {
            string[] names = key.Split('.');
            XmlElement e = null;
            foreach (string name in names)
            {
            	e = parent[name];
            	if (e == null) return null;
            	parent = e;
            }
            return e;
        }
        #endregion
    }
}