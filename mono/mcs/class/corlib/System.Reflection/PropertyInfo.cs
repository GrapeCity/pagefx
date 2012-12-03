//
// System.Reflection/PropertyInfo.cs
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
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace System.Reflection
{
	[Serializable]
	public class PropertyInfo : MemberInfo
    {
        // TODO: 
	    public PropertyAttributes Attributes
	    {
	        get
	        {
	            throw new NotImplementedException();
	        }
	    }

        // TODO:
	    public bool CanRead
	    {
	        get
	        {
	            throw new NotImplementedException();
	        }
	    }

        // TODO:
	    public bool CanWrite
	    {
	        get
	        {
	            throw new NotImplementedException();
	        }
	    } 

		public bool IsSpecialName {
			get {return (Attributes & PropertyAttributes.SpecialName) != 0;}
		}

	    internal Type m_declType;

	    public override Type DeclaringType
	    {
	        get
	        {
                return m_declType;
	            //throw new NotImplementedException();
	        }
	    }

	    public override MemberTypes MemberType {
			get {return MemberTypes.Property;}
		}


        public override string ToString()
        {
            if (PropertyType.IsPrimitive)
                return PropertyType.Name + " " + Name;
            
            return PropertyType + " " + Name;
        }


	    internal string m_name;

	    public override string Name
	    {
	        get
	        {
	            return m_name;
            }
	    }

	    public override Type ReflectedType
	    {
	        get
	        {
	            throw new NotImplementedException();
	        }
	    }

        internal int m_propType;
        // TODO:
	    public Type PropertyType
	    {
	        get
	        {
                return Assembly.GetType(m_propType);
	        }
	    } 
	
		protected PropertyInfo () { }

		public MethodInfo[] GetAccessors ()
		{
			return GetAccessors (false);
		}

        // TODO:
		public MethodInfo[] GetAccessors (bool nonPublic)
		{
		    throw new NotImplementedException(); 
		}

		public MethodInfo GetGetMethod()
		{
			return GetGetMethod (false);
		}

	    internal int m_setMethod;
	    internal int m_getMethod;

		public MethodInfo GetGetMethod(bool nonPublic)
		{
		    return DeclaringType.Methods[m_getMethod];
		}
		
		public ParameterInfo[] GetIndexParameters()
		{
		    throw new NotImplementedException();
		}

		public MethodInfo GetSetMethod()
		{
			return GetSetMethod (false);
		}
		
		public MethodInfo GetSetMethod (bool nonPublic)
		{
		    return DeclaringType.Methods[m_setMethod];
		}


	    internal Avm.Function m_getValue;

		[DebuggerHidden]
		[DebuggerStepThrough]
		public virtual object GetValue (object obj, object[] index)
		{
            if (index != null)
            {
                throw new NotImplementedException();
            }
            
		    return null;
			//return GetValue(obj, BindingFlags.Default, null, index, null);
		}

		
#if NOT_PFX
		public abstract object GetValue (object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);
#endif
		
#if NOT_PFX
		[DebuggerHidden]
		[DebuggerStepThrough]
		public virtual void SetValue (object obj, object value, object[] index)
		{
			SetValue (obj, value, BindingFlags.Default, null, index, null);
		}
#endif
		
#if NOT_PFX
		public abstract void SetValue (object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);
#endif

#if NET_2_0 || BOOTSTRAP_NET_2_0

		public virtual Type[] GetOptionalCustomModifiers () {
			return Type.EmptyTypes;
		}

		public virtual Type[] GetRequiredCustomModifiers () {
			return Type.EmptyTypes;
		}

		[MonoTODO("Not implemented")]
		public virtual object GetConstantValue () {
			throw new NotImplementedException ();
		}

		[MonoTODO("Not implemented")]
		public virtual object GetRawConstantValue() {
			throw new NotImplementedException ();
		}
#endif
	}
}
