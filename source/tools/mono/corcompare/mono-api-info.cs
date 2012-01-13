//
// mono-api-info.cs - Dumps public assembly information to an xml file.
//
// Authors:
//	Gonzalo Paniagua Javier (gonzalo@ximian.com)
//
// Copyright (C) 2003-2005 Novell, Inc (http://www.novell.com)
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Xml;

namespace Mono.AssemblyInfo
{
    class Driver
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[] { typeof(string).Assembly.Location };
                //return 1;
            }

            var acoll = new AssemblyCollection();

            foreach (var fullName in args)
            {
                acoll.Add(fullName);
            }

            var doc = new XmlDocument();
            acoll.Document = doc;
            acoll.DoOutput();

            //var writer = new XmlTextWriter(Console.Out);
            using (var cout = new StreamWriter("c:\\api-info.xml"))
            {
                var writer = new XmlTextWriter(cout);
                writer.Formatting = Formatting.Indented;
                XmlNode decl = doc.CreateXmlDeclaration("1.0", null, null);
                doc.InsertBefore(decl, doc.DocumentElement);
                doc.WriteTo(writer);
            }

            //Console.ReadLine();
            return 0;
        }
    }

    class AssemblyCollection
    {
        XmlDocument document;
        readonly ArrayList assemblies;

        public AssemblyCollection()
        {
            assemblies = new ArrayList();
        }

        public bool Add(string name)
        {
            var ass = LoadAssembly(name);
            if (ass == null)
                return false;

            assemblies.Add(ass);
            return true;
        }

        public void DoOutput()
        {
            if (document == null)
                throw new InvalidOperationException("Document not set");

            XmlNode nassemblies = document.CreateElement("assemblies", null);
            document.AppendChild(nassemblies);
            foreach (Assembly a in assemblies)
            {
                var data = new AssemblyData(document, nassemblies, a);
                data.DoOutput();
            }
        }

        public XmlDocument Document
        {
            set { document = value; }
        }

        static Assembly LoadAssembly(string aname)
        {
            Assembly ass = null;
            try
            {
                string name = aname;
                if (!name.EndsWith(".dll"))
                    name += ".dll";
                //ass = Assembly.LoadFrom (name);
                ass = Assembly.ReflectionOnlyLoadFrom(name);
                return ass;
            }
            catch { }

            //try {
            //    ass = Assembly.LoadWithPartialName (aname);
            //    return ass;
            //} catch { }

            return null;
        }
    }

    abstract class BaseData
    {
        protected XmlDocument document;
        protected XmlNode parent;

        protected BaseData(XmlDocument doc, XmlNode parent)
        {
            document = doc;
            this.parent = parent;
        }

        public abstract void DoOutput();

        protected void AddAttribute(XmlNode node, string name, string value)
        {
            var attr = document.CreateAttribute(name);
            attr.Value = value;
            node.Attributes.Append(attr);
        }

        public static bool IsMonoTODOAttribute(string s)
        {
            if (s == null)
                return false;
            if (//s.EndsWith ("MonoTODOAttribute") ||
                s.EndsWith("MonoDocumentationNoteAttribute") ||
                s.EndsWith("MonoExtensionAttribute") ||
                //			    s.EndsWith ("MonoInternalNoteAttribute") ||
                s.EndsWith("MonoLimitationAttribute") ||
                s.EndsWith("MonoNotSupportedAttribute"))
                return true;
            return s.EndsWith("TODOAttribute");
        }
    }

    class AssemblyData : BaseData
    {
        readonly Assembly ass;

        public AssemblyData(XmlDocument document, XmlNode parent, Assembly ass)
            : base(document, parent)
        {
            this.ass = ass;
        }

        public override void DoOutput()
        {
            if (document == null)
                throw new InvalidOperationException("Document not set");

            XmlNode nassembly = document.CreateElement("assembly", null);
            var aname = ass.GetName();
            AddAttribute(nassembly, "name", aname.Name);
            AddAttribute(nassembly, "version", aname.Version.ToString());
            parent.AppendChild(nassembly);
            //2008-08-18 tsv (stodyshev@gmail.com) - enabled reflection only context
            //AttributeData.OutputAttributes(document, nassembly, ass.GetCustomAttributes(false));
            AttributeData.OutputAttributes(document, nassembly, ass);
            var types = ass.GetExportedTypes();
            if (types == null || types.Length == 0)
                return;

            Array.Sort(types, TypeComparer.Default);

            XmlNode nss = document.CreateElement("namespaces", null);
            nassembly.AppendChild(nss);

            string currentNS = "$%&$&";
            XmlNode ns = null;
            XmlNode classes = null;
            foreach (var t in types)
            {
                if (t.Namespace == null || t.Namespace == "")
                    continue;

                if (t.IsNotPublic)
                    continue;

                if (t.IsNestedPublic || t.IsNestedAssembly || t.IsNestedFamANDAssem ||
                    t.IsNestedFamORAssem || t.IsNestedPrivate)
                    continue;

                if (t.DeclaringType != null)
                    continue; // enforce !nested

                if (t.Namespace != currentNS)
                {
                    currentNS = t.Namespace;
                    ns = document.CreateElement("namespace", null);
                    AddAttribute(ns, "name", currentNS);
                    nss.AppendChild(ns);
                    classes = document.CreateElement("classes", null);
                    ns.AppendChild(classes);
                }

                var bd = new TypeData(document, classes, t);
                bd.DoOutput();
            }
        }
    }

    abstract class MemberData : BaseData
    {
        readonly MemberInfo[] members;

        public MemberData(XmlDocument document, XmlNode parent, MemberInfo[] members)
            : base(document, parent)
        {
            this.members = members;
        }

        public override void DoOutput()
        {
            XmlNode mclass = document.CreateElement(ParentTag, null);
            parent.AppendChild(mclass);

            foreach (var member in members)
            {
                XmlNode mnode = document.CreateElement(Tag, null);
                mclass.AppendChild(mnode);
                AddAttribute(mnode, "name", GetName(member));
                if (!NoMemberAttributes)
                    AddAttribute(mnode, "attrib", GetMemberAttributes(member));

                //2008-08-18 tsv (stodyshev@gmail.com) - enabled reflection only context
                //AttributeData.OutputAttributes(document, mnode, member.GetCustomAttributes(false));
                AttributeData.OutputAttributes(document, mnode, member);

                AddExtraData(mnode, member);
            }
        }

        protected virtual void AddExtraData(XmlNode p, MemberInfo member)
        {
        }

        protected virtual string GetName(MemberInfo member)
        {
            return "NoNAME";
        }

        protected virtual string GetMemberAttributes(MemberInfo member)
        {
            return null;
        }

        public virtual bool NoMemberAttributes
        {
            get { return false; }
            set { }
        }

        public virtual string ParentTag
        {
            get { return "NoPARENTTAG"; }
        }

        public virtual string Tag
        {
            get { return "NoTAG"; }
        }
    }

    class TypeData : MemberData
    {
        readonly Type type;
        const BindingFlags flags = BindingFlags.Public | BindingFlags.Static |
                        BindingFlags.Instance | BindingFlags.DeclaredOnly |
                        BindingFlags.NonPublic;

        public TypeData(XmlDocument document, XmlNode parent, Type type)
            : base(document, parent, null)
        {
            this.type = type;
        }

        public override void DoOutput()
        {
            if (document == null)
                throw new InvalidOperationException("Document not set");

            XmlNode nclass = document.CreateElement("class", null);
            AddAttribute(nclass, "name", type.Name);
            string classType = GetClassType(type);
            AddAttribute(nclass, "type", classType);

            if (type.BaseType != null)
                AddAttribute(nclass, "base", type.BaseType.ToString());

            if (type.IsSealed)
                AddAttribute(nclass, "sealed", "true");

            if (type.IsAbstract)
                AddAttribute(nclass, "abstract", "true");

            if (type.IsSerializable)
                AddAttribute(nclass, "serializable", "true");

            string charSet = GetCharSet(type);
            AddAttribute(nclass, "charset", charSet);

            string layout = GetLayout(type);
            if (layout != null)
                AddAttribute(nclass, "layout", layout);

            parent.AppendChild(nclass);

            //2008-08-18 tsv (stodyshev@gmail.com) - enabled reflection only context
            //AttributeData.OutputAttributes(document, nclass, type.GetCustomAttributes(false));
            AttributeData.OutputAttributes(document, nclass, type);

            var interfaces = type.GetInterfaces();
            if (interfaces != null && interfaces.Length > 0)
            {
                XmlNode ifaces = document.CreateElement("interfaces", null);
                nclass.AppendChild(ifaces);
                foreach (var t in interfaces)
                {
                    if (!t.IsPublic)
                    {
                        // we're only interested in public interfaces
                        continue;
                    }
                    XmlNode iface = document.CreateElement("interface", null);
                    AddAttribute(iface, "name", t.ToString());
                    ifaces.AppendChild(iface);
                }
            }

#if NET_2_0
			// Generic constraints
			Type [] gargs = type.GetGenericArguments ();
			XmlElement ngeneric = (gargs.Length == 0) ? null :
				document.CreateElement ("generic-type-constraints");
			foreach (Type garg in gargs) {
				Type [] csts = garg.GetGenericParameterConstraints ();
				if (csts.Length == 0 || csts [0] == typeof (object))
					continue;
				XmlElement el = document.CreateElement ("generic-type-constraint");
				el.SetAttribute ("name", garg.ToString ());
				el.SetAttribute ("generic-attribute",
					garg.GenericParameterAttributes.ToString ());
				ngeneric.AppendChild (el);
				foreach (Type ct in csts) {
					XmlElement cel = document.CreateElement ("type");
					cel.AppendChild (document.CreateTextNode (ct.FullName));
					el.AppendChild (cel);
				}
			}
			if (ngeneric != null && ngeneric.FirstChild != null)
				nclass.AppendChild (ngeneric);
#endif

            var members = new ArrayList();

            var fields = GetFields(type);
            if (fields.Length > 0)
            {
                Array.Sort(fields, MemberInfoComparer.Default);
                var fd = new FieldData(document, nclass, fields);
                // Special case for enum fields
                if (classType == "enum")
                {
                    string etype = fields[0].GetType().ToString();
                    AddAttribute(nclass, "enumtype", etype);
                }
                members.Add(fd);
            }

            var ctors = GetConstructors(type);
            if (ctors.Length > 0)
            {
                Array.Sort(ctors, MemberInfoComparer.Default);
                members.Add(new ConstructorData(document, nclass, ctors));
            }

            var properties = GetProperties(type);
            if (properties.Length > 0)
            {
                Array.Sort(properties, MemberInfoComparer.Default);
                members.Add(new PropertyData(document, nclass, properties));
            }

            var events = GetEvents(type);
            if (events.Length > 0)
            {
                Array.Sort(events, MemberInfoComparer.Default);
                members.Add(new EventData(document, nclass, events));
            }

            var methods = GetMethods(type);
            if (methods.Length > 0)
            {
                Array.Sort(methods, MemberInfoComparer.Default);
                members.Add(new MethodData(document, nclass, methods));
            }

            foreach (MemberData md in members)
                md.DoOutput();

            var nested = type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic);
            if (nested != null && nested.Length > 0)
            {
                bool add_nested = false;
                foreach (var t in nested)
                {
                    if (t.IsNestedPublic || t.IsNestedFamily || t.IsNestedFamORAssem)
                    {
                        add_nested = true;
                        break;
                    }
                }

                if (add_nested)
                {
                    XmlNode classes = document.CreateElement("classes", null);
                    nclass.AppendChild(classes);
                    foreach (var t in nested)
                    {
                        if (t.IsNestedPublic || t.IsNestedFamily || t.IsNestedFamORAssem)
                        {
                            var td = new TypeData(document, classes, t);
                            td.DoOutput();
                        }
                    }
                }
            }
        }

        protected override string GetMemberAttributes(MemberInfo member)
        {
            if (member != type)
                throw new InvalidOperationException("odd");

            return ((int)type.Attributes).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// The includeExplicit bool was introduced to ignore explicit interface
        /// implementations for properties and events.
        ///
        /// When a property / event implements an explicit interface, then
        /// the MS class libraries only have the accessors; the corresponding
        /// property/event is not emitted.
        /// 
        /// For an example of a property, see System.Windows.Forms.TreeNodeCollection's
        /// implementation of ICollection.IsSynchronized.
        /// 
        /// For an example of an event, see System.Windows.Forms.PropertyGrid's
        /// implementation of IComPropertyBrowser.ComComponentNameChanged.
        /// 
        /// Both our C# compiler and that of MS always emit the property/event,
        /// so MS must be using another language and/or compiler for (part of)
        /// the BCL.
        ///
        /// To avoid numerous extra reports in the Class Status Pages, we'll
        /// ignore properties/events that are explictly implemented.
        /// 
        /// The getters/setters themselves are included in the generated API info
        /// though.
        /// </summary>
        public static bool MustDocumentMethod(MethodBase method, bool includeExplicit, out bool explicitImpl)
        {
            explicitImpl = false;

            if (method.IsPublic || method.IsFamily || method.IsFamilyOrAssembly)
                return true;

            if (!includeExplicit || method.DeclaringType.IsInterface)
                return false;

            var interfaces = method.DeclaringType.GetInterfaces();
            foreach (var interfaceType in interfaces)
            {
                if (!IsPublicType(interfaceType))
                    continue;

                var map = method.DeclaringType.GetInterfaceMap(
                    interfaceType);
                if (Array.IndexOf(map.TargetMethods, method) != -1)
                {
                    explicitImpl = true;
                    return true;
                }
            }

            return false;
        }

        public static bool MustDocumentMethod(MethodBase method, bool includeExplicit)
        {
            bool explicitImpl;
            return MustDocumentMethod(method, includeExplicit, out explicitImpl);
        }

        static bool IsPublicType(Type t)
        {
            if (t.IsPublic)
                return true;
            else if (!t.IsNestedPublic)
                return false;

            while (t.DeclaringType != null)
            {
                t = t.DeclaringType;

                if (!t.IsPublic && !(t.IsNestedPublic || t.IsNestedFamily || t.IsNestedFamORAssem))
                    return false;
            }

            return true;
        }

        static string GetClassType(Type t)
        {
            if (t.IsEnum)
                return "enum";

            if (t.IsValueType)
                return "struct";

            if (t.IsInterface)
                return "interface";

            if (typeof(Delegate).IsAssignableFrom(t))
                return "delegate";

            return "class";
        }

        private static string GetCharSet(Type type)
        {
            if (type.IsAnsiClass)
                return CharSet.Ansi.ToString(CultureInfo.InvariantCulture);

            if (type.IsAutoClass)
                return CharSet.Auto.ToString(CultureInfo.InvariantCulture);

            if (type.IsUnicodeClass)
                return CharSet.Unicode.ToString(CultureInfo.InvariantCulture);

            return CharSet.None.ToString(CultureInfo.InvariantCulture);
        }

        private static string GetLayout(Type type)
        {
            if (type.IsAutoLayout)
                return LayoutKind.Auto.ToString(CultureInfo.InvariantCulture);

            if (type.IsExplicitLayout)
                return LayoutKind.Explicit.ToString(CultureInfo.InvariantCulture);

            if (type.IsLayoutSequential)
                return LayoutKind.Sequential.ToString(CultureInfo.InvariantCulture);

            return null;
        }

        private FieldInfo[] GetFields(Type type)
        {
            var list = new ArrayList();

            var fields = type.GetFields(flags);
            foreach (var field in fields)
            {
                if (field.IsSpecialName)
                    continue;

                // we're only interested in public or protected members
                if (!field.IsPublic && !field.IsFamily && !field.IsFamilyOrAssembly)
                    continue;

                list.Add(field);
            }

            return (FieldInfo[])list.ToArray(typeof(FieldInfo));
        }

        internal static PropertyInfo[] GetProperties(Type type)
        {
            var list = new ArrayList();

            var properties = type.GetProperties(flags);
            foreach (var property in properties)
            {
                MethodInfo getMethod = null;
                MethodInfo setMethod = null;

                if (property.CanRead)
                {
                    try { getMethod = property.GetGetMethod(true); }
                    catch (SecurityException) { }
                }
                if (property.CanWrite)
                {
                    try { setMethod = property.GetSetMethod(true); }
                    catch (SecurityException) { }
                }

                bool hasGetter = (getMethod != null) && MustDocumentMethod(getMethod, false);
                bool hasSetter = (setMethod != null) && MustDocumentMethod(setMethod, false);

                // if neither the getter or setter should be documented, then
                // skip the property
                if (!hasGetter && !hasSetter)
                {
                    continue;
                }

                list.Add(property);
            }

            return (PropertyInfo[])list.ToArray(typeof(PropertyInfo));
        }

        private MethodInfo[] GetMethods(Type type)
        {
            var list = new ArrayList();

            var methods = type.GetMethods(flags);
            foreach (var method in methods)
            {
                bool explicitImpl;

                if (!MustDocumentMethod(method, true, out explicitImpl))
                    continue;

                // avoid writing (property/event) accessors twice
                if (!explicitImpl && method.IsSpecialName && !method.Name.StartsWith("op_"))
                    continue;

                list.Add(method);
            }

            return (MethodInfo[])list.ToArray(typeof(MethodInfo));
        }

        private ConstructorInfo[] GetConstructors(Type type)
        {
            var list = new ArrayList();

            var ctors = type.GetConstructors(flags);
            foreach (var constructor in ctors)
            {
                // we're only interested in public or protected members
                if (!constructor.IsPublic && !constructor.IsFamily && !constructor.IsFamilyOrAssembly)
                    continue;

                list.Add(constructor);
            }

            return (ConstructorInfo[])list.ToArray(typeof(ConstructorInfo));
        }

        private EventInfo[] GetEvents(Type type)
        {
            var list = new ArrayList();

            var events = type.GetEvents(flags);
            foreach (var eventInfo in events)
            {
                var addMethod = eventInfo.GetAddMethod(true);

                if (addMethod == null || !MustDocumentMethod(addMethod, false))
                    continue;

                list.Add(eventInfo);
            }

            return (EventInfo[])list.ToArray(typeof(EventInfo));
        }
    }

    class FieldData : MemberData
    {
        public FieldData(XmlDocument document, XmlNode parent, FieldInfo[] members)
            : base(document, parent, members)
        {
        }

        protected override string GetName(MemberInfo member)
        {
            var field = (FieldInfo)member;
            return field.Name;
        }

        protected override string GetMemberAttributes(MemberInfo member)
        {
            var field = (FieldInfo)member;
            return ((int)field.Attributes).ToString(CultureInfo.InvariantCulture);
        }

        protected override void AddExtraData(XmlNode p, MemberInfo member)
        {
            base.AddExtraData(p, member);
            var field = (FieldInfo)member;
            AddAttribute(p, "fieldtype", field.FieldType.ToString());

            if (field.IsLiteral)
            {
                var value = field.GetValue(null);
                string stringValue = null;
                if (value is Enum)
                {
                    // FIXME: when Mono bug #60090 has been
                    // fixed, we should just be able to use
                    // Convert.ToString
                    stringValue = ((Enum)value).ToString("D", CultureInfo.InvariantCulture);
                }
                else
                {
                    stringValue = Convert.ToString(value, CultureInfo.InvariantCulture);
                }

                if (stringValue != null)
                    AddAttribute(p, "value", stringValue);
            }
        }

        public override string ParentTag
        {
            get { return "fields"; }
        }

        public override string Tag
        {
            get { return "field"; }
        }
    }

    class PropertyData : MemberData
    {
        public PropertyData(XmlDocument document, XmlNode parent, PropertyInfo[] members)
            : base(document, parent, members)
        {
        }

        protected override string GetName(MemberInfo member)
        {
            var prop = (PropertyInfo)member;
            return prop.Name;
        }

        protected override void AddExtraData(XmlNode p, MemberInfo member)
        {
            base.AddExtraData(p, member);
            var prop = (PropertyInfo)member;
            AddAttribute(p, "ptype", prop.PropertyType.ToString());
            var _get = prop.GetGetMethod(true);
            var _set = prop.GetSetMethod(true);
            bool haveGet = (_get != null && TypeData.MustDocumentMethod(_get, false));
            bool haveSet = (_set != null && TypeData.MustDocumentMethod(_set, false));
            MethodInfo[] methods;

            if (haveGet && haveSet)
            {
                methods = new[] { _get, _set };
            }
            else if (haveGet)
            {
                methods = new[] { _get };
            }
            else if (haveSet)
            {
                methods = new[] { _set };
            }
            else
            {
                //odd
                return;
            }

            string parms = Parameters.GetSignature(methods[0].GetParameters());
            AddAttribute(p, "params", parms);

            var data = new MethodData(document, p, methods);
            //data.NoMemberAttributes = true;
            data.DoOutput();
        }

        protected override string GetMemberAttributes(MemberInfo member)
        {
            var prop = (PropertyInfo)member;
            return ((int)prop.Attributes & (0xFFFFFFFF ^ (int)PropertyAttributes.ReservedMask)).ToString(CultureInfo.InvariantCulture);
        }

        public override string ParentTag
        {
            get { return "properties"; }
        }

        public override string Tag
        {
            get { return "property"; }
        }
    }

    class EventData : MemberData
    {
        public EventData(XmlDocument document, XmlNode parent, EventInfo[] members)
            : base(document, parent, members)
        {
        }

        protected override string GetName(MemberInfo member)
        {
            var evt = (EventInfo)member;
            return evt.Name;
        }

        protected override string GetMemberAttributes(MemberInfo member)
        {
            var evt = (EventInfo)member;
            return ((int)evt.Attributes).ToString(CultureInfo.InvariantCulture);
        }

        protected override void AddExtraData(XmlNode p, MemberInfo member)
        {
            base.AddExtraData(p, member);
            var evt = (EventInfo)member;
            AddAttribute(p, "eventtype", evt.EventHandlerType.ToString());

            var _add = evt.GetAddMethod(true);
            var _remove = evt.GetRemoveMethod(true);
            bool haveAdd = (_add != null && TypeData.MustDocumentMethod(_add, true));
            bool haveRemove = (_remove != null && TypeData.MustDocumentMethod(_remove, true));
            MethodInfo[] methods;

            if (haveAdd && haveRemove)
            {
                methods = new[] { _add, _remove };
            }
            else if (haveAdd)
            {
                methods = new[] { _add };
            }
            else if (haveRemove)
            {
                methods = new[] { _remove };
            }
            else
            {
                return;
            }

            var data = new MethodData(document, p, methods);
            data.DoOutput();
        }

        public override string ParentTag
        {
            get { return "events"; }
        }

        public override string Tag
        {
            get { return "event"; }
        }
    }

    class MethodData : MemberData
    {
        bool noAtts;

        public MethodData(XmlDocument document, XmlNode parent, MethodBase[] members)
            : base(document, parent, members)
        {
        }

        protected override string GetName(MemberInfo member)
        {
            var method = (MethodBase)member;
            string name = method.Name;
            string parms = Parameters.GetSignature(method.GetParameters());
#if NET_2_0
			MethodInfo mi = method as MethodInfo;
			Type [] genArgs = mi == null ? Type.EmptyTypes :
				mi.GetGenericArguments ();
			if (genArgs.Length > 0) {
				string [] genArgNames = new string [genArgs.Length];
				for (int i = 0; i < genArgs.Length; i++) {
					genArgNames [i] = genArgs [i].Name;
					string genArgCsts = String.Empty;
					Type [] gcs = genArgs [i].GetGenericParameterConstraints ();
					if (gcs.Length > 0) {
						string [] gcNames = new string [gcs.Length];
						for (int g = 0; g < gcs.Length; g++)
							gcNames [g] = gcs [g].FullName;
						genArgCsts = String.Concat (
							"(",
							string.Join (", ", gcNames),
							") ",
							genArgNames [i]);
					}
					else
						genArgCsts = genArgNames [i];
					if ((genArgs [i].GenericParameterAttributes & GenericParameterAttributes.ReferenceTypeConstraint) != 0)
						genArgCsts = "class " + genArgCsts;
					else if ((genArgs [i].GenericParameterAttributes & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0)
						genArgCsts = "struct " + genArgCsts;
					genArgNames [i] = genArgCsts;
				}
				return String.Format ("{0}<{2}>({1})",
					name,
					parms,
					string.Join (",", genArgNames));
			}
#endif
            return String.Format("{0}({1})", name, parms);
        }

        protected override string GetMemberAttributes(MemberInfo member)
        {
            var method = (MethodBase)member;
            return ((int)(method.Attributes & ~MethodAttributes.ReservedMask)).ToString(CultureInfo.InvariantCulture);
        }

        protected override void AddExtraData(XmlNode p, MemberInfo member)
        {
            base.AddExtraData(p, member);

            var parms = new ParameterData(document, p,
                ((MethodBase)member).GetParameters());
            parms.DoOutput();

            if (!(member is MethodBase))
                return;

            var mbase = (MethodBase)member;

            if (mbase.IsAbstract)
                AddAttribute(p, "abstract", "true");
            if (mbase.IsVirtual)
                AddAttribute(p, "virtual", "true");
            if (mbase.IsFinal)
                AddAttribute(p, "final", "true");
            if (mbase.IsStatic)
                AddAttribute(p, "static", "true");

            if (!(member is MethodInfo))
                return;

            var method = (MethodInfo)member;
            AddAttribute(p, "returntype", method.ReturnType.ToString());

            //2008-08-18 tsv (stodyshev@gmail.com) - enabled reflection only context
            if (!method.DeclaringType.Assembly.ReflectionOnly)
                AttributeData.OutputAttributes(document, p, method.ReturnTypeCustomAttributes.GetCustomAttributes(false));

#if NET_2_0
			// Generic constraints
			Type [] gargs = method.GetGenericArguments ();
			XmlElement ngeneric = (gargs.Length == 0) ? null :
				document.CreateElement ("generic-method-constraints");
			foreach (Type garg in gargs) {
				Type [] csts = garg.GetGenericParameterConstraints ();
				if (csts.Length == 0 || csts [0] == typeof (object))
					continue;
				XmlElement el = document.CreateElement ("generic-method-constraint");
				el.SetAttribute ("name", garg.ToString ());
				el.SetAttribute ("generic-attribute",
					garg.GenericParameterAttributes.ToString ());
				ngeneric.AppendChild (el);
				foreach (Type ct in csts) {
					XmlElement cel = document.CreateElement ("type");
					cel.AppendChild (document.CreateTextNode (ct.FullName));
					el.AppendChild (cel);
				}
			}
			if (ngeneric != null && ngeneric.FirstChild != null)
				p.AppendChild (ngeneric);
#endif

        }

        public override bool NoMemberAttributes
        {
            get { return noAtts; }
            set { noAtts = value; }
        }

        public override string ParentTag
        {
            get { return "methods"; }
        }

        public override string Tag
        {
            get { return "method"; }
        }
    }

    class ConstructorData : MethodData
    {
        public ConstructorData(XmlDocument document, XmlNode parent, ConstructorInfo[] members)
            : base(document, parent, members)
        {
        }

        public override string ParentTag
        {
            get { return "constructors"; }
        }

        public override string Tag
        {
            get { return "constructor"; }
        }
    }

    class ParameterData : BaseData
    {
        private readonly ParameterInfo[] parameters;

        public ParameterData(XmlDocument document, XmlNode parent, ParameterInfo[] parameters)
            : base(document, parent)
        {
            this.parameters = parameters;
        }

        public override void DoOutput()
        {
            XmlNode parametersNode = document.CreateElement("parameters", null);
            parent.AppendChild(parametersNode);

            foreach (var parameter in parameters)
            {
                XmlNode paramNode = document.CreateElement("parameter", null);
                parametersNode.AppendChild(paramNode);
                AddAttribute(paramNode, "name", parameter.Name);
                AddAttribute(paramNode, "position", parameter.Position.ToString(CultureInfo.InvariantCulture));
                AddAttribute(paramNode, "attrib", ((int)parameter.Attributes).ToString());

                string direction = "in";

                if (parameter.ParameterType.IsByRef)
                {
                    direction = parameter.IsOut ? "out" : "ref";
                }

                var t = parameter.ParameterType;
                AddAttribute(paramNode, "type", t.ToString());

                if (parameter.IsOptional)
                {
                    AddAttribute(paramNode, "optional", "true");
                    if (parameter.DefaultValue != DBNull.Value)
                        AddAttribute(paramNode, "defaultValue", (parameter.DefaultValue == null) ? "NULL" : parameter.DefaultValue.ToString());
                }

                if (direction != "in")
                    AddAttribute(paramNode, "direction", direction);

                //2008-08-18 tsv (stodyshev@gmail.com) - enabled reflection only context
                //AttributeData.OutputAttributes(document, paramNode, parameter.GetCustomAttributes(false));
                AttributeData.OutputAttributes(document, paramNode, parameter);
            }
        }
    }

    class AttributeData : BaseData
    {
        readonly object[] atts;
        readonly object provider;

        AttributeData(XmlDocument doc, XmlNode parent, object[] attributes)
            : base(doc, parent)
        {
            atts = attributes;
        }

        AttributeData(XmlDocument doc, XmlNode parent, object provider)
            : base(doc, parent)
        {
            this.provider = provider;
        }

        public override void DoOutput()
        {
            if (document == null)
                throw new InvalidOperationException("Document not set");

            if (atts == null || atts.Length == 0)
                return;

            var natts = parent.SelectSingleNode("attributes");
            if (natts == null)
            {
                natts = document.CreateElement("attributes", null);
                parent.AppendChild(natts);
            }

            if (provider != null)
            {
                //TODO: emit xml for custom attributes
                //var attrs = GetCustomAttributes(provider);
                //if (attrs == null) return;
                //for (int i = 0; i < attrs.Count; ++i)
                //{
                //    var attr = attrs[i];
                //    Type t = attr.Constructor.DeclaringType;
                //}
            }
            else
            {
                OutputAtts(natts);   
            }
        }

        private void OutputAtts(XmlNode natts)
        {
            for (int i = 0; i < atts.Length; ++i)
            {
                var t = atts[i].GetType();

                if (MustSkipAttrType(t))
                    continue;

                XmlNode node = document.CreateElement("attribute");
                AddAttribute(node, "name", t.ToString());

                XmlNode properties = null;
                foreach (var pi in TypeData.GetProperties(t))
                {
                    if (pi.Name == "TypeId")
                        continue;

                    if (properties == null)
                    {
                        properties = node.AppendChild(document.CreateElement("properties"));
                    }

                    try
                    {
                        var o = pi.GetValue(atts[i], null);

                        var n = properties.AppendChild(document.CreateElement("property"));
                        AddAttribute(n, "name", pi.Name);

                        if (o == null)
                        {
                            AddAttribute(n, "null", "true");
                            continue;
                        }

                        string value = o.ToString();
                        if (t == typeof(GuidAttribute))
                            value = value.ToUpper();

                        AddAttribute(n, "value", value);
                    }
                    catch (TargetInvocationException)
                    {
                        continue;
                    }
                }

                natts.AppendChild(node);
            }
        }

        private static bool MustSkipAttrType(Type t)
        {
            if (!t.IsPublic && !IsMonoTODOAttribute(t.Name))
                return true;

            // we ignore attributes that inherit from SecurityAttribute on purpose as they:
            // * aren't part of GetCustomAttributes in Fx 1.0/1.1;
            // * are encoded differently and in a different metadata table; and
            // * won't ever exactly match MS implementation (from a syntax pov)
            if (t.IsSubclassOf(typeof(SecurityAttribute)))
                return true;

            return false;
        }

        public static void OutputAttributes(XmlDocument doc, XmlNode parent, object[] attributes)
        {
            var ad = new AttributeData(doc, parent, attributes);
            ad.DoOutput();
        }

        public static void OutputAttributes(XmlDocument doc, XmlNode parent, object provider)
        {
            var ad = new AttributeData(doc, parent, provider);
            ad.DoOutput();
        }

        private static bool MustDocumentAttribute(Type attributeType)
        {
            // only document MonoTODOAttribute and public attributes
            return attributeType.IsPublic || IsMonoTODOAttribute(attributeType.Name);
        }

        //2008-08-18 tsv (stodyshev@gmail.com) - enabled reflection only context
        public static IList<CustomAttributeData> GetCustomAttributes(object provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            var ass = provider as Assembly;
            if (ass != null)
                return CustomAttributeData.GetCustomAttributes(ass);

            var mod = provider as Module;
            if (mod != null)
                return CustomAttributeData.GetCustomAttributes(mod);

            var mi = provider as MemberInfo;
            if (mi != null)
                return CustomAttributeData.GetCustomAttributes(mi);

            var pi = provider as ParameterInfo;
            if (pi != null)
                return CustomAttributeData.GetCustomAttributes(pi);

            throw new NotImplementedException();
        }
    }

    class Parameters
    {
        private Parameters() { }

        public static string GetSignature(ParameterInfo[] infos)
        {
            if (infos == null || infos.Length == 0)
                return "";

            var sb = new StringBuilder();
            foreach (var info in infos)
            {
                string modifier;
                if (info.IsIn)
                    modifier = "in ";
                else if (info.IsRetval)
                    modifier = "ref ";
                else if (info.IsOut)
                    modifier = "out ";
                else
                    modifier = "";

                string type_name = info.ParameterType.ToString().Replace('<', '[').Replace('>', ']');
                sb.AppendFormat("{0}{1}, ", modifier, type_name);
            }

            sb.Length -= 2; // remove ", "
            return sb.ToString();
        }

    }

    class TypeComparer : IComparer
    {
        public static TypeComparer Default = new TypeComparer();

        public int Compare(object a, object b)
        {
            var ta = (Type)a;
            var tb = (Type)b;
            int result = String.Compare(ta.Namespace, tb.Namespace);
            if (result != 0)
                return result;

            return String.Compare(ta.Name, tb.Name);
        }
    }

    class MemberInfoComparer : IComparer
    {
        public static MemberInfoComparer Default = new MemberInfoComparer();

        public int Compare(object a, object b)
        {
            var ma = (MemberInfo)a;
            var mb = (MemberInfo)b;
            return String.Compare(ma.Name, mb.Name);
        }
    }

    class MethodBaseComparer : IComparer
    {
        public static MethodBaseComparer Default = new MethodBaseComparer();

        public int Compare(object a, object b)
        {
            var ma = (MethodBase)a;
            var mb = (MethodBase)b;
            int res = String.Compare(ma.Name, mb.Name);
            if (res != 0)
                return res;

            var pia = ma.GetParameters();
            var pib = mb.GetParameters();
            if (pia.Length != pib.Length)
                return pia.Length - pib.Length;

            string siga = Parameters.GetSignature(pia);
            string sigb = Parameters.GetSignature(pib);
            return String.Compare(siga, sigb);
        }
    }
}
