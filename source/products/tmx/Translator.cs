using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using DataDynamics;

namespace tmx
{
    class Translator
    {
        #region class UIComponent
        class UIComponent
        {
            public UIComponent Parent;
            public XmlElement Element;
            public string Namespace;
            public string Type;
            public string ID;

            public readonly List<UIComponent> Children = new List<UIComponent>();
        }
        #endregion

        #region Fields
        private XmlDocument _doc;
        private List<UIComponent> _components;
        private TextWriter _writer;
        private string _name;
        #endregion

        #region Start - Entry Point
        public void Translate(string[] files)
        {
            foreach (var path in files)
            {
                Translate(path);
            }
        }

        public void Translate(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            _doc = new XmlDocument();

            try
            {
                _doc.Load(path);
            }
            catch (Exception e)
            {
                Errors.UnableToLoadMXmlFile(e);
                return;
            }

            _name = Path.GetFileNameWithoutExtension(path);
            string outpath = Path.Combine(Options.OutDir, _name + ".cs");
            try
            {
                _writer = new StreamWriter(outpath);
            }
            catch (Exception e)
            {
                Errors.UnableToCreateFile(e, outpath);
            }

            LoadComponents();

            using (_writer)
            {
                TranslateCore();
            }
        }
        #endregion

        #region LoadComponents
        private void LoadComponents()
        {
            _components = new List<UIComponent>();
            LoadComponents(_doc.DocumentElement, null);

            foreach (var c in _components)
            {
                if (string.IsNullOrEmpty(c.ID))
                    c.ID = MakeID(c.Type);
            }
        }

        private string MakeID(string type)
        {
            string prefix = char.ToLower(type[0]) + type.Substring(1);
            int n = 1;
            while (true)
            {
                string id = prefix + n;
                if (!Algorithms.Contains(_components,
                                         delegate(UIComponent c)
                                             {
                                                 return c.ID == id;
                                             }))
                {
                    return id;
                }
                ++n;
            }
        }

        private static bool IsNotUIComponent(XmlElement e)
        {
            return false;
        }

        private void LoadComponents(XmlNode compNode, UIComponent parent)
        {
            foreach (XmlNode kid in compNode.ChildNodes)
            {
                var elem = kid as XmlElement;
                if (elem != null)
                {
                    if (IsNotUIComponent(elem))
                    {
                        //TODO:
                    }
                    else
                    {
                        CreateComponent(elem, parent);
                    }
                }
            }
        }

        private static string GetNamespace(XmlNode e)
        {
            string ns = e.NamespaceURI;
            if (ns == Const.MxNamespaceUri)
            {
                ns = MX.MapNamespace(e.LocalName);
            }
            else
            {
                ns = Options.Namespace;
            }
            return ns;
        }

        private UIComponent CreateComponent(XmlElement e, UIComponent parent)
        {
            if (IsNotUIComponent(e)) return null;

            var c = new UIComponent();
            c.Element = e;
            c.Parent = parent;
            c.ID = e.GetAttribute("id");
            c.Type = e.LocalName;
            c.Namespace = GetNamespace(e);

            _components.Add(c);

            if (parent != null)
                parent.Children.Add(c);

            LoadComponents(e, c);

            return c;
        }
        #endregion

        #region TranslateCore
        private void TranslateCore()
        {
            WriteLine("//");
            WriteLine("//WARNING: This file was auto generated. DO NOT EDIT!!!");
            WriteLine("//");
            WriteLine();

            WriteUsings();

            WriteLine("namespace {0}", Options.Namespace);
            BeginBlock();

            string partial = Options.Partial ? "partial " : "";

            var root = _doc.DocumentElement;
            string super = root.LocalName;

            WriteLine("public {0} class {1} : {2}", partial, _name, super);
            BeginBlock();

            WriteFields();
            WriteCtor();

            EndBlock();

            EndBlock();
        }

