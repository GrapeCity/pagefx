//
// System.Reflection.ICustomAttributeProvider.cs
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

using System.Collections.Generic;
using System.Runtime.InteropServices;
using Native;

namespace System.Reflection
{

    public class CustomAttributeProvider : ICustomAttributeProvider
    {
        internal object[] m_customAttrs;

        public Function m_customAttrsInit;

        internal object[] CustomAttrs
        {
            get
            {
                if (m_customAttrs == null)
                    m_customAttrs = (object[])m_customAttrsInit.call(null);
                return m_customAttrs;
            }
        }

        /// <summary>
        ///   Probes whether one or more `attribute_type' types are
        ///   defined by this member
        /// </summary>
        public bool IsDefined(Type attribute_type, bool inherit)
        {
            if (inherit)
            {
                var thisType = this as Type;
                while (thisType != null)
                {
                    if (thisType.IsDefined(attribute_type))
                        return true;
                    thisType = thisType.BaseType;
                }
                throw new NotImplementedException("IsDefined currently allowed only for types");
            }

            return false;
        }

        internal bool IsDefined(Type attribute_type)
        {
            foreach (var attr in CustomAttrs)
            {
                var type = attr.GetType();
                if (type == attribute_type)
                    return true;
            }
            return false;
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            if (inherit)
            {
                var type = ((object)this) as Type;
                if (type != null)
                {
                    List<object> ca = new List<object>();
                    while (type != null)
                    {
                        ca.AddRange(type.CustomAttrs);
                        type = type.BaseType;
                    }
                    return ca.ToArray();
                }
                throw new NotImplementedException("GetCustomAttributes(bool inherit)");
            }

            return CustomAttrs;
        }


        public object[] GetCustomAttributes(Type attribute_type, bool inherit)
        {
            if (inherit)
            {
                var type = ((object)this) as Type;
                if (type != null)
                {
                    List<object> ca = new List<object>();
                    while (type != null)
                    {
                        foreach (var attr in type.CustomAttrs)
                        {
                            if (attr.GetType() == attribute_type)
                                ca.Add(attr);
                        }
                        type = type.BaseType;
                    }
                    return ca.ToArray();
                }
                throw new NotImplementedException("GetCustomAttributes(Type attribute_type, bool inherit)");
            }

            return CustomAttrs;
        }
    }
}
