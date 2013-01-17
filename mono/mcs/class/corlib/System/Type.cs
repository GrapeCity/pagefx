//CHANGED

//
// System.Type.cs
//
// Author:
//   Miguel de Icaza (miguel@ximian.com)
//
// (C) Ximian, Inc.  http://www.ximian.com
//

//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Diagnostics;
using System.Reflection;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Globalization;
using Native;

namespace System
{
    [Serializable]
    public class Type : MemberInfo, IReflect
    {
        #region Fields
        internal int index;
        internal string ns;
		internal string name;
        internal object nsobj;
        
        int flags;
        internal int kind;

        int baseType = -1;
        internal int elemType = -1;
        internal int utype = -1;
        internal int rank = -1;

        FieldInfo[] m_myfields;
		internal Function m_myfieldsInit;
        
        int[] ifaces;
        internal Function m_box;
        internal Function m_unbox;
		internal Function m_copy;
		internal Function m_create;
        #endregion

        #region Const
        internal const int KIND_CLASS = 0;
        internal const int KIND_INTERFACE = 1;
        internal const int KIND_STRUCT = 2;
        internal const int KIND_ENUM = 3;
        internal const int KIND_GENERIC_PARAM = 4;
        internal const int KIND_DELEGATE = 5;
        internal const int KIND_ARRAY = 6;
        internal const int KIND_REF = 7;
        internal const int KIND_PTR = 8;
        internal const int KIND_TYPE_CODE = 100;
        #endregion

        internal EnumInfo EnumInfo
        {
            get
            {
                if (kind != KIND_ENUM)
                    return null;
                if (enumInfo == null)
					enumInfo = (EnumInfo)m_enumInfoInit.call(null);
                return enumInfo;
            }
        }
        EnumInfo enumInfo;
        Function m_enumInfoInit;

        internal FieldInfo[] MyFields
        {
            get 
            {
                if (m_myfields == null)
                {
                    if (m_myfieldsInit != null)
						m_myfields = (FieldInfo[])m_myfieldsInit.call(null);
                    else
                        m_myfields = new FieldInfo[0];
                }
                return m_myfields;
            }
        }

        public override string Name
        {
            get { return name; }
        }

        public virtual string Namespace
        {
            get { return ns; }
        }

        /// <summary>
        ///    The full name of the type including its namespace
        /// </summary>
        public virtual string FullName
        {
            get
            {
                if (ns == null || ns.Length == 0)
                    return name;
                return ns + Delimiter + name;
            }
        }

        internal object Class
        {
            get
            {
                object global = avm.Findpropstrict(nsobj, name);
                object klass = avm.GetProperty(global, nsobj, name);
                return klass;
            }
        }

        internal int ValueTypeKind
        {
            get
            {
                //if (kind == KIND_ENUM)
                if (utype >= 0)
                    return UnderlyingSystemType.kind;
                return kind;
            }
        }

        internal object Unbox(object obj)
        {
            if (m_unbox != null)
				return m_unbox.call(null, obj);
            if (m_copy != null)
				return m_copy.call(null, obj);
            return obj;
        }

        public object CreateInstance()
        {
            if (m_create != null)
				return m_create.call(null);
            return CreateInstanceDefault();
        }

        public object CreateInstanceDefault()
        {
            object global = avm.Findpropstrict(nsobj, name);
            return avm.Construct(global, nsobj, name);
        }

        internal object GetFieldValue(object fns, string fname)
        {
            object global = avm.Findpropstrict(nsobj, name);
            object klass = avm.GetProperty(global, nsobj, name);
            return avm.GetProperty(klass, fns, fname);
        }

        internal void SetFieldValue(object fns, string fname, object value)
        {
            object global = avm.Findpropstrict(nsobj, name);
            object klass = avm.GetProperty(global, nsobj, name);
            avm.SetProperty(klass, fns, fname, value);
        }

	    internal object[] GetFieldValues(object obj)
	    {
		    FieldInfo[] fields = MyFields;
		    if (fields == null) return null;
		    int n = fields.Length;
		    object[] arr = new object[n];
		    for (int i = 0; i < n; ++i)
			    arr[i] = fields[i].GetValue(obj);
		    return arr;
	    }

	    internal void CopyFields(object from, object to)
        {
            FieldInfo[] arr = MyFields;
            if (arr == null) return;
            int n = arr.Length;
            for (int i = 0; i < n; ++i)
            {
                FieldInfo f = arr[i];
                object val = f.GetValue(from);
                f.SetValue(to, val);
            }
        }
        
        internal object GetZero()
        {
            TypeCode typeCode = GetTypeCode(this);
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return false;

                case TypeCode.Byte:
                    return (byte)0;

                case TypeCode.Char:
                    return (char)0;

                case TypeCode.Double:
                    return 0.0;

                case TypeCode.Int16:
                    return (short)0;

                case TypeCode.Int32:
                    return 0;

                case TypeCode.SByte:
                    return (sbyte)0;

                case TypeCode.Single:
                    return 0.0;

                case TypeCode.UInt16:
                    return (ushort)0;

                case TypeCode.UInt32:
                    return 0u;

                default:
                    return null;
            }
        }

        public static readonly char Delimiter = '.';
        public static readonly Type[] EmptyTypes = { };
        internal static readonly MemberFilter FilterAttribute = FilterAttribute_impl;
        internal static readonly MemberFilter FilterName = FilterName_impl;
        public static readonly MemberFilter FilterNameIgnoreCase = FilterNameIgnoreCase_impl;
        public static readonly object Missing;

