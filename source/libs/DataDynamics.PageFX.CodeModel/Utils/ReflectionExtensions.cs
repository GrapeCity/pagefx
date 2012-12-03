using System;
using System.Reflection;

namespace DataDynamics
{
    public static class ReflectionExtensions
    {
        public static T[] GetAttributes<T>(this ICustomAttributeProvider provider, bool inherit)
            where T : Attribute
        {
            return (T[])provider.GetCustomAttributes(typeof(T), inherit);
        }

        public static T GetAttribute<T>(this ICustomAttributeProvider provider, bool inherit)
            where T : Attribute
        {
            var attrs = provider.GetAttributes<T>(inherit);
            if (attrs != null && attrs.Length > 0)
                return attrs[0];
            return null;
        }
    }
}