        private void WriteUsings()
        {
            var nslist = new List<string>();
            var hash = new Hashtable();

            var root = _doc.DocumentElement;
            string rootNS = GetNamespace(root);
            if (!hash.Contains(rootNS))
            {
                hash[rootNS] = rootNS;
                nslist.Add(rootNS);
            }

            foreach (var c in _components)
            {
                if (!hash.Contains(c.Namespace))
                {
                    hash[c.Namespace] = c.Namespace;
                    nslist.Add(c.Namespace);
                }
            }

            foreach (var ns in nslist)
                WriteLine("using {0};", ns);
            WriteLine();
        }

        private void WriteFields()
        {
            WriteLine("#region Fields");
            foreach (var c in _components)
                WriteLine("private {0} {1};", c.Type, c.ID);
            WriteLine("#endregion");
            WriteLine();
        }

        private void WriteCtor()
        {
            WriteLine("#region ctor");
            WriteLine("public {0}()", _name);
            BeginBlock();

            WriteLine("//Instantiate components");
            foreach (var c in _components)
                WriteLine("{0} = new {1}();", c.ID, c.Type);

            //Init attributes
            foreach (var c in _components)
                SetProperties(c);
            WriteLine();

            WriteLine("//Build visual hierarchy");
            foreach (var c in _components)
            {
                if (c.Parent != null)
                    WriteLine("{0}.addChild({1});", c.Parent.ID, c.ID);
                else
                    WriteLine("addChild({0});", c.ID);
            }

            EndBlock();
            WriteLine("#endregion");
        }      
        #endregion

        #region SetProperties
        private void SetProperties(UIComponent c)
        {
            WriteLine();
            WriteLine("//Setup {0} properties", c.ID);
            foreach (XmlAttribute attr in c.Element.Attributes)
                SetProperty(c.ID, attr.Name, attr.Value);
        }

        private void SetProperty(string id, string attr, string value)
        {
            if (attr == "id")
                return;

            if (attr == "width" || attr == "height")
            {
                if (value.EndsWith("%"))
                {
                    attr = "percent" + char.ToUpper(attr[0]) + attr.Substring(1);
                    value = value.Substring(0, value.Length - 1);
                }
                SetValue(id, attr, value);
            }
            else
            {
                var type = MX.GetAttrType(attr);
                switch (type)
                {
                    case AttrType.Number:
                        SetValue(id, attr, value);
                        break;

                    case AttrType.Constraint:
                        SetStyle(id, attr, value);
                        break;

                    case AttrType.Color:
                        if (value.StartsWith("#"))
                        {
                            value = "0x" + value.Substring(1);
                            SetStyle(id, attr, value);
                        }
                        else
                        {
                            //TODO: parse color
                        }
                        break;

                    case AttrType.String:
                    default:
                        SetString(id, attr, value);
                        break;

                    case AttrType.StyleString:
                        SetStyleString(id, attr, value);
                        break;

                    case AttrType.StyleNumber:
                        SetStyle(id, attr, value);
                        break;
                }
            }
        }

        private void SetValue(string id, string attr, string value)
        {
            WriteLine("{0}.{1} = {2};", id, attr, value);
        }

        private void SetString(string id, string attr, string value)
        {
            WriteLine("{0}.{1} = \"{2}\";", id, attr, value);
        }

        private void SetStyle(string id, string attr, string value)
        {
            WriteLine("{0}.setStyle(\"{1}\", {2});", id, attr, value);
        }

        private void SetStyleString(string id, string attr, string value)
        {
            WriteLine("{0}.setStyle(\"{1}\", \"{2}\");", id, attr, value);
        }
        #endregion

        #region Write Methods
        private void BeginBlock()
        {
            WriteLine("{");
            Indent();
        }

        private void EndBlock()
        {
            Unindent();
            WriteLine("}");
        }

        private Indent _tab = new Indent("\t");

        private void Indent()
        {
            _tab++;
        }

        private void Unindent()
        {
            _tab--;
        }

        private void Write(string s)
        {
            _writer.Write(_tab + s);
        }

        private void WriteLine()
        {
            _writer.WriteLine();
        }

        private void WriteLine(string s)
        {
            Write(s);
            WriteLine();
        }

        private void Write(string format, params object[] args)
        {
            Write(string.Format(format, args));
        }

        private void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }
        #endregion
    }
}