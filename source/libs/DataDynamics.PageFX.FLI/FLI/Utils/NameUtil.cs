using System;
using System.Collections.Generic;
using System.Text;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.FLI
{
    class NameUtil
    {
        /// <summary>
        /// Removes bad characters from name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string MakeGoodName(string name)
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

        public static string GetTypeNamespace(string rootns, IType type)
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

        public static string GetFullName(ITypeMember m)
        {
            var type = m.DeclaringType;
            if (type == null)
                throw new ArgumentException("Member has no declaring type.");
            return type.FullName + "." + m.Name;
        }

        public static string GetParamsString(IMethod method)
        {
            var sb = new StringBuilder();
            AppendParams(sb, method);
            return sb.ToString();
        }

        private static void AppendParams(StringBuilder sb, IMethod method)
        {
            int n = method.Parameters.Count;
            for (int i = 0; i < n; ++i)
            {
                if (i > 0) sb.Append("_");
                var p = method.Parameters[i];
                string pt = GetSigName(p.Type);
                sb.Append(pt);
            }
        }

        public static string GetTypesString(IEnumerable<IType> types, string prefix, string suffix, string sep)
        {
            var sb = new StringBuilder();
            if (prefix != null)
                sb.Append(prefix);
            bool f = false;
            foreach (var type in types)
            {
                if (f) sb.Append(sep);
                sb.Append(GetSigName(type));
                f = true;
            }
            if (suffix != null)
                sb.Append(suffix);
            return sb.ToString();
        }

        public static string GetQName(ICustomAttributeProvider provider)
        {
            var type = provider as IType;
            if (type != null)
                return GetSigName(type);

            var method = provider as IMethod;
            if (method != null)
            return GetSigName(method.DeclaringType) + "_" + GetMethodName(method) ;

            var field = provider as IField;
            if (field != null)
                return GetSigName(field.DeclaringType) + "_" + field.Name;

            return null;
        }

        public static string GetSigName(IType type)
        {
            if (type == null) return "";
            return type.SigName;
        }

        private static bool NeedDeclaringTypePrefix(IMethod method)
        {
            if (method.IsInternalCall)
            {
                var type = method.DeclaringType;
                if (type.IsArray) return true;
            }
            return method.IsNew();
        }

        private static bool NeedReturnTypePrefix(IMethod method)
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

        public static string GetStaticCtorName(IType type)
        {
            return CLRNames.StaticConstructor;
        }

        private static string GetSig(IMethod method)
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
                if (NeedDeclaringTypePrefix(method))
                {
                    sb.Append(GetSigName(declType));
                    sb.Append("_");
                }

                if (NeedReturnTypePrefix(method))
                {
                    sb.Append(GetSigName(method.Type));
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
                sb.Append(GetTypesString(method.GenericArguments, "[", "]", ","));
            }

            if (addParams && method.Parameters.Count > 0)
            {
                sb.Append("_");
                AppendParams(sb, method);
            }
            return sb.ToString();
        }

        public static string GetMethodName(IMethod method)
        {
            var iface = method.GetInterfaceOfExplicitImpl();
            if (iface != null)
                return GetMethodName(iface);

            string sig = GetSig(method);
            var type = method.DeclaringType;
            if (type.IsInterface)
                return GetSigName(type) + "." + sig;

            return sig;
        }

        public static string GetMethodName(IType type, string name)
        {
            var m = type.FindMethod(name);
            if (m == null)
                throw new ArgumentException(string.Format("Unable to find method {0} in type {1}", name, type.FullName));
            return GetMethodName(m);
        }

        public static string GetMethodName(IType type, string name, int argcount)
        {
            var m = type.FindMethod(name, argcount);
            if (m == null)
                throw new ArgumentException(string.Format("Unable to find method {0} in type {1}", name, type.FullName));
            return GetMethodName(m);
        }
    }
}