        internal const BindingFlags DefaultBindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;

        /* implementation of the delegates for MemberFilter */
        static bool FilterName_impl(MemberInfo m, object filterCriteria)
        {
            string name = (string)filterCriteria;
            if (name == null || name.Length == 0)
                return false; // because m.Name cannot be null or empty

            if (name[name.Length - 1] == '*')
                return string.Compare(name, 0, m.Name, 0, name.Length - 1, false, CultureInfo.InvariantCulture) == 0;

            return name.Equals(m.Name);
        }

        static bool FilterNameIgnoreCase_impl(MemberInfo m, object filterCriteria)
        {
            string name = (string)filterCriteria;
            if (name == null || name.Length == 0)
                return false; // because m.Name cannot be null or empty

            if (name[name.Length - 1] == '*')
                return string.Compare(name, 0, m.Name, 0, name.Length - 1, true, CultureInfo.InvariantCulture) == 0;

            return String.Compare(name, m.Name, true, CultureInfo.InvariantCulture) == 0;
        }

        static bool FilterAttribute_impl(MemberInfo m, object filterCriteria)
        {
            int flags = ((IConvertible)filterCriteria).ToInt32(null);
            if (m is MethodInfo)
                return ((int)((MethodInfo)m).Attributes & flags) != 0;
            if (m is FieldInfo)
                return ((int)((FieldInfo)m).Attributes & flags) != 0;
            if (m is PropertyInfo)
                return ((int)((PropertyInfo)m).Attributes & flags) != 0;
            if (m is EventInfo)
                return ((int)((EventInfo)m).Attributes & flags) != 0;
            return false;
        }

        /// <summary>
        ///   The assembly where the type is defined.
        /// </summary>
        public virtual Assembly Assembly
        {
            get { return Assembly.Instance; }
        }

        /// <summary>
        ///   Gets the fully qualified name for the type including the
        ///   assembly name where the type is defined.
        /// </summary>
        public virtual string AssemblyQualifiedName
        {
            get
            {
                return FullName + "," + Assembly.FullName;
            }
        }

        /// <summary>
        ///   Returns the Attributes associated with the type.
        /// </summary>
        public TypeAttributes Attributes
        {
            get
            {
                return GetAttributeFlagsImpl();
            }
        }

        /// <summary>
        ///   Returns the basetype for this type
        /// </summary>
        public virtual Type BaseType
        {
            get { return Assembly.GetType(baseType); }
        }

        /// <summary>
        ///   Returns the class that declares the member.
        /// </summary>
        public override Type DeclaringType
        {
            get
            {
                return null;
            }
        }

#if NOT_PFX
        /// <summary>
        ///
        /// </summary>
        public static Binder DefaultBinder
        {
            get
            {
                return Binder.DefaultBinder;
            }
        }
#endif

        public virtual Guid GUID
        {
            get { return Guid.Empty; }
        }

        public bool HasElementType
        {
            get
            {
                return elemType >= 0;
            }
        }

        protected virtual bool HasElementTypeImpl()
        {
            return HasElementType;
        }

        public bool IsAbstract
        {
            get
            {
                return (Attributes & TypeAttributes.Abstract) != 0;
            }
        }

        public bool IsAnsiClass
        {
            get
            {
                return (Attributes & TypeAttributes.StringFormatMask)
                == TypeAttributes.AnsiClass;
            }
        }

        public bool IsArray
        {
            get { return kind == KIND_ARRAY; }
        }

        protected virtual bool IsArrayImpl()
        {
            return IsArray;
        }

        internal static bool IsArrayImpl(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            return type.IsArray;
        }

        public bool IsAutoClass
        {
            get
            {
                return (Attributes & TypeAttributes.StringFormatMask) == TypeAttributes.AutoClass;
            }
        }

        public bool IsAutoLayout
        {
            get
            {
                return (Attributes & TypeAttributes.LayoutMask) == TypeAttributes.AutoLayout;
            }
        }

        public bool IsByRef
        {
            get { return kind == KIND_REF; }
        }

        protected virtual bool IsByRefImpl()
        {
            return IsByRef;
        }

        public bool IsClass
        {
            get
            {
                switch (kind)
                {
                    case KIND_CLASS:
                    case KIND_DELEGATE:
                    case KIND_ARRAY:
                    case KIND_TYPE_CODE + (int)TypeCode.String:
                        return true;
                }
                return false;
                //if (IsInterface) return false;
                //return !IsSubclassOf(typeof(ValueType));
            }
        }

        public bool IsCOMObject
        {
            get
            {
                return IsCOMObjectImpl();
            }
        }

        public bool IsEnum
        {
            get { return kind == KIND_ENUM; }
        }

        internal bool IsExplicitLayout
        {
            get
            {
                return (Attributes & TypeAttributes.LayoutMask) == TypeAttributes.ExplicitLayout;
            }
        }

        public bool IsImport
        {
            get
            {
                return (Attributes & TypeAttributes.Import) != 0;
            }
        }

        public bool IsInterface
        {
            get { return kind == KIND_INTERFACE; }
        }

        internal bool IsLayoutSequential
        {
            get
            {
                return (Attributes & TypeAttributes.LayoutMask) == TypeAttributes.SequentialLayout;
            }
        }

#if NOT_PFX
        public bool IsMarshalByRef
        {
            get
            {
                return IsMarshalByRefImpl();
            }
        }
#endif

