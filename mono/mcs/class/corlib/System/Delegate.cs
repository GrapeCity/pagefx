//CHANGED
//
// System.Delegate.cs
//
// Authors:
//   Miguel de Icaza (miguel@ximian.com)
//   Daniel Stodden (stodden@in.tum.de)
//   Dietmar Maurer (dietmar@ximian.com)
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

using System.Reflection;
using System.Runtime.CompilerServices;
using Native;

namespace System
{
    public abstract class Delegate : ICloneable
    {
        internal object m_target;
        internal Function m_function;
    	private MethodInfo _method;

	    public object Function
	    {
			get { return m_function; }
	    }

        public MethodInfo Method
        {
            get { return _method ?? (_method = new MethodInfo {m_function = m_function}); }
        }

        public object Target
        {
            get { return m_target; }
        }

        #region CreateDelegate Methods
#if NOT_PFX
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern Delegate CreateDelegate_internal(Type type, object target, MethodInfo info);

#if NET_2_0
        public
#else
        internal
#endif
 static Delegate CreateDelegate(Type type, MethodInfo method, bool throwOnBindFailure)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (method == null)
                throw new ArgumentNullException("method");

            if (!type.IsSubclassOf(typeof(MulticastDelegate)))
                throw new ArgumentException("type is not a subclass of Multicastdelegate");

            if (!method.IsStatic)
                if (throwOnBindFailure)
                    throw new ArgumentException("The method should be static.", "method");
                else
                    return null;

            MethodInfo invoke = type.GetMethod("Invoke");

            // FIXME: Check the return type on the 1.0 profile as well
#if NET_2_0
            Type returnType = method.ReturnType;
            Type delReturnType = invoke.ReturnType;
            bool returnMatch = returnType == delReturnType;

            if (!returnMatch) {
                // Delegate covariance
                if (!delReturnType.IsValueType && (delReturnType != typeof (ValueType)) && (delReturnType.IsAssignableFrom (returnType)))
                    returnMatch = true;
            }

            if (!returnMatch)
                if (throwOnBindFailure)
                    throw new ArgumentException ("method return type is incompatible");
                else
                    return null;
#endif

            ParameterInfo[] delargs = invoke.GetParameters();
            ParameterInfo[] args = method.GetParameters();

            if (args.Length != delargs.Length)
                if (throwOnBindFailure)
                    throw new ArgumentException("method argument length mismatch");
                else
                    return null;

            int length = delargs.Length;
            for (int i = 0; i < length; i++)
            {
                bool match = delargs[i].ParameterType == args[i].ParameterType;

#if NET_2_0
                // Delegate contravariance
                if (!match) {
                    Type argType = delargs [i].ParameterType;

                    if (!argType.IsValueType && (argType != typeof (ValueType)) && (args [i].ParameterType.IsAssignableFrom (argType)))
                        match = true;
                }
#endif

                if (!match)
                    if (throwOnBindFailure)
                        throw new ArgumentException("method arguments are incompatible");
                    else
                        return null;
            }

            Delegate d = CreateDelegate_internal(type, null, method);
#if !AVM
            d.original_method_info = method;
#endif
            return d;
        }

        public static Delegate CreateDelegate(Type type, MethodInfo method)
        {
            return CreateDelegate(type, method, true);
        }

#if NET_2_0
        public
#else
        internal
#endif
 static Delegate CreateDelegate(Type type, object target, MethodInfo method, bool throwOnBindFailure)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (method == null)
                throw new ArgumentNullException("method");

            if (!type.IsSubclassOf(typeof(MulticastDelegate)))
                throw new ArgumentException("type is not a subclass of Multicastdelegate");

            Delegate d = CreateDelegate_internal(type, target, method);
#if !AVM
            d.original_method_info = method;
#endif
            return d;

        }

#if NET_2_0
        public
#else
        internal
#endif
 static Delegate CreateDelegate(Type type, object target, MethodInfo method)
        {
            return CreateDelegate(type, target, method, true);
        }

        public static Delegate CreateDelegate(Type type, object target, string method)
        {
            return CreateDelegate(type, target, method, false);
        }

#if NET_2_0
        public 
#else
        internal
#endif
 static Delegate CreateDelegate(Type type, Type target, string method, bool ignoreCase, bool throwOnBindFailure)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (target == null)
                throw new ArgumentNullException("target");

            if (method == null)
                throw new ArgumentNullException("method");

            if (!type.IsSubclassOf(typeof(MulticastDelegate)))
                throw new ArgumentException("type is not subclass of MulticastDelegate.");

            ParameterInfo[] delargs = type.GetMethod("Invoke").GetParameters();
            Type[] delargtypes = new Type[delargs.Length];

            for (int i = 0; i < delargs.Length; i++)
                delargtypes[i] = delargs[i].ParameterType;

            /* 
             * FIXME: we should check the caller has reflection permission
             * or if it lives in the same assembly...
             */
            BindingFlags flags = BindingFlags.ExactBinding | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;
            if (ignoreCase)
                flags |= BindingFlags.IgnoreCase;
            MethodInfo info = target.GetMethod(method, flags, null, delargtypes, new ParameterModifier[0]);

            if (info == null)
            {
                if (throwOnBindFailure)
                    throw new ArgumentException("Couldn't bind to method.");
                else
                    return null;
            }

            return CreateDelegate_internal(type, null, info);
        }

        public static Delegate CreateDelegate(Type type, Type target, string method)
        {
            return CreateDelegate(type, target, method, false, true);
        }

