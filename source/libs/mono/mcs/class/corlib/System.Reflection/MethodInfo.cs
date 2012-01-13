//CHANGED

//
// System.Reflection/MethodInfo.cs
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Reflection {

	[Serializable]
    public class MethodInfo: MethodBase
    {
        public virtual MethodInfo GetBaseDefinition()
        {
            throw new NotImplementedException();
        }

		public MethodInfo() {
		}

	    public override MemberTypes MemberType { get {return MemberTypes.Method;} }
	    
	    public override Type ReflectedType
	    {
	        get { throw new NotImplementedException(); }
	    }

        //TODO:
	    internal int m_returnType;
		public virtual Type ReturnType {
			get
			{
			    return Assembly.GetType(m_returnType);
			}
		}

        internal CustomAttributeProvider m_returnCustomAttributes;

        public virtual ICustomAttributeProvider ReturnTypeCustomAttributes
        {
            get { return m_returnCustomAttributes; }
        }
        

#if NET_2_0 || BOOTSTRAP_NET_2_0
		[ComVisible (true)]
		public virtual MethodInfo GetGenericMethodDefinition ()
		{
			throw new NotSupportedException ();
		}

		public virtual MethodInfo MakeGenericMethod (params Type [] types)
		{
			throw new NotSupportedException ();
		}

		// GetGenericArguments, IsGenericMethod, IsGenericMethodDefinition
		// and ContainsGenericParameters are implemented in the derived classes.
	    public override MethodImplAttributes GetMethodImplementationFlags()
	    {
	        throw new NotImplementedException();
	    }

	    public override RuntimeMethodHandle MethodHandle
	    {
	        get { throw new NotImplementedException(); }
	    }

	    [ComVisible (true)]
		public override Type [] GetGenericArguments () {
			return Type.EmptyTypes;
		}

		public override bool IsGenericMethod {
			get {
				return false;
			}
		}

		public override bool IsGenericMethodDefinition {
			get {
				return false;
			}
		}

		public override bool ContainsGenericParameters {
			get {
				return false;
			}
		}

		public virtual ParameterInfo ReturnParameter {
			get {
				throw new NotSupportedException ();
			}
		}
#endif
	}
}