        public bool IsNestedAssembly
        {
            get
            {
                return (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedAssembly;
            }
        }

        public bool IsNestedFamANDAssem
        {
            get
            {
                return (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamANDAssem;
            }
        }

        public bool IsNestedFamily
        {
            get
            {
                return (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamily;
            }
        }

        public bool IsNestedFamORAssem
        {
            get
            {
                return (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamORAssem;
            }
        }

        public bool IsNestedPrivate
        {
            get
            {
                return (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPrivate;
            }
        }

        public bool IsNestedPublic
        {
            get
            {
                return (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPublic;
            }
        }

        public bool IsNotPublic
        {
            get
            {
                return (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NotPublic;
            }
        }

        public bool IsPointer
        {
            get { return kind == KIND_PTR; }
        }

        protected virtual bool IsPointerImpl()
        {
            return IsPointer;
        }

        public bool IsPrimitive
        {
            get
            {
                //TypeCode tc = GetTypeCode(this);
                //return tc >= TypeCode.Boolean && tc <= TypeCode.Decimal;

                int k = kind;
                return k >= KIND_TYPE_CODE + (int)TypeCode.Boolean
                       && k <= KIND_TYPE_CODE + (int)TypeCode.Decimal;
            }
        }

        protected virtual bool IsPrimitiveImpl()
        {
            return IsPrimitive;
        }

        public bool IsPublic
        {
            get
            {
                return (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.Public;
            }
        }

        public bool IsSealed
        {
            get
            {
                return (Attributes & TypeAttributes.Sealed) != 0;
            }
        }

        internal bool IsSerializable
        {
            get
            {
                if ((Attributes & TypeAttributes.Serializable) != 0)
                    return true;

                // Enums and delegates are always serializable

                Type type = UnderlyingSystemType;
                if (type == null)
                    return false;

                // Fast check for system types
                //if (type.IsSystemType)
                //    return type_is_subtype_of(type, typeof(Enum), false) || type_is_subtype_of(type, typeof(Delegate), false);

                // User defined types depend on this behavior
                do
                {
                    if ((type == typeof(Enum)) || (type == typeof(Delegate)))
                        return true;

                    type = type.BaseType;
                } while (type != null);

                return false;
            }
        }

        public bool IsSpecialName
        {
            get
            {
                return (Attributes & TypeAttributes.SpecialName) != 0;
            }
        }

        public bool IsUnicodeClass
        {
            get
            {
                return (Attributes & TypeAttributes.StringFormatMask) == TypeAttributes.UnicodeClass;
            }
        }

        public bool IsValueType
        {
            get
            {
                if (IsPrimitive) return true;
                return kind == KIND_STRUCT;
            }
        }

        protected virtual bool IsValueTypeImpl()
        {
            return IsValueType;
        }

        public override MemberTypes MemberType
        {
            get { return MemberTypes.TypeInfo; }
        }

        public override Type ReflectedType
        {
            get { return DeclaringType; }
        }

		public virtual RuntimeTypeHandle TypeHandle 
        {
			get { throw new NotSupportedException(); }
		}

#if NOT_PFX
        public ConstructorInfo TypeInitializer
        {
            get
            {
                return GetConstructorImpl(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static,
                    null,
                    CallingConventions.Any,
                    EmptyTypes,
                    null);
            }
        }
#endif

        public virtual Type UnderlyingSystemType
        {
            get
            {
                if (utype >= 0)
                    return Assembly.GetType(utype);
                return this;
            }
        }

        public override bool Equals(object o)
        {
            if (o == null)
                return false;

            Type cmp = o as Type;
            if (cmp == null)
                return false;
            return Equals(cmp);
        }

        public bool Equals(Type type)
        {
            if (type == null) return false;
            //return UnderlyingSystemType.EqualsInternal(type.UnderlyingSystemType);
            return ReferenceEquals(this, type);
        }

        private static Type internal_from_name(string name, bool throwOnError, bool ignoreCase)
        {
            return Reflection.Assembly.Instance.GetType(name, throwOnError, ignoreCase);
        }

        public static Type GetType(string typeName)
        {
            if (typeName == null)
                throw new ArgumentNullException("typeName");

            return internal_from_name(typeName, false, false);
        }

        public static Type GetType(string typeName, bool throwOnError)
        {
            if (typeName == null)
                throw new ArgumentNullException("typeName");

            Type type = internal_from_name(typeName, throwOnError, false);
            if (throwOnError && type == null)
                throw new TypeLoadException("Error loading '" + typeName + "'");

            return type;
        }

        public static Type GetType(string typeName, bool throwOnError, bool ignoreCase)
        {
            if (typeName == null)
                throw new ArgumentNullException("typeName");

            Type t = internal_from_name(typeName, throwOnError, ignoreCase);
            if (throwOnError && t == null)
                throw new TypeLoadException("Error loading '" + typeName + "'");

            return t;
        }

        internal static Type[] GetTypeArray(object[] args)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            Type[] ret;
            ret = new Type[args.Length];
            for (int i = 0; i < args.Length; ++i)
                ret[i] = args[i].GetType();
            return ret;
        }

        public static TypeCode GetTypeCode(Type type)
        {
            if (type == null)
                /* MS.NET returns this */
                return TypeCode.Empty;

            if (type.kind >= KIND_TYPE_CODE)
                return (TypeCode)(type.kind - KIND_TYPE_CODE);

            if (type.kind == KIND_ENUM)
                return (TypeCode)(type.UnderlyingSystemType.kind - KIND_TYPE_CODE);

            return TypeCode.Object;
        }

        //NOTE: This method is required by CLR and CSC
        public static Type GetTypeFromHandle(RuntimeTypeHandle handle)
        {
            throw new NotSupportedException("System.Type.GetTypeFromHandle");
        }

        public virtual bool IsSubclassOf(Type c)
        {
            if (c == null || c == this)
                return false;

            // Fast check for system types
            //if (IsSystemType)
            //    return c.IsSystemType && type_is_subtype_of(this, c, false);

            // User defined types depend on this behavior
            for (Type type = BaseType; type != null; type = type.BaseType)
                if (type == c)
                    return true;

            return false;
        }

#if NOT_PFX
        public virtual Type[] FindInterfaces(TypeFilter filter, object filterCriteria)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");

            ArrayList ifaces = new ArrayList();
            foreach (Type iface in GetInterfaces())
            {
                if (filter(iface, filterCriteria))
                    ifaces.Add(iface);
            }

            return (Type[])ifaces.ToArray(typeof(Type));
        }
#endif

        internal Type GetInterface(string name)
        {
            return GetInterface(name, false);
        }

        internal virtual Type GetInterface(string name, bool ignoreCase)
        {
            foreach (Type iface in GetInterfaces())
            {
                if (string.Compare(iface.FullName, name, ignoreCase) == 0)
                    return iface;
            }
            return null;
        }

#if NOT_PFX
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        internal static extern void GetInterfaceMapData(Type t, Type iface, out MethodInfo[] targets, out MethodInfo[] methods);

#if NET_2_0
		[ComVisible (true)]
#endif
        public virtual InterfaceMapping GetInterfaceMap(Type interfaceType)
        {
            InterfaceMapping res;
            if (interfaceType == null)
                throw new ArgumentNullException("interfaceType");
            if (!interfaceType.IsInterface)
                throw new ArgumentException(Locale.GetText("Argument must be an interface."), "interfaceType");
            res.TargetType = this;
            res.InterfaceType = interfaceType;
            GetInterfaceMapData(this, interfaceType, out res.TargetMethods, out res.InterfaceMethods);
            if (res.TargetMethods == null)
                throw new ArgumentException(Locale.GetText("Interface not found"), "interfaceType");

            return res;
        }
#endif

        public virtual Type[] GetInterfaces()
        {
            if (ifaces == null)
                return new Type[0];

            int n = ifaces.Length;
            Type[] arr = new Type[n];
            for (int i = 0; i < n; ++i)
            {
                int id = ifaces[i];
                arr[i] = Assembly.GetType(id);
            }

            return arr;
        }

        internal bool IsBaseOf(Type t)
        {
            if (t == null) return false;
            Type tb = t.BaseType;
            while (tb != null)
            {
                if (Equals(tb, this))
                    return true;
                tb = tb.BaseType;
            }
            return false;
        }

        internal bool IsImplementedBy(Type t)
        {
            if (t == null) return false;
            foreach (Type iface in t.GetInterfaces())
            {
                if (iface == this)
                    return true;
            }
            return false;
        }

        internal bool Implements(Type iface)
        {
            if (iface == null) return false;
            if (!iface.IsInterface) return false;
            foreach (Type myIface in GetInterfaces())
            {
                if (myIface == iface)
                    return true;
            }
            return false;
        }

        public virtual bool IsAssignableFrom(Type c)
        {
            if (c == null)
                return false;

            if (Equals(c))
                return true;

            if (IsInterface)
                return IsImplementedBy(c);

            return IsBaseOf(c);


            ////if (c is TypeBuilder)
            ////	return ((TypeBuilder)c).IsAssignableTo (this);

            ///* Handle user defined type classes */
            //if (!IsSystemType)
            //{
            //    Type systemType = UnderlyingSystemType;
            //    if (!systemType.IsSystemType)
            //        return false;
            //    return systemType.IsAssignableFrom(c);
            //}

            //if (!c.IsSystemType)
            //{
            //    Type underlyingType = c.UnderlyingSystemType;
            //    if (!underlyingType.IsSystemType)
            //        return false;
            //    return IsAssignableFrom(underlyingType);
            //}

            //throw new NotImplementedException("System.Type.IsAssignableFrom");
        }

        public virtual bool IsInstanceOfType(object o)
        {
            if (o == null) 
                return false;
            return IsAssignableFrom(o.GetType());
        }

        //[MethodImplAttribute(MethodImplOptions.InternalCall)]
        internal static bool type_is_subtype_of(Type a, Type b, bool check_interfaces)
        {
            if (a == null) return false;
            if (b == null) return false;
            if (b.IsInterface)
            {
                if (check_interfaces)
                    return a.Implements(b);
                return false;
            }
            return b.IsBaseOf(a);
        }

        public virtual int GetArrayRank()
        {
            throw new NotSupportedException("Type.GetArrayRank");	// according to MSDN
        }

        internal Type ElementType
        {
            get
            {
                if (elemType < 0) return null;
                return Assembly.GetType(elemType);
            }
        }

        public virtual Type GetElementType()
        {
            return ElementType;
        }

        #region GetMember
        public EventInfo GetEvent(string name)
        {
            return GetEvent(name, DefaultBindingFlags);
        }

        public virtual EventInfo GetEvent(string name, BindingFlags bindingAttr)
        {
            throw new NotImplementedException("Type.GetEvent(string, BinadingFlags)");
        }

        public virtual EventInfo[] GetEvents()
        {
            return GetEvents(DefaultBindingFlags);
        }

        public virtual EventInfo[] GetEvents(BindingFlags bindingAttr)
        {
            throw new NotImplementedException("Type.GetEvents");
        }

        public FieldInfo GetField(string name)
        {
            return GetField(name, DefaultBindingFlags);
        }

        public virtual FieldInfo GetField(string name, BindingFlags bindingAttr)
        {
            throw new NotImplementedException("Type.GetField(string name, BindingFlags bindingAttr)");
        }

        public FieldInfo[] GetFields()
        {
            return GetFields(DefaultBindingFlags);
        }

        public virtual FieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            throw new NotImplementedException("Type.GetFields(BindingFlags bindingAttr)");
        }

        public MemberInfo[] GetMember(string name)
        {
            return GetMember(name, DefaultBindingFlags);
        }

        public virtual MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
        {
            return GetMember(name, MemberTypes.All, bindingAttr);
        }

        public virtual MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
        {
            if ((bindingAttr & BindingFlags.IgnoreCase) != 0)
                return FindMembers(type, bindingAttr, FilterNameIgnoreCase, name);
            else
                return FindMembers(type, bindingAttr, FilterName, name);
        }

        public MemberInfo[] GetMembers()
        {
            return GetMembers(DefaultBindingFlags);
        }

        public virtual MemberInfo[] GetMembers(BindingFlags bindingAttr)
        {
            throw new NotImplementedException("Type.GetMembers(BindingFlags bindingAttr)");
        }

        public MethodInfo GetMethod(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            return GetMethodImpl(name, DefaultBindingFlags, null, CallingConventions.Any, null, null);
        }



        public MethodInfo GetMethod(string name, BindingFlags bindingAttr)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            return GetMethodImpl(name, bindingAttr, null, CallingConventions.Any, null, null);
        }

        public MethodInfo GetMethod(string name, Type[] types)
        {
            return GetMethod(name, DefaultBindingFlags, null, CallingConventions.Any, types, null);
        }

        public MethodInfo GetMethod(string name, Type[] types, ParameterModifier[] modifiers)
        {
            return GetMethod(name, DefaultBindingFlags, null, CallingConventions.Any, types, modifiers);
        }

        public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder,
                                     Type[] types, ParameterModifier[] modifiers)
        {

            return GetMethod(name, bindingAttr, binder, CallingConventions.Any, types, modifiers);
        }

        public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder,
                                     CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (types == null)
                throw new ArgumentNullException("types");

            for (int i = 0; i < types.Length; i++)
                if (types[i] == null)
                    throw new ArgumentNullException("types");

            return GetMethodImpl(name, bindingAttr, binder, callConvention, types, modifiers);
        }

        internal MethodInfo[] Methods { 
            get
            {
                if (m_methods == null)
                {
					object mm = m_methodsInit.call(null);
                    if (mm is MethodInfo[])
                    {
                        m_methods = (MethodInfo[]) mm;
                    }
                    else
                    {
                        throw new InvalidCastException("seems bad");
                    }
                        

                    foreach (var method in m_methods)
                    {
                        method.m_declType = this;
                    }
                }
                return m_methods;
            }
        }

        internal ConstructorInfo[] Constructors
        {
            get
            {

                if (m_constructorsInit == null)
                {
                    Console.WriteLine("m_constructorsInit is null");
                    throw new NullReferenceException();
                }
                if (m_constructors  == null)
                {
					m_constructors = (ConstructorInfo[])m_constructorsInit.call(null);
                    if (m_constructors == null)
                    {
                        Console.WriteLine("Constructors is null");
                        throw new NullReferenceException("ctors is null");
                    }

                    foreach (var ctor in m_constructors)
                    {
                        if (ctor != null)
                        {
                            ctor.m_declType = this;
                        }
                        else
                        {
                            Console.WriteLine("ctor is null in Constructors");
                        }
                    }
                }
                return m_constructors;
            }
        }

        internal Function m_methodsInit;
        internal Function m_constructorsInit;
        

        private bool CheckSig(bool isCtor, MethodBase method, string name, BindingFlags bindingAttr, CallingConventions callingConvention, Type[] types, ParameterModifier[] modifiers)
        {
            if (method.IsGenericMethod)
                return false;
            
            if (!isCtor && method.Name != name)
            {
                    return false;
            }
                
            
            //if (method.CallingConvention != callingConvention)
            //{
            //    throw new InvalidOperationException("method.CallingConvention:" + method.CallingConvention + "convention:" + callingConvention + " name:" + name);
            //    //                return false;
            //}
            

            // check parametr types
            if (types != null)
            {
                var a = method.GetParameters();
                int n = types.Length;
                if (a.Length != n)
                {
                    return false;
                }
                
                for (int i = 0; i < n; i++)
                {
                    if (a[i].GetType() == types[i])
                        return false;
                }
            }

            //if (((bindingAttr & BindingFlags.Public) != 0) && (!method.IsPublic))
            //    return false;
            
            //if (((bindingAttr & BindingFlags.Static) != 0) && (!method.IsStatic))
            //    return false;

            return true;
        }

        protected virtual MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder,
                                                     CallingConventions callConvention, Type[] types,
                                                     ParameterModifier[] modifiers)
        {
            try
            {
                foreach (var method in Methods)
                {
                    if (CheckSig(false, method, name, bindingAttr, callConvention, types, modifiers))
                        return method;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }

            string s = null;
            s += "Something bad occured, name: " + name;
            foreach (var method in Methods)
            {
                s += method.Name + " ##";
            }
            if (s == null)
                s = "null";
            throw new NotImplementedException("Type.GetMethodImpl _________" + s);
        }

        internal MethodInfo GetMethodImplInternal(string name, BindingFlags bindingAttr, Binder binder,
                                                            CallingConventions callConvention, Type[] types,
                                                            ParameterModifier[] modifiers)
        {
            return GetMethodImpl(name, bindingAttr, binder, callConvention, types, modifiers);
        }

        internal virtual MethodInfo GetMethod(MethodInfo fromNoninstanciated)
        {
            throw new InvalidOperationException("can only be called in generic type");
        }

        internal virtual ConstructorInfo GetConstructor(ConstructorInfo fromNoninstanciated)
        {
            throw new InvalidOperationException("can only be called in generic type");
        }

        internal virtual FieldInfo GetField(FieldInfo fromNoninstanciated)
        {
            throw new InvalidOperationException("can only be called in generic type");
        }


        public MethodInfo[] GetMethods()
        {
            return GetMethods(DefaultBindingFlags);
        }

        private MethodInfo[] m_methods;
        private ConstructorInfo[] m_constructors;

        // TODO:
        public virtual MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            return Methods;
            //throw new NotImplementedException("Type.GetMethods(BindingFlags bindingAttr)");
        }

        public Type GetNestedType(string name)
        {
            return GetNestedType(name, DefaultBindingFlags);
        }

        public virtual Type GetNestedType(string name, BindingFlags bindingAttr)
        {
            throw new NotImplementedException("Type.GetNestedType(string name, BindingFlags bindingAttr)");
        }

        public Type[] GetNestedTypes()
        {
            return GetNestedTypes(DefaultBindingFlags);
        }

        public virtual Type[] GetNestedTypes(BindingFlags bindingAttr)
        {
            throw new NotImplementedException("Type.GetNestedTypes(BindingFlags bindingAttr)");
        }

        public PropertyInfo[] GetProperties()
        {
            return GetProperties(DefaultBindingFlags);
        }

        internal Function m_propertiesInit;
        internal PropertyInfo[] m_properties;

        public virtual PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {

            if (m_properties == null)
            {

				m_properties = (PropertyInfo[])m_propertiesInit.call(null);
                if (m_properties == null)
                {
                    Console.WriteLine("m_properties is null");
                }
                

                foreach (var prop in m_properties)
                {
                    prop.m_declType = this;
                }
            }
            
            return m_properties;
            //throw new NotImplementedException("Type.GetProperties(BindingFlags bindingAttr)");
        }


        public PropertyInfo GetProperty(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            return GetPropertyImpl(name, DefaultBindingFlags, null, null, null, null);
        }

        public PropertyInfo GetProperty(string name, BindingFlags bindingAttr)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            return GetPropertyImpl(name, bindingAttr, null, null, null, null);
        }

