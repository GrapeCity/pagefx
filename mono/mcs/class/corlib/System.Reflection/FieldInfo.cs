//CHANGED

//
// System.Reflection.FieldInfo.cs
//
// Author:
//   Miguel de Icaza (miguel@ximian.com)
//
// (C) Ximian, Inc.  http://www.ximian.com
// Copyright (C) 2004-2005 Novell, Inc (http://www.novell.com)
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

using System.Diagnostics;
using System.Globalization;
#if NOT_PFX
using System.Reflection.Emit;
#endif
using System.Runtime.CompilerServices;
using Native;

namespace System.Reflection
{
    [Serializable]
    public class FieldInfo : MemberInfo
    {
        internal int type;
		internal Namespace ns;
		internal string name;
		internal int declaringType;
		internal int flags;
		internal bool isStatic;

        public virtual FieldAttributes Attributes
        {
            get { return (FieldAttributes)flags; }
        }

        public virtual RuntimeFieldHandle FieldHandle
        {
            get { throw new NotImplementedException(); }
        }

        public virtual Type FieldType
        {
            get { return Assembly.GetType(type); }
        }

        public override MemberTypes MemberType
        {
            get { return MemberTypes.Field; }
        }

        public override string Name
        {
            get { return name; }
        }

        public override Type DeclaringType
        {
            get { return Assembly.GetType(declaringType); }
        }

        public override Type ReflectedType
        {
            get { return DeclaringType; }
        }

#if NOT_PFX
        internal virtual UnmanagedMarshal UMarshal
        {
            get { return null; }
        }
#endif
        
        #region Flags
        public bool IsLiteral
        {
            get { return (Attributes & FieldAttributes.Literal) != 0; }
        }

        public bool IsStatic
        {
            get
            {
                return isStatic;
                //return (Attributes & FieldAttributes.Static) != 0;
            }
        }

        public bool IsInitOnly
        {
            get { return (Attributes & FieldAttributes.InitOnly) != 0; }
        }

        public bool IsPublic
        {
            get
            {
                return (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public;
            }
        }

        public bool IsPrivate
        {
            get
            {
                return (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Private;
            }
        }
        public bool IsFamily
        {
            get
            {
                return (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Family;
            }
        }
        public bool IsAssembly
        {
            get
            {
                return (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Assembly;
            }
        }

        public bool IsFamilyAndAssembly
        {
            get
            {
                return (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamANDAssem;
            }
        }

        public bool IsFamilyOrAssembly
        {
            get
            {
                return (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamORAssem;
            }
        }

        public bool IsPinvokeImpl
        {
            get
            {
                return (Attributes & FieldAttributes.PinvokeImpl) == FieldAttributes.PinvokeImpl;
            }
        }

        public bool IsSpecialName
        {
            get
            {
                return (Attributes & FieldAttributes.SpecialName) == FieldAttributes.SpecialName;
            }
        }

        public bool IsNotSerialized
        {
            get
            {
                return (Attributes & FieldAttributes.NotSerialized) == FieldAttributes.NotSerialized;
            }
        }
        #endregion

        public virtual object GetValue(object obj)
        {
            Type ft = FieldType;
            object v;
            if (isStatic)
                v = ft.GetFieldValue(ns, name);
            else
                v = avm.GetProperty(obj, ns, name);
            Function box = ft.m_box;
            if (box != null)
                v = box.call(null, v);
            return v;
        }

        public virtual void SetValue(object obj, object value)
        {
            Type ft = FieldType;
            Function unbox = ft.m_unbox;
            if (unbox != null)
            {
                value = unbox.call(null, value);
            }
            if (isStatic)
                ft.SetFieldValue(ns, name, value);
            else
                avm.SetProperty(obj, ns, name, value);
        }

#if NOT_PFX
        public virtual void SetValue(object obj, object val, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        
        public static FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle)
        {
            throw new NotImplementedException();
        }
#endif

#if NOT_PFX
#if NET_2_0
		public static FieldInfo GetFieldFromHandle (RuntimeFieldHandle handle, RuntimeTypeHandle declaringType)
		{
			return internal_from_handle_type (handle.Value, declaringType.Value);
		}
#endif
#endif

        //
        // Note: making this abstract imposes an implementation requirement
        //       on any class that derives from it.  However, since it's also
        //       internal, that means only classes inside corlib can derive
        //       from FieldInfo.  See
        //
        //          errors/cs0534-4.cs errors/CS0534-4-lib.cs
        //
        //          class/Microsoft.JScript/Microsoft.JScript/JSFieldInfo.cs
        //
        internal virtual int GetFieldOffset()
        {
            throw new NotImplementedException();
        }

#if NOT_PFX
        [CLSCompliant(false)]
        public virtual object GetValueDirect(TypedReference obj)
        {
            throw new NotImplementedException();
        }

        [CLSCompliant(false)]
        public virtual void SetValueDirect(TypedReference obj, object value)
        {
            throw new NotImplementedException();
        }
#endif

        internal virtual object[] GetPseudoCustomAttributes()
        {
            throw new NotImplementedException();
        }

#if NET_2_0 || BOOTSTRAP_NET_2_0
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		extern Type[] GetTypeModifiers (bool optional);

		public virtual Type[] GetOptionalCustomModifiers () {
			Type[] types = GetTypeModifiers (true);
			if (types == null)
				return Type.EmptyTypes;
			return types;
		}

		public virtual Type[] GetRequiredCustomModifiers () {
			Type[] types = GetTypeModifiers (false);
			if (types == null)
				return Type.EmptyTypes;
			return types;
		}

		public virtual object GetRawConstantValue ()
		{
			throw new NotSupportedException ("This non-CLS method is not implemented.");
		}
#endif
    }
}