#if NET_2_0
        public static Delegate CreateDelegate (Type type, Type target, string method, bool ignoreCase) {
            return CreateDelegate (type, target, method, ignoreCase, true);
        }
#endif

#if NET_2_0
        public
#else
        internal
#endif
 static Delegate CreateDelegate(Type type, object target, string method, bool ignoreCase, bool throwOnBindFailure)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (target == null)
                throw new ArgumentNullException("target");

            if (method == null)
                throw new ArgumentNullException("method");

            if (!type.IsSubclassOf(typeof(MulticastDelegate)))
                throw new ArgumentException("type");

            ParameterInfo[] delargs = type.GetMethod("Invoke").GetParameters();
            Type[] delargtypes = new Type[delargs.Length];

            for (int i = 0; i < delargs.Length; i++)
                delargtypes[i] = delargs[i].ParameterType;

            /* 
             * FIXME: we should check the caller has reflection permission
             * or if it lives in the same assembly...
             */
            BindingFlags flags = BindingFlags.ExactBinding | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

            if (ignoreCase)
                flags |= BindingFlags.IgnoreCase;

            MethodInfo info = target.GetType().GetMethod(method, flags, null, delargtypes, new ParameterModifier[0]);

            if (info == null)
                if (throwOnBindFailure)
                    throw new ArgumentException("Couldn't bind to method '" + method + "'.");
                else
                    return null;

            return CreateDelegate_internal(type, target, info);
        }

        public static Delegate CreateDelegate(Type type, object target, string method, bool ignoreCase)
        {
            return CreateDelegate(type, target, method, ignoreCase, true);
        }
#endif
        #endregion

        #region DynamicInvoke
#if NET_2_0
		public object DynamicInvoke (params object[] args)
#else
        public object DynamicInvoke(object[] args)
#endif
        {
            //return DynamicInvokeImpl(args);
			return m_function.apply(m_target, args.m_value);
        }

        protected virtual object DynamicInvokeImpl(object[] args)
        {
			return m_function.apply(m_target, args.m_value);
        }
        #endregion

		protected virtual Delegate CreateInstance()
		{
			return (Delegate)GetType().CreateInstanceDefault();
		}

        public virtual object Clone()
        {
            Delegate d = CreateInstance();
            d.m_target = m_target;
            d.m_function = m_function;
            return d;
        }

        public override bool Equals(object obj)
        {
            Delegate d = obj as Delegate;

            if (d == null)
                return false;

            //MONO
            // Do not compare method_ptr, since it can point to a trampoline
            //if ((d.target_type == target_type) && (d.m_target == m_target) &&
            //    (d.method_name == method_name) && (d.method_info == method_info))
            //    return true;

            if (m_target == d.m_target && m_function == d.m_function)
                return true;

            return false;
        }

        public virtual Delegate[] GetInvocationList()
        {
            return new Delegate[] { this };
        }

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void AddEventListener(object dispatcher, string eventName, object f);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RemoveEventListener(object dispatcher, string eventName, object f);
		
        public virtual void AddEventListeners(object dispatcher, string eventName)
        {
            AddEventListener(dispatcher, eventName, m_function);
        }

        public virtual void RemoveEventListeners(object dispatcher, string eventName)
        {
            RemoveEventListener(dispatcher, eventName, m_function);
        }

        public virtual int GetInvocationCount()
        {
            return 1;
        }

        /// <symmary>
        ///   Returns a new MulticastDelegate holding the
        ///   concatenated invocation lists of MulticastDelegates a and b
        /// </symmary>
        public static Delegate Combine(Delegate a, Delegate b)
        {
            if (a == null)
            {
                if (b == null)
                    return null;
                return b;
            }
            else
                if (b == null)
                    return a;

            if (a.GetType() != b.GetType())
                throw new ArgumentException(Locale.GetText("Incompatible Delegate Types."));

            return a.CombineImpl(b);
        }

        /// <symmary>
        ///   Returns a new MulticastDelegate holding the
        ///   concatenated invocation lists of an Array of MulticastDelegates
        /// </symmary>
#if NET_2_0
		[System.Runtime.InteropServices.ComVisible (true)]
		public static Delegate Combine (params Delegate[] delegates)
#else
        public static Delegate Combine(Delegate[] delegates)
#endif
        {
            if (delegates == null)
                return null;

            Delegate retval = null;

            foreach (Delegate next in delegates)
                retval = Combine(retval, next);

            return retval;
        }

        protected virtual Delegate CombineImpl(Delegate d)
        {
            throw new MulticastNotSupportedException("");
        }

        public static Delegate Remove(Delegate source, Delegate value)
        {
            if (source == null)
                return null;

            return source.RemoveImpl(value);
        }

        protected virtual Delegate RemoveImpl(Delegate d)
        {
            if (Equals(d))
                return null;

            return this;
        }

#if NET_1_1
        public static Delegate RemoveAll(Delegate source, Delegate value)
        {
            Delegate tmp = source;
            while ((source = Remove(source, value)) != tmp)
                tmp = source;

            return tmp;
        }
#endif

        public static bool operator ==(Delegate a, Delegate b)
        {
            if ((object)a == null)
            {
                if ((object)b == null)
                    return true;
                return false;
            }
            else if ((object)b == null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Delegate a, Delegate b)
        {
            return !(a == b);
        }

//        public static implicit operator Function(Delegate d)
//        {
//            if (d == null)
//                throw new ArgumentNullException("d");
//            if (d.GetInvocationCount() > 1)
//                throw new ArgumentException("Unable cast delegate to function. Delegates has more than one invocated functions.");
//            return d.m_function;
//        }
    }
}