        public PropertyInfo GetProperty(string name, Type returnType)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            return GetPropertyImpl(name, DefaultBindingFlags, null, returnType, null, null);
        }

        public PropertyInfo GetProperty(string name, Type[] types)
        {
            return GetProperty(name, DefaultBindingFlags, null, null, types, null);
        }

        public PropertyInfo GetProperty(string name, Type returnType, Type[] types)
        {
            return GetProperty(name, DefaultBindingFlags, null, returnType, types, null);
        }

        public PropertyInfo GetProperty(string name, Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            return GetProperty(name, DefaultBindingFlags, null, returnType, types, modifiers);
        }

        public PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType,
                                         Type[] types, ParameterModifier[] modifiers)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (types == null)
                throw new ArgumentNullException("types");

            foreach (Type t in types)
            {
                if (t == null)
                    throw new ArgumentNullException("types");
            }

            return GetPropertyImpl(name, bindingAttr, binder, returnType, types, modifiers);
        }

        
        bool CheckPropertySignature(PropertyInfo pi, string name)
        {
            if (pi.Name != name)
                return false;

            return true;
        }

        protected virtual PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder,
                                                         Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            foreach (var prop in m_properties)
            {
                if (CheckPropertySignature(prop, name))
                    return prop;
            }

            return null;
            //throw new NotImplementedException("Type.GetPropertyImpl");
        }

        internal virtual PropertyInfo GetPropertyImplInternal(string name, BindingFlags bindingAttr, Binder binder,
                                                       Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            return GetPropertyImpl(name, bindingAttr, binder, returnType, types, modifiers);
        }

        protected virtual ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr,
                                       Binder binder,
                                       CallingConventions callConvention,
                                       Type[] types,
                                       ParameterModifier[] modifiers)
        {

            var ctors = Constructors;
            if (ctors == null)
            {
                Console.WriteLine("ctros is null");
                throw  new NullReferenceException();
            }
            foreach (var ctor in ctors)
            {
                if (CheckSig(true, ctor, null, bindingAttr, callConvention, types, modifiers))
                    return ctor;
            }
            throw new NotImplementedException("Type.GetConstructorImpl");
        }

        #endregion

        protected virtual TypeAttributes GetAttributeFlagsImpl()
        {
            return (TypeAttributes)flags;
        }

        protected virtual bool IsCOMObjectImpl()
        {
            return false;
        }

