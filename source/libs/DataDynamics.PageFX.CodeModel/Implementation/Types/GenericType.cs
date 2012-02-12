using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataDynamics.PageFX.CodeModel
{
    public class GenericType : UserDefinedType, IGenericType
    {
        #region IGenericType Members
        public IGenericParameterCollection GenericParameters
        {
            get { return _genericParams; }
        }
        readonly GenericParameterCollection _genericParams = new GenericParameterCollection();
        #endregion

        #region Resolver
        public static IType Resolve(IType contextType, IType type)
        {
            return Resolve(contextType, null, type);
        }

        public static IType Resolve(IType contextType, IMethod contextMethod, IType type)
        {
            switch (type.TypeKind)
            {
                case TypeKind.Array:
                    {
                        var arrayType = (IArrayType)type;
                        var elemType = Resolve(contextType, contextMethod, arrayType.ElementType);
                        if (elemType != arrayType.ElementType)
                            return TypeFactory.MakeArray(elemType, arrayType.Dimensions);
                        return type;
                    }

                case TypeKind.Pointer:
                    {
                        var ct = (ICompoundType)type;
                        var elemType = Resolve(contextType, contextMethod, ct.ElementType);
                        if (elemType != ct.ElementType)
                            return TypeFactory.MakePointerType(elemType);
                        return type;
                    }

                case TypeKind.Reference:
                    {
                        var ct = (ICompoundType)type;
                        var elemType = Resolve(contextType, contextMethod, ct.ElementType);
                        if (elemType != ct.ElementType)
                            return TypeFactory.MakeReferenceType(elemType);
                        return type;
                    }
            }

            var gp = type as IGenericParameter;
            if (gp != null)
            {
                if (gp.DeclaringMethod != null)
                {
                    if (contextMethod.IsGenericInstance)
                        return contextMethod.GenericArguments[gp.Position];
                    return gp;
                }
                return ((IGenericInstance)contextType).GenericArguments[gp.Position];
            }

            var gt = type as IGenericInstance;
            if (gt != null)
            {
                var args = gt.GenericArguments.Select(p => Resolve(contextType, contextMethod, p)).ToList();
            	return TypeFactory.MakeGenericType(gt.Type, args);
            }
            return type;
        }
        #endregion

        #region Utils
        public static bool HasGenericParams(IEnumerable<IType> set)
        {
            if (set == null) return false;
            return Algorithms.TrueAny(set, HasGenericParams);
        }

        public static bool HasGenericParams(IType type)
        {
            if (type == null) return false;
            if (type is IGenericType) return true;

            switch (type.TypeKind)
            {
                case TypeKind.GenericParameter:
                    return true;

                case TypeKind.Array:
                case TypeKind.Pointer:
                case TypeKind.Reference:
                    return HasGenericParams(((ICompoundType)type).ElementType);
            }

            var gi = type as IGenericInstance;
            if (gi != null)
            {
            	return gi.GenericArguments.Any(HasGenericParams);
            }

            return false;
        }

        public static bool IsGenericContext(ITypeMember member)
        {
            var method = member as IMethod;
            if (method != null)
            {
                if (method.IsGeneric)
                    return true;
                if (method.IsGenericInstance)
                {
                    if (HasGenericParams(method.GenericArguments))
                        return true;
                }
                if (method.Parameters.Any(p => HasGenericParams(p.Type)))
                {
                	return true;
                }
            }

            var type = member as IType;
            if (type != null)
            {
                if (HasGenericParams(type))
                    return true;
            }

            if (HasGenericParams(member.DeclaringType))
                return true;

            if (HasGenericParams(member.Type))
                return true;

            return false;
        }

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
            //return Algorithms.Find(gi.Methods, i => i.ProxyOfMethod == m);
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
            //return Algorithms.Find(gi.Fields, i => i.ProxyOfField == f);
        }
        #endregion

        #region Generic Methods
        static IType ResolveChecked(IType contextType, IMethod contextMethod, IType type)
        {
            var t = Resolve(contextType, contextMethod, type);
            if (t == null)
                throw new InvalidOperationException(
                    string.Format("Unable to resolve type {0}", type));
            if (HasGenericParams(t))
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

            var gi = declType as IGenericInstance;
            if (gi != null)
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

        static IMethod Create(IType declType, IMethod method, IType[] args)
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

        #region DisplayName
        public override string DisplayName
        {
            get
            {
            	return _displayName ?? (_displayName = ToDisplayName(FullName)
            	                                       + Format(_genericParams, TypeNameKind.DisplayName, false));
            }
        }
        private string _displayName;

        internal static string ToDisplayName(string name)
        {
            return ToDisplayName(name, false);
        }

        internal static string ToDisplayName(string name, bool sig)
        {
            if (name == null) return null;
            int n = name.Length;
            if (n <= 2)
            {
                return name;
            }

            var sb = new StringBuilder();
            for (int i = 0; i < n; ++i)
            {
                char c = name[i];
                if (c == '`')
                {
                    int j = i + 1;
                    for (; j < n; ++j)
                    {
                        if (!char.IsDigit(name[j]))
                            break;
                    }
                    if (j == i + 1)
                        sb.Append(c);
                    else
                        i = j - 1;
                }
                else
                {
                    if (sig)
                    {
                        switch (c)
                        {
                            case '.':
                                sb.Append('_');
                                continue;
                        }
                    }

                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
        #endregion

        static readonly string SigPrefix = ((int)'[').ToString("X2");
        static readonly string SigSuffix = ((int)']').ToString("X2");

        internal static string Format(IEnumerable<IType> args, TypeNameKind kind, bool clr)
        {
            bool sig = kind == TypeNameKind.SigName;
            string prefix, suffix, sep;
            if (sig)
            {
                prefix = SigPrefix;
                suffix = SigSuffix;
                sep = "_";
            }
            else
            {
                prefix = clr ? "[" : "<";
                suffix = clr ? "]" : ">";
                sep = ",";
            }
            return Format(args, t => GetName(t, kind), prefix, suffix, sep);
        }

        internal static string Format(IEnumerable<IGenericParameter> args, TypeNameKind kind, bool clr)
        {
            return Format(Algorithms.Convert<IGenericParameter, IType>(args), kind, clr);
        }

        public override string Key
        {
            get
            {
                if (_key == null)
                    _key = FullNameBase + Format(_genericParams, TypeNameKind.Key, true);
                return base.Key;
            }
        }
        string _key;
    }
}