using System;
using System.Collections.Generic;
using System.Text;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.FLI
{
    internal static class NameExtensions
    {
        /// <summary>
        /// Removes bad characters from name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToValidName(this string name)
        {
            var sb = new StringBuilder();
            foreach (var c in name)
            {
                switch (c)
                {
                    case '\\':
                    case '/':
                        sb.Append('!');
                        break;

                    case '.':
                        sb.Append('_');
                        break;

                    //TODO: Complete

                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }

        public static string GetTypeNamespace(this IType type, string rootns)
        {
            string ns = type.Namespace;
            if (string.IsNullOrEmpty(ns))
                ns = "";
            if (type.Visibility == Visibility.Internal)
                ns = type.Assembly.Name + "$" + ns;

            if (!string.IsNullOrEmpty(rootns))
            {
                if (type.HasRootNamespace())
                {
                    if (ns.Length == 0) return rootns;
                    return rootns + "." + ns;
                }
            }

            return ns;
        }

        public static string GetFullName(this ITypeMember member)
        {
            var type = member.DeclaringType;
            if (type == null)
                throw new ArgumentException("Member has no declaring type.");
            return type.FullName + "." + member.Name;
        }

        public static string GetParametersSignature(this IMethod method)
        {
            var sb = new StringBuilder();
            AppendParamsSig(sb, method);
            return sb.ToString();
        }

        private static void AppendParamsSig(StringBuilder sb, IMethod method)
        {
        	int n = method.Parameters.Count;
            for (int i = 0; i < n; ++i)
            {
                if (i > 0) sb.Append("_");
                var p = method.Parameters[i];
                string pt = p.Type.GetSigName();
                sb.Append(pt);
            }
        }

        private static string GetSigName(this IEnumerable<IType> types, string prefix, string suffix, string sep)
        {
        	return types.Join(prefix, suffix, sep, type => type.GetSigName());
        }

        public static string GetQName(this ICustomAttributeProvider provider)
        {
            var type = provider as IType;
            if (type != null)
                return type.GetSigName();

            var method = provider as IMethod;
            if (method != null)
            return method.DeclaringType.GetSigName() + "_" + method.GetMethodName() ;

            var field = provider as IField;
            if (field != null)
                return field.DeclaringType.GetSigName() + "_" + field.Name;

            return null;
        }

        public static string GetSigName(this IType type)
        {
        	return type == null ? "" : type.SigName;
        }

    	private static bool NeedDeclaringTypePrefix(this IMethod method)
        {
            if (method.IsInternalCall)
            {
                var type = method.DeclaringType;
                if (type.IsArray) return true;
            }
            return method.IsNew();
        }

        private static bool NeedReturnTypePrefix(this IMethod method)
        {
            //Note: this is fix for cast operators
            if (method.IsStatic && method.Name.StartsWith("op_"))
                return true;
            var bm = method.BaseMethod;
            if (bm != null)
            {
                if (bm.Type != method.Type)
                    return true;
            }
            return false;
        }

        public static string GetStaticCtorName(this IType type)
        {
            return CLRNames.StaticConstructor;
        }

        private static string GetSig(this IMethod method)
        {
            var declType = method.DeclaringType;
            var sb = new StringBuilder();
            bool addParams = true;
            if (method.IsConstructor)
            {
                if (!method.IsStatic)
                    sb.Append(declType.Name);
                sb.Append(method.Name);
            }
            else
            {
                if (method.NeedDeclaringTypePrefix())
                {
                    sb.Append(declType.GetSigName());
                    sb.Append("_");
                }

                if (method.NeedReturnTypePrefix())
                {
                    sb.Append(method.Type.GetSigName());
                    sb.Append("_");
                }

                var prop = method.Association as IProperty;
                if (prop != null && method.IsAccessor())
                {
                    sb.Append(prop.Name);
                    addParams = false;
                }
                else
                {
                    sb.Append(method.Name);
                }
            }

            if (method.IsGenericInstance)
            {
                sb.Append(method.GenericArguments.GetSigName("[", "]", ","));
            }

            if (addParams && method.Parameters.Count > 0)
            {
                sb.Append("_");
                AppendParamsSig(sb, method);
            }
            return sb.ToString();
        }

        public static string GetMethodName(this IMethod method)
        {
            var iface = method.GetInterfaceOfExplicitImpl();
            if (iface != null)
                return iface.GetMethodName();

            string sig = method.GetSig();
            var type = method.DeclaringType;
            if (type.IsInterface)
                return type.GetSigName() + "." + sig;

            return sig;
        }

    	public static string GetMethodName(this IType type, string name, int argcount)
        {
            var m = type.FindMethod(name, argcount);
            if (m == null)
                throw new ArgumentException(string.Format("Unable to find method {0} in type {1}", name, type.FullName));
            return m.GetMethodName();
        }
    }
}