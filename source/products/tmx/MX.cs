using System.Collections;
using System.Reflection;
using System.Xml;
using DataDynamics;

namespace tmx
{
    class MX
    {
        #region MapNamespace
        private static Hashtable _nsmap;

        private static void LadNsMap()
        {
            if (_nsmap != null) return;
            _nsmap = new Hashtable();

            var rs = ResourceHelper.GetStream(typeof(MX), "mx.xml");

            var doc = new XmlDocument();
            doc.Load(rs);

            var components = doc.DocumentElement["components"];
            if (components != null)
            {
                foreach (XmlNode cn in components.ChildNodes)
                {
                    var ce = cn as XmlElement;
                    if (ce != null)
                    {
                        string className = ce.GetAttribute("className");
                        int i = className.LastIndexOf(':');
                        if (i >= 0)
                        {
                            string ns = className.Substring(0, i).Trim();
                            string name = className.Substring(i + 1).Trim();
                            _nsmap[name] = ns;
                        }
                    }
                }
            }
        }

        public static string MapNamespace(string componentName)
        {
            LadNsMap();
            return (string)_nsmap[componentName];
        }
        #endregion

        #region GetAttrType
        private static Hashtable _attrTypes;

        private static void LoadAttrTypes()
        {
            if (_attrTypes != null) return;
            _attrTypes = new Hashtable();

            foreach (var fi in typeof(AttrType).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var type = (AttrType)fi.GetValue(null);
                var attrs = ReflectionHelper.GetAttributes<AttrAttribute>(fi, true);
                if (attrs != null)
                {
                    foreach (var attr in attrs)
                        _attrTypes[attr.Name] = type;
                }
            }
            
        }

        public static AttrType GetAttrType(string name)
        {
            LoadAttrTypes();
            var v = _attrTypes[name];
            if (v != null)
                return (AttrType)v;
            return AttrType.String;
        }
        #endregion
    }
}