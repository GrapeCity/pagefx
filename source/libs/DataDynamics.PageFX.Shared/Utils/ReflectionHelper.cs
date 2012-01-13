using System;
using System.Reflection;

namespace DataDynamics
{
    public static class ReflectionHelper
    {
        public static T[] GetAttributes<T>(ICustomAttributeProvider provider, bool inherit)
            where T : Attribute
        {
            return (T[])provider.GetCustomAttributes(typeof(T), inherit);
        }

        public static T GetAttribute<T>(ICustomAttributeProvider provider, bool inherit)
            where T : Attribute
        {
            var attrs = GetAttributes<T>(provider, inherit);
            if (attrs != null && attrs.Length > 0)
                return attrs[0];
            return null;
        }
    }
}