#if NOT_PFX
protected virtual bool IsContextfulImpl()
        {
            return typeof(ContextBoundObject).IsAssignableFrom(this);
        }
#endif

#if NOT_PFX
        protected virtual bool IsMarshalByRefImpl()
        {
            return typeof(MarshalByRefObject).IsAssignableFrom(this);
        }

#endif

#if NET_2_0
		[ComVisible (true)]
#endif
        public ConstructorInfo GetConstructor(Type[] types)
        {
            return GetConstructor(DefaultBindingFlags, null, CallingConventions.Any, types, null);
        }

#if NET_2_0
		[ComVisible (true)]
#endif
        public ConstructorInfo GetConstructor(BindingFlags bindingAttr, Binder binder,
                               Type[] types, ParameterModifier[] modifiers)
        {
            return GetConstructor(bindingAttr, binder, CallingConventions.Any, types, modifiers);
        }

#if NET_2_0
		[ComVisible (true)]
#endif
        public ConstructorInfo GetConstructor(BindingFlags bindingAttr, Binder binder,
                               CallingConventions callConvention,
                               Type[] types, ParameterModifier[] modifiers)
        {
            if (types == null)
                throw new ArgumentNullException("types");

            foreach (Type t in types)
            {
                if (t == null)
                    throw new ArgumentNullException("types");
            }

            return GetConstructorImpl(bindingAttr, binder, callConvention, types, modifiers);
        }

        public ConstructorInfo[] GetConstructors()
        {
            return GetConstructors(BindingFlags.Public | BindingFlags.Instance);
        }

        public virtual ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
        {
            throw new NotImplementedException("Type.GetConstructors(BindingFlags bindingAttr)");
        }

        public virtual MemberInfo[] GetDefaultMembers()
        {
            object[] att = GetCustomAttributes(typeof(DefaultMemberAttribute), true);
            if (att.Length == 0)
                return new MemberInfo[0];

            MemberInfo[] member = GetMember(((DefaultMemberAttribute)att[0]).MemberName);
            return (member != null) ? member : new MemberInfo[0];
        }

        public virtual MemberInfo[] FindMembers(MemberTypes memberType, BindingFlags bindingAttr,
                             MemberFilter filter, object filterCriteria)
        {
            MemberInfo[] result;
            ArrayList l = new ArrayList();

            // Console.WriteLine ("FindMembers for {0} (Type: {1}): {2}",
            // this.FullName, this.GetType().FullName, this.obj_address());

            if ((memberType & MemberTypes.Constructor) != 0)
            {
                ConstructorInfo[] c = GetConstructors(bindingAttr);
                if (filter != null)
                {
                    foreach (MemberInfo m in c)
                    {
                        if (filter(m, filterCriteria))
                            l.Add(m);
                    }
                }
                else
                {
                    l.AddRange(c);
                }
            }
            if ((memberType & MemberTypes.Event) != 0)
            {
                EventInfo[] c = GetEvents(bindingAttr);
                if (filter != null)
                {
                    foreach (MemberInfo m in c)
                    {
                        if (filter(m, filterCriteria))
                            l.Add(m);
                    }
                }
                else
                {
                    l.AddRange(c);
                }
            }
            if ((memberType & MemberTypes.Field) != 0)
            {
                FieldInfo[] c = GetFields(bindingAttr);
                if (filter != null)
                {
                    foreach (MemberInfo m in c)
                    {
                        if (filter(m, filterCriteria))
                            l.Add(m);
                    }
                }
                else
                {
                    l.AddRange(c);
                }
            }
            if ((memberType & MemberTypes.Method) != 0)
            {
                MethodInfo[] c = GetMethods(bindingAttr);
                if (filter != null)
                {
                    foreach (MemberInfo m in c)
                    {
                        if (filter(m, filterCriteria))
                            l.Add(m);
                    }
                }
                else
                {
                    l.AddRange(c);
                }
            }
            if ((memberType & MemberTypes.Property) != 0)
            {
                PropertyInfo[] c;
                int count = l.Count;
                Type ptype;
                if (filter != null)
                {
                    ptype = this;
                    while ((l.Count == count) && (ptype != null))
                    {
                        c = ptype.GetProperties(bindingAttr);
                        foreach (MemberInfo m in c)
                        {
                            if (filter(m, filterCriteria))
                                l.Add(m);
                        }
                        ptype = ptype.BaseType;
                    }
                }
                else
                {
                    c = GetProperties(bindingAttr);
                    l.AddRange(c);
                }
            }
            if ((memberType & MemberTypes.NestedType) != 0)
            {
                Type[] c = GetNestedTypes(bindingAttr);
                if (filter != null)
                {
                    foreach (MemberInfo m in c)
                    {
                        if (filter(m, filterCriteria))
                        {
                            l.Add(m);
                        }
                    }
                }
                else
                {
                    l.AddRange(c);
                }
            }
            result = new MemberInfo[l.Count];
            l.CopyTo(result);
            return result;
        }


        [DebuggerHidden]
        [DebuggerStepThrough]
        public object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args)
        {
            return InvokeMember(name, invokeAttr, binder, target, args, null, null, null);
        }

        [DebuggerHidden]
        [DebuggerStepThrough]
        public object InvokeMember(string name, BindingFlags invokeAttr, Binder binder,
                        object target, object[] args, CultureInfo culture)
        {
            return InvokeMember(name, invokeAttr, binder, target, args, null, culture, null);
        }

        public virtual object InvokeMember(string name, BindingFlags invokeAttr,
                             Binder binder, object target, object[] args,
                             ParameterModifier[] modifiers,
                             CultureInfo culture, string[] namedParameters)
        {
            throw new NotImplementedException("Type.InvokeMember");
        }

        public override string ToString()
        {
            return FullName;
        }

        internal bool IsSystemType
        {
            get
            {
                switch (GetTypeCode(this))
                {
                    case TypeCode.Boolean:
                    case TypeCode.Byte:
                    case TypeCode.Char:
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.SByte:
                    case TypeCode.Single:
                    case TypeCode.String:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        return true;
                }
                return false;
            }
        }

