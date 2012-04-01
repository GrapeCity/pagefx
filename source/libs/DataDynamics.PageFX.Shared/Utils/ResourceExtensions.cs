using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DataDynamics
{
    public static class ResourceExtensions
    {
        public static Stream GetResourceStream(this Assembly assembly, string name)
        {
        	return (from fullName in assembly.GetManifestResourceNames()
					where fullName.Contains(name)
					select assembly.GetManifestResourceStream(fullName)).FirstOrDefault();
        }

    	public static Stream GetResourceStream(this Type type, string name)
        {
            return type.Assembly.GetResourceStream(name);
        }

        public static string GetTextResource(this Assembly assembly, string name)
        {
            var rs = assembly.GetResourceStream(name);
            return rs.ReadText();
        }

        public static string GetTextResource(this Type type, string name)
        {
            return type.Assembly.GetTextResource(name);
        }

        public static Image GetImageResource(this Assembly assembly, string subname)
        {
            var rs = assembly.GetResourceStream(subname);
            if (rs == null) return null;
            try
            {
                return Image.FromStream(rs);
            }
            catch
            {
                return null;
            }
        }

        public static Image GetImageResource(this Type type, string subname)
        {
            return type.Assembly.GetImageResource(subname);
        }
    }
}