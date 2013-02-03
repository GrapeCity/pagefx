//CHANGED
//
// System.Reflection/MethodBase.cs
//
// Author:
//   Paolo Molaro (lupus@ximian.com)
//
// (C) 2001 Ximian, Inc.  http://www.ximian.com
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

using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Native;

namespace System.Reflection
{
    [Serializable]
    public abstract class MethodBase : MemberInfo
    {
        internal Type m_declType;
        
        public override Type DeclaringType
        {
            get
            {
                return m_declType;
                //throw new NotImplementedException();
            }
        }

        internal string m_name;

        public override string Name
        {
            get
            {
                return m_name;
                //throw new NotImplementedException();
            }
        }

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public extern static MethodBase GetCurrentMethod();

        public static MethodBase GetMethodFromHandle(RuntimeMethodHandle handle)
        {
            return GetMethodFromHandleInternalType(handle.Value, IntPtr.Zero);
        }

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static MethodBase GetMethodFromHandleInternalType(IntPtr method_handle, IntPtr type_handle);

#if NET_2_0
		[ComVisible (false)]
		public static MethodBase GetMethodFromHandle(RuntimeMethodHandle handle, RuntimeTypeHandle declaringType) {
			return GetMethodFromHandleInternalType (handle.Value, declaringType.Value);
		}
#endif

        public abstract MethodImplAttributes GetMethodImplementationFlags();

        internal ParameterInfo[] m_parameters;

        public virtual ParameterInfo[] GetParameters()
        {
            return m_parameters;
        }

        internal Function m_function;
        
        // FIXME: when this method is uncommented, corlib fails
        // to build
        [DebuggerStepThrough]
        [DebuggerHidden]
        public virtual object Invoke(object obj, object[] parameters)
        {
            NativeArray arr = new NativeArray();

            if (!IsStatic)
                arr.push(obj);

            if (parameters != null)
            {
                int n = parameters.Length;
                if (n != GetParameterCount())
                    throw new TargetParameterCountException("Parameter count mismatch.");

                ParameterInfo[] pi = GetParameters();

                for (int i = 0; i < n; i++)
                {
                    if (pi[i] == null)
                        throw new InvalidOperationException("ParametrInfo is null");

                    object v = pi[i].ParameterType.Unbox(parameters[i]);
                    if (v == null)
                        throw new InvalidCastException("After unbox parameter is null");
                    arr.push(v);
                }
            }
            else
            {
                if (GetParameterCount() != 0)
                    throw new TargetParameterCountException("Parameter count mismatch.");
            }

            return m_function.apply(null, arr);
        }

        
        //
        // This is a quick version for our own use. We should override
        // it where possible so that it does not allocate an array.
        //
        internal virtual int GetParameterCount()
        {
            ParameterInfo[] pi = GetParameters();
            if (pi == null)
                return 0;

            return pi.Length;
        }

#if NOT_PFX
        [DebuggerHidden]
        [DebuggerStepThrough]
#if NET_2_0 || BOOTSTRAP_NET_2_0
		virtual
#endif
        public Object Invoke(Object obj, Object[] parameters)
        {
            return Invoke(obj, 0, null, parameters, null);
        }
#endif

#if NOT_PFX
#if NET_2_0 || BOOTSTRAP_NET_2_0
		Object _MethodBase.Invoke(Object obj, Object[] parameters) {
			return Invoke (obj, parameters);
		}
#endif
#endif

#if NOT_PFX
        public abstract Object Invoke(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture);
#endif

        protected MethodBase()
        {
        }

        public abstract RuntimeMethodHandle MethodHandle { get; }

        internal int m_attributes;
        
        public virtual MethodAttributes Attributes
        {
            get
            {
                return (MethodAttributes) m_attributes;
            }
        }

        public virtual CallingConventions CallingConvention
        {
            get { return CallingConventions.Standard; }
        }

        public bool IsPublic
        {
            get
            {
                return (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public;
            }
        }
        public bool IsPrivate
        {
            get
            {
                return (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Private;
            }
        }
        public bool IsFamily
        {
            get
            {
                return (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Family;
            }
        }
        public bool IsAssembly
        {
            get
            {
                return (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Assembly;
            }
        }
        public bool IsFamilyAndAssembly
        {
            get
            {
                return (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamANDAssem;
            }
        }
        public bool IsFamilyOrAssembly
        {
            get
            {
                return (Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamORAssem;
            }
        }
        public bool IsStatic
        {
            get
            {
                return (Attributes & MethodAttributes.Static) != 0;
            }
        }
        public bool IsFinal
        {
            get
            {
                return (Attributes & MethodAttributes.Final) != 0;
            }
        }
        public bool IsVirtual
        {
            get
            {
                return (Attributes & MethodAttributes.Virtual) != 0;
            }
        }
        public bool IsHideBySig
        {
            get
            {
                return (Attributes & MethodAttributes.HideBySig) != 0;
            }
        }
        public bool IsAbstract
        {
            get
            {
                return (Attributes & MethodAttributes.Abstract) != 0;
            }
        }
        public bool IsSpecialName
        {
            get
            {
                int attr = (int)Attributes;
                return (attr & (int)MethodAttributes.SpecialName) != 0;
            }
        }
        [ComVisibleAttribute(true)]
        public bool IsConstructor
        {
            get
            {
                //int attr = (int)Attributes;
                //return ((attr & (int)MethodAttributes.RTSpecialName) != 0
                //    && (Name == ".ctor"));
                return this is ConstructorInfo;
            }
        }

#if NET_2_0 || BOOTSTRAP_NET_2_0
		[ComVisible (true)]
		public virtual Type [] GetGenericArguments ()
		{
			throw new NotSupportedException ();
		}

		public virtual bool ContainsGenericParameters {
			get {
				return false;
			}
		}

		public virtual bool IsGenericMethodDefinition {
			get {
				return false;
			}
		}

		public virtual bool IsGenericMethod {
			get {
				return false;
			}
		}
#endif

#if NOT_PFX
#if NET_2_0
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		internal extern static MethodBody GetMethodBodyInternal (IntPtr handle);

		internal static MethodBody GetMethodBody (IntPtr handle) {
			return GetMethodBodyInternal (handle);
		}

		public virtual MethodBody GetMethodBody () {
			throw new NotSupportedException ();
		}
#endif
#endif

        internal virtual int get_next_table_index(object obj, int table, bool inc)
        {
            throw new NotSupportedException();
        }
    }
}