#if NET_2_0 || BOOTSTRAP_NET_2_0
		public virtual Type[] GetGenericArguments ()
		{
			throw new NotSupportedException ();
		}

		public virtual bool ContainsGenericParameters {
			get { return false; }
		}

		public virtual extern bool IsGenericTypeDefinition {
			[MethodImplAttribute(MethodImplOptions.InternalCall)]
			get;
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern Type GetGenericTypeDefinition_impl ();

		public virtual Type GetGenericTypeDefinition ()
		{
			Type res = GetGenericTypeDefinition_impl ();
			if (res == null)
				throw new InvalidOperationException ();

			return res;
		}

		public virtual extern bool IsGenericType {
			[MethodImplAttribute(MethodImplOptions.InternalCall)]
			get;
		}

#if NOT_PFX
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		static extern Type MakeGenericType (Type gt, Type [] types);

		public virtual Type MakeGenericType (params Type[] types)
		{
			if (!IsGenericTypeDefinition)
				throw new InvalidOperationException ("not a generic type definition");
			if (types == null)
				throw new ArgumentNullException ("types");
			foreach (Type t in types) {
				if (t == null)
					throw new ArgumentNullException ("types");
			}
			Type res = MakeGenericType (this, types);
			if (res == null)
				throw new TypeLoadException ();
			return res;
		}
#endif

		public virtual bool IsGenericParameter {
			get {
				return false;
			}
		}

		public bool IsNested {
			get {
				return DeclaringType != null;
			}
		}

		public bool IsVisible {
			get {
				if (IsNestedPublic)
					return DeclaringType.IsVisible;

				return IsPublic;
			}
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern int GetGenericParameterPosition ();
		
		public virtual int GenericParameterPosition {
			get {
				int res = GetGenericParameterPosition ();
				if (res < 0)
					throw new InvalidOperationException ();
				return res;
			}
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern GenericParameterAttributes GetGenericParameterAttributes ();

		public virtual GenericParameterAttributes GenericParameterAttributes {
			get {
				if (!IsGenericParameter)
					throw new InvalidOperationException ();

				return GetGenericParameterAttributes ();
			}
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern Type[] GetGenericParameterConstraints_impl ();

		public virtual Type[] GetGenericParameterConstraints ()
		{
			if (!IsGenericParameter)
				throw new InvalidOperationException ();

			return GetGenericParameterConstraints_impl ();
		}

		public virtual MethodBase DeclaringMethod {
			get {
				return null;
			}
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern Type make_array_type (int rank);

		public virtual Type MakeArrayType ()
		{
			return MakeArrayType (1);
		}

		public virtual Type MakeArrayType (int rank)
		{
			if (rank < 1)
				throw new IndexOutOfRangeException ();
			return make_array_type (rank);
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern Type make_byref_type ();

		public virtual Type MakeByRefType ()
		{
			return make_byref_type ();
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public extern virtual Type MakePointerType ();

		[MonoTODO("Not implemented")]
		public static Type ReflectionOnlyGetType (string typeName, 
							  bool throwIfNotFound, 
							  bool ignoreCase)
		{
			throw new NotImplementedException ();
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern void GetPacking (out int packing, out int size);		

		public virtual StructLayoutAttribute StructLayoutAttribute {
			get {
				LayoutKind kind;

				if (IsLayoutSequential)
					kind = LayoutKind.Sequential;
				else if (IsExplicitLayout)
					kind = LayoutKind.Explicit;
				else
					kind = LayoutKind.Auto;

				StructLayoutAttribute attr = new StructLayoutAttribute (kind);

				if (IsUnicodeClass)
					attr.CharSet = CharSet.Unicode;
				else if (IsAnsiClass)
					attr.CharSet = CharSet.Ansi;
				else
					attr.CharSet = CharSet.Auto;

				if (kind != LayoutKind.Auto)
					GetPacking (out attr.Pack, out attr.Size);

				return attr;
			}
		}

		internal object[] GetPseudoCustomAttributes ()
		{
			int count = 0;

			/* IsSerializable returns true for delegates/enums as well */
			if ((Attributes & TypeAttributes.Serializable) != 0)
				count ++;
			if ((Attributes & TypeAttributes.Import) != 0)
				count ++;

			if (count == 0)
				return null;
			object[] attrs = new object [count];
			count = 0;

			if ((Attributes & TypeAttributes.Serializable) != 0)
				attrs [count ++] = new SerializableAttribute ();
#if NOT_PFX
			if ((Attributes & TypeAttributes.Import) != 0)
				attrs [count ++] = new ComImportAttribute ();
#endif

			return attrs;
		}			

#endif

    }
}
