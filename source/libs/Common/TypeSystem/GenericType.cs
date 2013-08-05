using System;
using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public class GenericType : TypeImpl
    {
		private readonly GenericParameterCollection _genericParams = new GenericParameterCollection();

		public GenericType()
		{
		}

		public GenericType(IEnumerable<IType> parameters)
		{
			foreach (var parameter in parameters)
			{
				_genericParams.Add(parameter);
			}
		}

    	public override ITypeCollection GenericParameters
        {
            get { return _genericParams; }
        }
        
    	#region Resolver
        public static IType Resolve(IType contextType, IType type)
        {
            return Resolve(contextType, null, type);
        }

        public static IType Resolve(IType contextType, IMethod contextMethod, IType type)
        {
	        if (type == null)
				throw new ArgumentNullException("type");

	        var typeFactory = type.Assembly.TypeFactory;
			switch (type.TypeKind)
            {
                case TypeKind.Array:
                    {
	                    var elemType = Resolve(contextType, contextMethod, type.ElementType);
                        if (!ReferenceEquals(elemType, type.ElementType))
                            return typeFactory.MakeArray(elemType, type.ArrayDimensions);
                        return type;
                    }

                case TypeKind.Pointer:
                    {
	                    var elemType = Resolve(contextType, contextMethod, type.ElementType);
                        if (!ReferenceEquals(elemType, type.ElementType))
                            return typeFactory.MakePointerType(elemType);
                        return type;
                    }

                case TypeKind.Reference:
                    {
	                    var elemType = Resolve(contextType, contextMethod, type.ElementType);
                        if (!ReferenceEquals(elemType, type.ElementType))
                            return typeFactory.MakeReferenceType(elemType);
                        return type;
                    }
            }

	        if (type.IsGenericParameter())
            {
                if (type.DeclaringMethod != null)
                {
                    if (contextMethod.IsGenericInstance)
                        return contextMethod.GenericArguments[type.GetGenericParameterInfo().Position];
                    return type;
                }
				return contextType.GenericArguments[type.GetGenericParameterInfo().Position];
            }

            if (type.IsGenericInstance())
            {
                var args = type.GenericArguments.Select(p => Resolve(contextType, contextMethod, p)).ToList();
            	return typeFactory.MakeGenericType(type.Type, args);
            }

            return type;
        }
        #endregion

        #region Utils

	    public static IMethod FindMethodProxy(IType type, IMethod m)
        {
            foreach (var method in type.Methods)
            {
                if (method.ProxyOf == m)
                    return method;
                if (m is MethodProxy && method.ProxyOf == m.ProxyOf)
                    return method;
            }
            return null;
        }

        public static IField FindFieldProxy(IType type, IField f)
        {
            foreach (var field in type.Fields)
            {
                if (field.ProxyOf == f)
                    return field;
                if (f is FieldProxy && field.ProxyOf == f.ProxyOf)
                    return field;
            }
            return null;
        }

        #endregion

        #region Generic Methods
        static IType ResolveChecked(IType contextType, IMethod contextMethod, IType type)
        {
            var t = Resolve(contextType, contextMethod, type);
            if (t == null)
                throw new InvalidOperationException(
                    string.Format("Unable to resolve type {0}", type));
            if (t.HasGenericParams())
                throw new InvalidOperationException(
                    string.Format("type {0} is not completely instatiated", t));
            return t;
        }

        public static IMethod CreateMethodInstance(IType declType, IMethod method, IType[] args)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            if (method.IsGeneric)
                return Create(declType, method, args);
            //TODO: Check generic method instance
            return method;
        }

        public static IMethod ResolveMethodInstance(IType contextType, IMethod contextMethod, IMethod method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            if (!method.IsGenericInstance)
                return method;

            //if (!HasGenericParams(method.GenericArguments))
            //    return method;

            var declType = method.DeclaringType;
            declType = ResolveChecked(contextType, contextMethod, declType);

            if (declType.IsGenericInstance())
            {
                var proxy = FindMethodProxy(declType, method);
                if (proxy != null)
                    method = proxy;
            }

            int n = method.GenericArguments.Length;
            var args = new IType[n];
            for (int i = 0; i < n; ++i)
            {
                var arg = ResolveChecked(contextType, contextMethod, method.GenericArguments[i]);
                args[i] = arg;
            }

            return Create(declType, method, args);
        }

        private static IMethod Create(IType declType, IMethod method, IType[] args)
        {
            var gm = GenericMethodInstance.Unwrap(method);
            if (gm == null)
                throw new InvalidOperationException();
            if (!gm.IsGeneric)
                throw new InvalidOperationException();
            
            foreach (var m in declType.Methods)
            {
                if (!m.IsGenericInstance) continue;
                if (m.InstanceOf != gm) continue;
                if (Signature.Equals(m.GenericArguments, args))
                    return m;
            }

            var gmi = new GenericMethodInstance(declType, gm, args);
            //gmi.ContextType = contextType;
            //gmi.ContextMethod = contextMethod;
            declType.Methods.Add(gmi);

            return gmi;
        }
        #endregion
    }
}