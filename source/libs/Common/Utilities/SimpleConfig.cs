using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Xml;
using DataDynamics.PageFX.Common.Graphics;

namespace DataDynamics.PageFX.Common.Utilities
{
    public sealed class SimpleConfig
    {
        private readonly Hashtable _table = new Hashtable();

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

	    public T Get<T>(string key, T defval, Predicate<T> validator)
	    {
		    T value;
		    switch (Type.GetTypeCode(typeof(T)))
		    {
			    case TypeCode.Boolean:
				    value = (T) (object) GetBool(key, (bool) (object) defval);
					break;
			    case TypeCode.Int32:
				    value = (T) (object) GetInt32(key, (int) (object) defval);
					break;
				case TypeCode.Single:
					value = (T)(object)GetFloat(key, (float)(object)defval);
					break;
				case TypeCode.Double:
					value = (T)(object)GetDouble(key, (double)(object)defval);
					break;
			    case TypeCode.String:
				    value = (T) (object) this[key, (string) (object) defval];
					break;
			    default:
				    if (typeof(T) == typeof(Color))
				    {
					    value = (T) (object) GetColor(key, (Color) (object) defval);
						break;
				    }
				    throw new NotSupportedException();
		    }

		    if (validator != null && !validator(value))
			    return defval;

		    return value;
	    }

		public T Get<T>(string key, T defval)
		{
			return Get(key, defval, null);
		}

		public void Set<T>(string key, T value)
		{
			if (typeof(T) == typeof(Color))
			{
				var color = (Color) (object) value;
				this[key] = string.Format("#{0:x2}{1:x2}{2:x2}", color.R, color.G, color.B);
				return;
			}

			if (typeof(T) == typeof(bool))
			{
				this[key] = value.ToString().ToLower();
				return;
			}

			this[key] = Convert.ToString(value, CultureInfo.InvariantCulture);
		}

	    private bool GetBool(string key, bool defval)
        {
            string s = this[key, defval ? "true" : "false"];
            if (string.IsNullOrEmpty(s)) return defval;
            if (string.Equals(s, "true", StringComparison.OrdinalIgnoreCase)) return true;
            if (string.Equals(s, "false", StringComparison.OrdinalIgnoreCase)) return false;
            int v;
            if (int.TryParse(s, out v)) return v != 0;
            return defval;
        }

        private int GetInt32(string key, int defval)
        {
            string s = this[key, Convert.ToString(defval, CultureInfo.InvariantCulture)];
            int v;
            if (!int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out v))
				return defval;
            return v;
        }

		private float GetFloat(string key, float defval)
		{
			string s = this[key, Convert.ToString(defval, CultureInfo.InvariantCulture)];
			float v;
			if (!float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out v))
				return defval;
			return v;
		}

		private double GetDouble(string key, double defval)
		{
			string s = this[key, Convert.ToString(defval, CultureInfo.InvariantCulture)];
			double v;
			if (!double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out v))
				return defval;
			return v;
		}

        private Color GetColor(string key, Color defval)
        {
            string s = this[key, "#" + defval.R.ToString("x2") + defval.G.ToString("x2") + defval.B.ToString("x2")];
            var c = defval;
            if (s.TryParseColor(ref c))
                return c;
            return defval;
        }

        #region Load/Save

		private sealed class LoadState
		{
			public string Key = "";
			private bool _root = true;

			public void Push(string name)
			{
				if (_root)
				{
					_root = false;
					return;
				}
				Key = Key.Length > 0 ? Key + "." + name : name;
			}

			public void Pop()
			{
				int i = Key.LastIndexOf('.');
				Key = i >= 0 ? Key.Substring(0, i) : "";
			}
		}
        
        public void Load(string path)
        {
            using (var reader = XmlReader.Create(path))
                Load(reader);
        }

        public void Load(XmlReader reader)
        {
	        var state = new LoadState();
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            bool empty = reader.IsEmptyElement;
                            string name = reader.Name;
                            state.Push(name);
                            ReadAttrs(reader);
                            if (empty) state.Pop();
                        }
                        break;

                    case XmlNodeType.Text:
		                this[state.Key] = reader.Value;
		                break;

                    case XmlNodeType.EndElement:
                        state.Pop();
                        break;
                }
            }
        }

	    private static void ReadAttrs(XmlReader reader)
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

		private void ChangeElements(XmlElement root)
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