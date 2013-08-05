using System;
using System.Text;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.FlashLand.Core
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

	    public static string GetQName(this ICustomAttributeProvider provider)
        {
            var type = provider as IType;
            if (type != null)
                return type.GetSigName();

            var method = provider as IMethod;
            if (method != null)
				return method.DeclaringType.GetSigName() + "_" + method.GetSigName(Runtime.Avm);

            var field = provider as IField;
            if (field != null)
                return field.DeclaringType.GetSigName() + "_" + field.Name;

            return null;
        }
    }
}