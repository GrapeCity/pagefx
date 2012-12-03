//CHANGED
// System.Reflection.ParameterInfo
//
// Sean MacIsaac (macisaac@ximian.com)
//
// (C) 2001 Ximian, Inc.
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
//

#if NOT_PFX
using System.Reflection.Emit;
#endif
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Reflection
{
    [Serializable]
    public class ParameterInfo : CustomAttributeProvider 
    {
        
        protected Type ClassImpl
        {
            get
            {
                return Assembly.GetType(m_class_impl);
            }
        }

        internal int m_class_impl;
        
        

        protected object DefaultValueImpl;
        protected MemberInfo MemberImpl;
        protected string NameImpl;
        protected int PositionImpl;
        protected ParameterAttributes AttrsImpl;
#if NOT_PFX
        private UnmanagedMarshal marshalAs;
#endif

        protected ParameterInfo()
        {
        }
#if NOT_PFX
        internal ParameterInfo(ParameterBuilder pb, Type type, MemberInfo member, int position)
        {
            ClassImpl = type;
            MemberImpl = member;
            if (pb != null)
            {
                NameImpl = pb.Name;
                PositionImpl = pb.Position - 1;	// ParameterInfo.Position is zero-based
                AttrsImpl = (ParameterAttributes)pb.Attributes;
            }
            else
            {
                NameImpl = "";
                PositionImpl = position - 1;
                AttrsImpl = ParameterAttributes.None;
            }
        }

        /* to build a ParameterInfo for the return type of a method */
        internal ParameterInfo(Type type, MemberInfo member, UnmanagedMarshal marshalAs)
        {
            ClassImpl = type;
            MemberImpl = member;
            NameImpl = "";
            PositionImpl = -1;	// since parameter positions are zero-based, return type pos is -1
            AttrsImpl = ParameterAttributes.Retval;
            this.marshalAs = marshalAs;
        }
#endif
        public override string ToString()
        {
            Type elementType = ClassImpl;
            if (elementType == null)
                throw new InvalidProgramException("1");

            while (elementType.HasElementType)
            {
                elementType = elementType.GetElementType();
                if (elementType == null)
                    throw new InvalidOperationException("2");
            }
            
            if (ClassImpl == null)
                throw new InvalidOperationException("3");

            bool useShort = elementType.IsPrimitive || ClassImpl == typeof (void);
                //|| ClassImpl.Namespace == MemberImpl.DeclaringType.Namespace;
            
            string result = useShort
                ? ClassImpl.Name
                : ClassImpl.FullName;
            // MS.NET seems to skip this check and produce an extra space for return types
            if (!IsRetval)
            {
                result += ' ';
                result += NameImpl;
            }
            return result;
        }

        public virtual Type ParameterType
        {
            get { return ClassImpl; }
        }
        public virtual ParameterAttributes Attributes
        {
            get { return AttrsImpl; }
        }
        public virtual object DefaultValue
        {
            get { return DefaultValueImpl; }
        }

        public bool IsIn
        {
            get
            {
#if NET_2_0
				return (Attributes & ParameterAttributes.In) != 0;
#else
                return (AttrsImpl & ParameterAttributes.In) != 0;
#endif
            }
        }

        public bool IsLcid
        {
            get
            {
#if NET_2_0
				return (Attributes & ParameterAttributes.Lcid) != 0;
#else
                return (AttrsImpl & ParameterAttributes.Lcid) != 0;
#endif
            }
        }

        public bool IsOptional
        {
            get
            {
#if NET_2_0
				return (Attributes & ParameterAttributes.Optional) != 0;
#else
                return (AttrsImpl & ParameterAttributes.Optional) != 0;
#endif
            }
        }

        public bool IsOut
        {
            get
            {
#if NET_2_0
				return (Attributes & ParameterAttributes.Out) != 0;
#else
                return (AttrsImpl & ParameterAttributes.Out) != 0;
#endif
            }
        }

        public bool IsRetval
        {
            get
            {
#if NET_2_0
				return (Attributes & ParameterAttributes.Retval) != 0;
#else
                return (AttrsImpl & ParameterAttributes.Retval) != 0;
#endif
            }
        }

        public virtual MemberInfo Member
        {
            get { return MemberImpl; }
        }

        public virtual string Name
        {
            get { return NameImpl; }
        }

        public virtual int Position
        {
            get { return PositionImpl; }
        }

#if NET_2_0 || BOOTSTRAP_NET_2_0
		public
#else
        internal
#endif
 extern int MetadataToken
        {
            [MethodImplAttribute(MethodImplOptions.InternalCall)]
            get;
        }
      
        internal object[] GetPseudoCustomAttributes()
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

		[MonoTODO]
		public virtual object RawDefaultValue {
			get {
				throw new NotImplementedException ();
			}
		}
#endif
    }
}
