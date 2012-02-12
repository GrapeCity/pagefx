using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DataDynamics.PageFX.CodeModel
{
    //Writes api-info in format of mono-api-info tool.
    public static class ApiInfo
    {
        static readonly string[] ExcludedNamespaces =
        {
            //"Microsoft.Win32.SafeHandles",
            //"System.Collections",
            //"System.Collections.Specialized",
            //"System.IO.IsolatedStorage",
            //"System.Runtime.ConstrainedExecution",
            //"System.Runtime.Serialization",
            //"System.Runtime.Versioning",
            //"System.Reflection.Emit",
            //"System.Diagnostics.SymbolStore",
            //"System.Security",
            //"System.Security.Cryptography",
            //"System.Security.Cryptography.X509Certificates",
            //"System.Security.Principal",
            //"System.Text",
            //"System.Threading",
            //"System.Timers",
            //"System.Globalization",
        };

        static readonly string[] ExcludedTypes =
        {
            //"System.Array",
            //"System.Convert",
            //"System.Console",
            //"System.ICloneable",
            //"System.ICustomFormatter",
            //"System.TimeSpan",
            //"System.Guid",
            //"System.ComponentModel.CollectionChangeEventArgs",
            //"System.ComponentModel.CollectionChangeEventHandler",
        };

        static bool IsExcludedSystemType(IType type)
        {
            var st = type.SystemType;
            if (st == null) return false;
            switch (st.Code)
            {
                case SystemTypeCode.Array:
                case SystemTypeCode.Boolean:
                case SystemTypeCode.Char:
                case SystemTypeCode.Decimal:
                case SystemTypeCode.DateTime:
                case SystemTypeCode.Double:
                case SystemTypeCode.Int16:
                case SystemTypeCode.Int32:
                case SystemTypeCode.Int64:
                case SystemTypeCode.Int8:
                case SystemTypeCode.IntPtr:
                case SystemTypeCode.Single:
                case SystemTypeCode.UInt16:
                case SystemTypeCode.UInt32:
                case SystemTypeCode.UInt64:
                case SystemTypeCode.UInt8:
                case SystemTypeCode.UIntPtr:
                case SystemTypeCode.String:
                case SystemTypeCode.Delegate:
                case SystemTypeCode.Object:
                case SystemTypeCode.MulticastDelegate:
                case SystemTypeCode.Enum:
                case SystemTypeCode.Type:
                case SystemTypeCode.ValueType:
                    return true;
            }
            return false;
        }

        public static bool ExcludeGenerics;

        public static bool TypeFilter(IType type)
        {
            if (type == null) return false;
            if (!type.IsVisible) return false;
            //if (type.IsEnum) return false;
            if (ExcludeGenerics && type is IGenericType) return false;

            if (IsExcludedSystemType(type)) return false;
            if (TypeFilters.IsPageFX(type)) return false;
            //if (TypeFilters.IsAttribute(type)) return false;
            //if (TypeFilters.IsException(type)) return false;
            if (Algorithms.Contains(ExcludedNamespaces, type.Namespace)) return false;
            if (Algorithms.Contains(ExcludedTypes, type.FullName)) return false;
            return true;
        }

        public static void Write(XmlWriter writer, IAssembly assembly)
        {
            Write(writer, assembly, TypeFilter);
        }

        public static void Write(XmlWriter writer, IAssembly assembly, Predicate<IType> typeFilter)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            var impl = new Impl {typeFilter = typeFilter};
            impl.Write(writer, assembly);
        }

        class Impl
        {
            class Namespace
            {
                public string Name { get; set; }
                public readonly List<IType> Types = new List<IType>();
            }
            readonly Hashtable _nscache = new Hashtable();
            readonly List<Namespace> _namespaces = new List<Namespace>();
            public Predicate<IType> typeFilter;

            public void Write(XmlWriter writer, IAssembly assembly)
            {
                writer.WriteStartElement("assembly");
                writer.WriteAttributeString("name", assembly.Name);
                writer.WriteAttributeString("version", assembly.Version.ToString());

                WriteTypes(writer, assembly);

                writer.WriteEndElement();
            }

            #region WriteTypes
            void WriteTypes(XmlWriter writer, IAssembly assembly)
            {
                foreach (var type in Util.GetTypes(assembly))
                {
                    if (typeFilter != null && !typeFilter(type))
                        continue;
                    
                    string nsname = type.Namespace ?? "";

                	var ns = _nscache[nsname] as Namespace;
                    if (ns == null)
                    {
                        ns = new Namespace {Name = nsname};
                        _nscache[nsname] = ns;
                        _namespaces.Add(ns);
                    }
                    ns.Types.Add(type);
                }

                if (_namespaces.Count <= 0) return;

                _namespaces.Sort((x, y) => string.Compare(x.Name, y.Name));

                writer.WriteStartElement("namespaces");
                foreach (var ns in _namespaces)
                    WriteNamespace(writer, ns);
                writer.WriteEndElement();
            }
            #endregion

            #region WriteNamespace
            static void WriteNamespace(XmlWriter writer, Namespace ns)
            {
                writer.WriteStartElement("namespace");
                writer.WriteAttributeString("name", ns.Name);

                if (ns.Types.Count > 0)
                {
                    ns.Types.Sort((x, y) => string.Compare(x.Name, y.Name));
                    WriteClasses(writer, ns.Types);
                }

                writer.WriteEndElement();
            }
            #endregion

            #region WriteAttributes
            static void WriteAttributes(XmlWriter writer, ICustomAttributeProvider cp)
            {
                var attrs = cp.CustomAttributes;
                if (attrs == null) return;
                int n = attrs.Count;
                if (n <= 0) return;
                //writer.WriteStartElement("attributes");
                //for (int i = 0; i < n; ++i)
                //{
                //}
            }
            #endregion

            #region WriteClass
            static void WriteClasses(XmlWriter writer, IEnumerable<IType> set)
            {
                bool parent = false;
                foreach (var type in set)
                {
                    if (!parent)
                    {
                        writer.WriteStartElement("classes");
                        parent = true;
                    }
                    WriteClass(writer, type);
                }
                if (parent)
                    writer.WriteEndElement();
            }

            static void WriteClass(XmlWriter writer, IType type)
            {
                writer.WriteStartElement("class");
                writer.WriteAttributeString("name", type.DisplayName);
                writer.WriteAttributeString("type", Util.GetClassType(type));
                
                WriteClassAttrs(writer, type);

                WriteAttributes(writer, type);
                WriteInterfaces(writer, type);
                WriteMembers(writer, type);

                writer.WriteEndElement();
            }

            static void WriteClassAttrs(XmlWriter writer, IType type)
            {
                if (type.BaseType != null)
                    writer.WriteAttributeString("base", type.BaseType.FullName);

                if (type.IsSealed)
                    writer.WriteAttributeString("sealed", "true");

                if (type.IsAbstract)
                    writer.WriteAttributeString("abstract", "true");

                //TODO:
                //serializable
                //charset
                //layout

                if (type.IsEnum)
                {
                    writer.WriteAttributeString("enumtype", type.ValueType.FullName);
                }
            }
            #endregion

            #region WriteInterfaces
            static bool FilterInterface(IType iface)
            {
                if (!iface.IsVisible) return false;
                //In silverlight System.ICloneable is internal
                if (iface.FullName == "System.ICloneable") return false;
                return true;
            }

            static void WriteInterfaces(XmlWriter writer, IType type)
            {
                var ifaces = type.Interfaces;
                if (ifaces == null) return;
                bool parent = false;
                foreach (var iface in ifaces)
                {
                    // we're only interested in public interfaces
                    if (!FilterInterface(iface)) continue;
                    if (!parent)
                    {
                        writer.WriteStartElement("interfaces");
                        parent = true;
                    }
                    writer.WriteStartElement("interface");
                    writer.WriteAttributeString("name", iface.FullName);
                    writer.WriteEndElement();
                }
                if (parent)
                    writer.WriteEndElement();
            }
            #endregion

            #region WriteMembers
            static void WriteMembers(XmlWriter writer, IType type)
            {
                WriteFields(writer, type);
                WriteConstructors(writer, type);
                WriteProperties(writer, type);
                WriteEvents(writer, type);
                WriteMethods(writer, type);
                WriteNestedTypes(writer, type);
            }
            #endregion

            #region WriteFields
            static void WriteFields(XmlWriter writer, IType type)
            {
                bool parentTag = false;
                foreach (var field in Util.GetFields(type))
                {
                    if (!parentTag)
                    {
                        writer.WriteStartElement("fields");
                        parentTag = true;
                    }
                    WriteField(writer, field);
                }
                if (parentTag)
                    writer.WriteEndElement();
            }

            static void WriteField(XmlWriter writer, IField field)
            {
                writer.WriteStartElement("field");
                writer.WriteAttributeString("name", field.Name);
                WriteAttrib(writer, field);
                WriteFieldValue(writer, field);
                writer.WriteEndElement();
            }

            static void WriteAttrib(XmlWriter writer, ITypeMember m)
            {
                string s = Util.GetAttrib(m);
                if (string.IsNullOrEmpty(s)) return;
                writer.WriteAttributeString("attrib", s);
            }

            static void WriteFieldValue(XmlWriter writer, IField field)
            {
                if (!field.IsConstant) return;
                var v = field.Value;
                if (v == null) return;
                if (v is Enum)
                {
                    writer.WriteAttributeString("value", ((Enum)v).ToString("D"));
                    return;
                }
                if (v is char)
                {
                    writer.WriteAttributeString("value", XmlConvert.ToString((char)v));
                    return;
                }
                writer.WriteAttributeString("value", v.ToString());
            }
            #endregion

            #region WriteConstructors
            static void WriteConstructors(XmlWriter writer, IType type)
            {
                bool parentTag = false;
                foreach (var ctor in Util.GetConstructors(type))
                {
                    if (!parentTag)
                    {
                        writer.WriteStartElement("constructors");
                        parentTag = true;
                    }
                    WriteMethod(writer, ctor, "contsructor");
                }
                if (parentTag)
                    writer.WriteEndElement();
            }
            #endregion

            #region WriteMethod
            static void WriteMethods(XmlWriter writer, IEnumerable<IMethod> methods)
            {
                bool parent = false;
                foreach (var m in methods)
                {
                    if (!parent)
                    {
                        writer.WriteStartElement("methods");
                        parent = true;
                    }
                    WriteMethod(writer, m, "method");
                }
                if (parent)
                    writer.WriteEndElement();
            }

            static void WriteMethod(XmlWriter writer, IMethod method, string tag)
            {
                writer.WriteStartElement(tag);
                writer.WriteAttributeString("name", GetMethodName(method, true));
                WriteAttrib(writer, method);
                if (method.IsAbstract)
                    writer.WriteAttributeString("abstract", "true");
                if (method.IsVirtual)
                    writer.WriteAttributeString("virtual", "true");
                if (method.IsFinal)
                    writer.WriteAttributeString("final", "true");
                if (method.IsStatic)
                    writer.WriteAttributeString("static", "true");

                if (!method.IsConstructor)
                {
                	writer.WriteAttributeString("returntype", method.Type == null ? "void" : method.Type.FullName);
                }

                WriteParameters(writer, method.Parameters);

                writer.WriteEndElement();
            }
            #endregion

            #region WriteParameters
            static void WriteParameters(XmlWriter writer, IParameterCollection args)
            {
                writer.WriteStartElement("parameters");
                if (args != null)
                {
                    foreach (var p in args)
                        WriteParameter(writer, p);
                }
                writer.WriteEndElement();
            }

            static void WriteParameter(XmlWriter writer, IParameter p)
            {
                writer.WriteStartElement("parameter");
                writer.WriteAttributeString("name", p.Name);
                writer.WriteAttributeString("position", p.Index.ToString());
                writer.WriteAttributeString("attrib", "0");
                writer.WriteAttributeString("type", p.Type.FullName);

                //TODO: optional params

                string dir = Util.GetDirection(p);
                if (dir != "in")
                    writer.WriteAttributeString("direction", dir);

                writer.WriteEndElement();
            }
            #endregion

            #region WriteProperties
            static void WriteProperties(XmlWriter writer, IType type)
            {
                bool parent = false;
                foreach (var prop in Util.GetProperties(type))
                {
                    if (!parent)
                    {
                        writer.WriteStartElement("properties");
                        parent = true;
                    }
                    WriteProperty(writer, prop);
                }
                if (parent)
                    writer.WriteEndElement();
            }

            static void WriteProperty(XmlWriter writer, IProperty prop)
            {
                writer.WriteStartElement("property");
                writer.WriteAttributeString("name", prop.Name);
                WriteAttrib(writer, prop);
                writer.WriteAttributeString("ptype", prop.Type.FullName);

                //NOTE: in mono-api-diff params attribute is required.
                string sig = Util.GetSignature(prop.Parameters, false);
                if (string.IsNullOrEmpty(sig))
                    sig = "";
                writer.WriteAttributeString("params", sig);

                if (prop.Getter != null && prop.Setter != null)
                    WriteMethods(writer, new[] { prop.Getter, prop.Setter });
                else if (prop.Getter != null)
                    WriteMethods(writer, new[] { prop.Getter });
                else if (prop.Setter != null)
                    WriteMethods(writer, new[] { prop.Setter });

                writer.WriteEndElement();
            }
            #endregion

            #region WriteEvents
            static void WriteEvents(XmlWriter writer, IType type)
            {
                bool parent = false;
                foreach (var e in Util.GetEvents(type))
                {
                    if (!parent)
                    {
                        writer.WriteStartElement("events");
                        parent = true;
                    }
                    WriteEvent(writer, e);
                }
                if (parent)
                    writer.WriteEndElement();
            }

            static void WriteEvent(XmlWriter writer, IEvent e)
            {
                writer.WriteStartElement("event");
                writer.WriteAttributeString("name", e.Name);
                WriteAttrib(writer, e);
                writer.WriteAttributeString("eventtype", e.Type.FullName);

                if (e.Adder != null && e.Remover != null)
                    WriteMethods(writer, new[] { e.Adder, e.Remover });
                else if (e.Adder != null)
                    WriteMethods(writer, new[] { e.Adder });
                else if (e.Remover != null)
                    WriteMethods(writer, new[] { e.Remover });

                writer.WriteEndElement();
            }
            #endregion

            #region WriteMethods
            static void WriteMethods(XmlWriter writer, IType type)
            {
                WriteMethods(writer, Util.GetMethods(type));
            }
            #endregion

            #region WriteNestedTypes
            static void WriteNestedTypes(XmlWriter writer, IType type)
            {
                var types = type.Types;
                if (types == null) return;
                if (types.Count <= 0) return;
                var list = new List<IType>();
                foreach (var nt in types)
                {
                    if (!nt.IsVisible) continue;
                    list.Add(nt);
                }
                if (list.Count > 0)
                {
                    list.Sort((x, y) => string.Compare(x.Name, y.Name));
                    WriteClasses(writer, list);
                }
            }
            #endregion
        }

        public static string GetFullMethodName(IMethod method, bool displayName)
        {
            return method.DeclaringType.FullName + "." + GetMethodName(method, displayName);
        }

        public static string GetFullMethodName(IMethod method)
        {
            return GetFullMethodName(method, false);
        }

        public static string GetMethodName(IMethod method, bool displayName)
        {
            string name = method.Name;
            if (method.GenericParameters.Count > 0)
            {
                name += GenericType.Format(method.GenericParameters, TypeNameKind.DisplayName, false);
            }
            string parms = Util.GetSignature(method.Parameters, displayName);
            return String.Format("{0}({1})", name, parms);
        }

        public static string GetMethodName(IMethod method)
        {
            return GetMethodName(method, false);
        }

        class Util
        {
            public static IType[] GetTypes(ITypeContainer assembly)
            {
                var types = new List<IType>();
                foreach (var type in assembly.Types)
                {
                    if (type.DeclaringType != null)
                        continue;
                    if (type.IsVisible)
                        types.Add(type);
                }
                return types.ToArray();
            }

            public static T[] GetVisibleMembers<T>(IEnumerable<T> set)
                where T:ITypeMember
            {
                var list = new List<T>();
                foreach (var m in set)
                {
                    // we're only interested in public or protected members
                    if (!IsVisibleMember(m))
                        continue;
                    list.Add(m);
                }
                list.Sort((x, y) => string.Compare(x.Name, y.Name));
                return list.ToArray();
            }

            public static IField[] GetFields(IType type)
            {
                return GetVisibleMembers(type.Fields);
            }

            public static IProperty[] GetProperties(IType type)
            {
                return GetVisibleMembers(type.Properties);
            }

            public static IEvent[] GetEvents(IType type)
            {
                return GetVisibleMembers(type.Events);
            }

            public static IMethod[] GetConstructors(IType type)
            {
                var list = new List<IMethod>();
                foreach (var m in type.Methods)
                {
                    if (!IsVisibleMember(m)) continue;
                    if (!m.IsConstructor) continue;
                    if (type.IsAbstract) continue;

                    list.Add(m);
                }
                list.Sort((x, y) => string.Compare(x.Name, y.Name));
                return list.ToArray();
            }

            static bool FilterMethod(IMethod m)
            {
                if (!IsVisibleMember(m)) return false;
                if (m.IsConstructor) return false;
                if (ExcludeGenerics && m.GenericParameters.Count > 0) return false;
                if (m.Association != null) return false;
                return true;
            }

            public static IMethod[] GetMethods(IType type)
            {
                var list = new List<IMethod>();
                foreach (var m in type.Methods)
                {
                    if (!FilterMethod(m)) continue;
                    list.Add(m);
                }
                list.Sort((x, y) => string.Compare(x.Name, y.Name));
                return list.ToArray();
            }

            public static string GetModifier(IParameter p)
            {
                if (p.IsByRef)
                    return p.IsOut ? "out " : "ref ";
                return p.IsIn ? "in " : "";
            }

            public static string GetDirection(IParameter p)
            {
                if (p.IsByRef)
                    return p.IsOut ? "out" : "ref";
                return "in";
            }

            static IType UnwrapRefType(IType type)
            {
                while (type.TypeKind == TypeKind.Reference)
                {
                    type = ((ICompoundType)type).ElementType;
                }
                return type;
            }

            static string GetArgTypeName(IType type, bool displayName)
            {
                type = UnwrapRefType(type);
                if (displayName)
                    return type.DisplayName;
                return type.FullName.Replace('<', '[').Replace('>', ']');
            }

            public static string GetSignature(IEnumerable<IParameter> args, bool displayName)
            {
                if (args == null) return "";
                
                var sb = new StringBuilder();
                foreach (var p in args)
                {
                    string modifier = GetModifier(p);
                    string type_name = GetArgTypeName(p.Type, displayName);
                    sb.AppendFormat("{0}{1}, ", modifier, type_name);
                }

                if (sb.Length > 0)
                    sb.Length -= 2; // remove ", "
                return sb.ToString();
            }

            public static bool IsVisibleMember(ITypeMember m)
            {
                switch (m.Visibility)
                {
                    case Visibility.NestedProtected:
                    case Visibility.NestedProtectedInternal:
                    case Visibility.Protected:
                    case Visibility.ProtectedInternal:
                    case Visibility.Public:
                    case Visibility.NestedPublic:
                        return true;
                }
                return false;
            }

            public static string GetClassType(IType type)
            {
                switch (type.TypeKind)
                {
                    case TypeKind.Class:
                        return "class";
                    case TypeKind.Delegate:
                        return "delegate";
                    case TypeKind.Enum:
                        return "enum";
                    case TypeKind.Interface:
                        return "interface";
                    case TypeKind.Struct:
                    case TypeKind.Primitive:
                        return "struct";
                    default:
                        return "";
                }
            }

            public static string GetAttrib(ITypeMember m)
            {
                //TODO:
                return "0";
            }
        }
    }
}